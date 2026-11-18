import { useParams } from 'react-router-dom';

export function FtpServerDetailScreen() {
  const { ftpServerId } = useParams<{ ftpServerId: string }>();
  return (
    <main>
      <h1>FTP Server Detail</h1>
      <p>ftpServerId: <code>{ftpServerId}</code></p>
    </main>
  );
}

