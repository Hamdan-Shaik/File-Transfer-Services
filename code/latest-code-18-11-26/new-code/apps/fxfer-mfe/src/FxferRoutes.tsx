import { Navigate, Route, Routes } from 'react-router-dom';
import { SearchSourceLocationsScreen } from '@/screens/configure-source-locations-and-destinations/SearchSourceLocationsScreen';
import { SearchFtpServerCredentialsScreen } from '@/screens/configure-source-locations-and-destinations/SearchFtpServerCredentialsScreen';
import { SearchFileTransferTrackingScreen } from '@/screens/configure-source-locations-and-destinations/SearchFileTransferTrackingScreen';
import { FileTrackingScreen } from '@/screens/file-tracking/FileTrackingScreen';
import { FileTrackingDetailScreen } from '@/screens/file-tracking/FileTrackingDetailScreen';
import { TransferConfigurationDetailScreen } from '@/screens/configure-source-locations-and-destinations/TransferConfigurationDetailScreen';
import { TransferConfigurationEditScreen } from '@/screens/configure-source-locations-and-destinations/TransferConfigurationEditScreen';
import { FtpServerDetailScreen } from '@/screens/manage-ftp-servers-and-credentials/FtpServerDetailScreen';

export function FxferRoutes() {
  return (
    <Routes>
      <Route path="source-locations" element={<SearchSourceLocationsScreen />} />
      <Route path="source-locations/:sourceLocationId" element={<TransferConfigurationDetailScreen />} />
      <Route path="source-locations/:sourceLocationId/edit" element={<TransferConfigurationEditScreen />} />

      <Route path="ftp-servers" element={<SearchFtpServerCredentialsScreen />} />
      <Route path="ftp-servers/:ftpServerId" element={<FtpServerDetailScreen />} />

      <Route path="file-tracking" element={<FileTrackingScreen />} />
      <Route path="file-tracking/:fileId" element={<FileTrackingDetailScreen />} />
      <Route path="file-tracking-legacy" element={<SearchFileTransferTrackingScreen />} />

      <Route path="" element={<Navigate to="source-locations" replace />} />
      <Route path="*" element={<Navigate to="source-locations" replace />} />
    </Routes>
  );
}
