using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests.Specs;

public sealed class RunFileTransferBatchFeatureTests
{
    public static IEnumerable<object[]> WiredScenarios
    {
        get
        {
            var featurePath = Path.Combine(AppContext.BaseDirectory, "specs", "run-file-transfer-batch.feature");
            var featureText = File.ReadAllText(featurePath);

            var scenarios = ParseFeature(featureText);

            var wiredRuleIds = new HashSet<string>(StringComparer.Ordinal)
            {
                "F03-001",
                "F03-002",
                "F03-013",
                "F03-021",
                "F03-028",
            };

            foreach (var scenario in scenarios)
            {
                var primaryRuleId = scenario.RuleIds.FirstOrDefault(wiredRuleIds.Contains);
                if (primaryRuleId is null)
                {
                    continue;
                }

                yield return new object[] { primaryRuleId, scenario.Name, scenario.Steps.ToArray() };
            }
        }
    }

    [Theory]
    [MemberData(nameof(WiredScenarios))]
    public void run_file_transfer_batch_feature_wired_scenarios(string ruleId, string scenarioName, string[] steps)
    {
        var stepDefinitions = RunFileTransferBatchSteps.Create();
        var world = new RunFileTransferBatchSteps.World();

        foreach (var stepText in steps)
        {
            var matched = stepDefinitions
                .Select(d => (def: d, match: d.Pattern.Match(stepText)))
                .FirstOrDefault(x => x.match.Success);

            if (matched.def is null)
            {
                throw new InvalidOperationException($"Missing step definition: {ruleId} '{stepText}'");
            }

            matched.def.Run(world, matched.match).GetAwaiter().GetResult();
        }
    }

    private sealed class ParsedScenario
    {
        public List<string> RuleIds { get; } = new();
        public required string Name { get; init; }
        public List<string> Steps { get; } = new();
        public bool IsOutline { get; init; }
        public List<Dictionary<string, string>> Examples { get; } = new();
    }

    private static List<ParsedScenario> ParseFeature(string featureText)
    {
        var scenarios = new List<ParsedScenario>();
        var lines = featureText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

        var pendingRuleIds = new List<string>();
        ParsedScenario? current = null;
        var inExamples = false;
        string[]? exampleHeaders = null;

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var ruleMatches = Regex.Matches(line, @"@rule:([A-Z0-9-]+)");
            if (ruleMatches.Count > 0)
            {
                pendingRuleIds.AddRange(ruleMatches.Select(m => m.Groups[1].Value));
                continue;
            }

            var scenarioMatch = Regex.Match(line, @"^Scenario(?: Outline)?:\s*(.*)$");
            if (scenarioMatch.Success)
            {
                current = new ParsedScenario
                {
                    Name = scenarioMatch.Groups[1].Value,
                    IsOutline = line.StartsWith("Scenario Outline:", StringComparison.Ordinal),
                };

                current.RuleIds.AddRange(pendingRuleIds);
                pendingRuleIds.Clear();

                scenarios.Add(current);
                inExamples = false;
                exampleHeaders = null;
                continue;
            }

            if (current is null)
            {
                continue;
            }

            if (Regex.IsMatch(line, "^Examples:\\s*$"))
            {
                inExamples = true;
                continue;
            }

            if (inExamples && line.StartsWith('|'))
            {
                var cells = line.Trim('|').Split('|').Select(c => c.Trim()).ToArray();
                if (exampleHeaders is null)
                {
                    exampleHeaders = cells;
                }
                else
                {
                    var row = new Dictionary<string, string>(StringComparer.Ordinal);
                    for (var i = 0; i < exampleHeaders.Length && i < cells.Length; i++)
                    {
                        row[exampleHeaders[i]] = cells[i];
                    }

                    current.Examples.Add(row);
                }

                continue;
            }

            var stepMatch = Regex.Match(line, @"^(Given|When|Then|And|But)\s+(.*)$");
            if (stepMatch.Success)
            {
                current.Steps.Add(stepMatch.Groups[2].Value);
            }
        }

        // Expand outlines.
        var expanded = new List<ParsedScenario>();
        foreach (var scenario in scenarios)
        {
            if (!scenario.IsOutline || scenario.Examples.Count == 0)
            {
                expanded.Add(scenario);
                continue;
            }

            foreach (var example in scenario.Examples)
            {
                var nameSuffix = string.Join(", ", example.Select(kv => $"{kv.Key}={kv.Value}"));
                var copy = new ParsedScenario
                {
                    Name = $"{scenario.Name} ({nameSuffix})",
                    IsOutline = false,
                };
                copy.RuleIds.AddRange(scenario.RuleIds);

                foreach (var step in scenario.Steps)
                {
                    var expandedStep = step;
                    foreach (var kv in example)
                    {
                        expandedStep = expandedStep.Replace($"<{kv.Key}>", kv.Value, StringComparison.Ordinal);
                    }

                    copy.Steps.Add(expandedStep);
                }

                expanded.Add(copy);
            }
        }

        return expanded;
    }
}