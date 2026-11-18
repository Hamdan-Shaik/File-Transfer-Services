import { FxferBffClient, FxferBffError } from '@/api/FxferBffClient';
import type { InboundTargetMetadata } from '@/api/FxferBffClient';

export class FteMetadataApi {
  public static async getFteMetadatas(contractName: string): Promise<InboundTargetMetadata[]> {
    try {
      return await FxferBffClient.searchInboundTargets(contractName);
    } catch (e: unknown) {
      if (e instanceof FxferBffError && e.problem.status === 404) {
        return [];
      }
      throw e;
    }
  }
}

