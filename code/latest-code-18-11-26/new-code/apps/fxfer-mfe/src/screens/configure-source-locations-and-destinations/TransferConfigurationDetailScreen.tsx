import { useParams } from 'react-router-dom';

export function TransferConfigurationDetailScreen() {
  const { sourceLocationId } = useParams<{ sourceLocationId: string }>();
  return (
    <main>
      <h1>Transfer Configuration</h1>
      <p>sourceLocationId: <code>{sourceLocationId}</code></p>
    </main>
  );
}

