import type { NextPage } from 'next';
import { BrowserRouter } from 'react-router-dom';
import { FxferRoutes } from '@/FxferRoutes';

const FxferCatchAllPage: NextPage = () => {
  if (typeof window === 'undefined') {
    return null;
  }

  return (
    <BrowserRouter basename="/fxfer">
      <FxferRoutes />
    </BrowserRouter>
  );
};

export default FxferCatchAllPage;

