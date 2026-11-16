import { z } from 'zod';

const RuntimeConfigSchema = z.object({
  bffBaseUrl: z.string(),
});

export type RuntimeConfig = z.infer<typeof RuntimeConfigSchema>;

export async function loadRuntimeConfig(): Promise<RuntimeConfig> {
  const res = await fetch('/runtime-config.json', { cache: 'no-store' });
  const json = await res.json();
  return RuntimeConfigSchema.parse(json);
}

