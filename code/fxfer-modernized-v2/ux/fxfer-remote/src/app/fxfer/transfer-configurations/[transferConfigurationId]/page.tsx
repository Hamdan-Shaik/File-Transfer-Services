import { TransferConfigurationDetailScreen } from '../../../../flows/manage-source-locations/TransferConfigurationDetailScreen';

export function generateStaticParams() {
  return [{ transferConfigurationId: 'example' }];
}

export default async function TransferConfigurationPage({
  params,
}: {
  params: Promise<{ transferConfigurationId: string }>;
}) {
  const { transferConfigurationId } = await params;
  return <TransferConfigurationDetailScreen transferConfigurationId={transferConfigurationId} />;
}

