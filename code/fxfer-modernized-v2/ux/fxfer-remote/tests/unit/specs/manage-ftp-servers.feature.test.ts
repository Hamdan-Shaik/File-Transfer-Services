import path from 'node:path';

import { createManageFtpServersSteps, loadManageFtpServersFeatureText } from './manage-ftp-servers.steps';
import { setFileTransferApiContext } from '../../../src/flows/manage-ftp-servers/api/fileTransferApi';

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

const featurePath = path.resolve('tests/unit/specs/manage-ftp-servers.feature');
const featureText = loadManageFtpServersFeatureText(featurePath);
const scenarios = parseFeature(featureText);

const wiredRuleIds = new Set([
  'F01-003',
  'F01-014',
  'F01-031',
  'F01-038',
  'F01-040',
  'F01-041',
  'F01-049',
  'F01-050',
  'F01-051',
]);

describe('manage-ftp-servers.feature (wired scenarios)', () => {
  const steps = createManageFtpServersSteps();

  for (const scenario of scenarios.filter((s) => wiredRuleIds.has(s.ruleId))) {
    test(`${scenario.ruleId} ${scenario.name}`, async () => {
      vi.restoreAllMocks();

      // Many scenarios assume a signed-in caller; default the API context.
      setFileTransferApiContext({ accessToken: 'token', bffBaseUrl: 'https://bff.example.test' });

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

