import type { ReactNode } from 'react';

import { Providers } from '../providers/Providers';
import '../styles/globals.css';

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang='en'>
      <body className='font-sans'>
        <Providers>{children}</Providers>
      </body>
    </html>
  );
}

