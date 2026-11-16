import path from 'node:path';

import {
  createManageSourceLocationsSteps,
  loadManageSourceLocationsFeatureText,
} from './manage-source-locations.steps';

type ParsedScenario = {
  ruleId: string;
  name: string;
  steps: string[];
};

function parseFeature(featureText: string): ParsedScenario[] {
  const scenarios: ParsedScenario[] = [];
  const lines = featureText.split(/\r?\n/);

  let pendingRuleId: string | null = null;
  let current: ParsedScenario | null = null;

  for (const rawLine of lines) {
    const line = rawLine.trim();
    if (line === '' || line.startsWith('#')) continue;

    const ruleMatch = line.match(/^@rule:([A-Z0-9-]+)/);
    if (ruleMatch) {
      pendingRuleId = ruleMatch[1];
      continue;
    }

    const scenarioMatch = line.match(/^Scenario(?: Outline)?:\s*(.*)$/);
    if (scenarioMatch) {
      current = {
        ruleId: pendingRuleId ?? 'UNSPECIFIED',
        name: scenarioMatch[1] ?? '',
        steps: [],
      };
      scenarios.push(current);
      pendingRuleId = null;
      continue;
    }

    if (!current) continue;

    const stepMatch = line.match(/^(Given|When|Then|And|But)\s+(.*)$/);
    if (stepMatch) {
      current.steps.push(stepMatch[2]);
    }
  }

  return scenarios;
}

const featurePath = path.resolve('tests/unit/specs/manage-source-locations.feature');
const featureText = loadManageSourceLocationsFeatureText(featurePath);
const scenarios = parseFeature(featureText);

const wiredRuleIds = new Set(['F02-003', 'F02-004']);

describe('manage-source-locations.feature (wired scenarios)', () => {
  const steps = createManageSourceLocationsSteps();

  for (const scenario of scenarios.filter((s) => wiredRuleIds.has(s.ruleId))) {
    test(`${scenario.ruleId} ${scenario.name}`, async () => {
      vi.restoreAllMocks();

      const world = {} as Record<string, unknown>;
      for (const stepText of scenario.steps) {
        const step = steps.find((s) => s.text === stepText);
        if (!step) {
          throw new Error(`Missing step definition: ${scenario.ruleId} '${stepText}'`);
        }
        await step.run(world as any);
      }
    });
  }
});

