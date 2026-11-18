import { useParams } from 'react-router-dom';

export function TransferConfigurationEditScreen() {
  const { sourceLocationId } = useParams<{ sourceLocationId: string }>();
  return (
    <main>
      <h1>Edit Transfer Configuration</h1>
      <p>sourceLocationId: <code>{sourceLocationId}</code></p>
    </main>
  );
}

