import { App } from './App';

type ExposedModule = './App';

const moduleMap: Record<ExposedModule, () => Promise<() => unknown>> = {
  './App': async () => () => App,
};

export async function get(module: string) {
  const factory = moduleMap[module as ExposedModule];
  if (!factory) {
    throw new Error(`Unknown module: ${module}`);
  }
  return factory();
}

// Module Federation containers accept an init call for shared scopes.
export function init() {}

const container = { get, init };
// eslint-disable-next-line @typescript-eslint/no-explicit-any
(globalThis as any).fxferRemote = container;

export default container;

