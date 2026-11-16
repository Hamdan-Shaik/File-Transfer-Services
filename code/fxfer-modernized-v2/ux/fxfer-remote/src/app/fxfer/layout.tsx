import type { ReactNode } from 'react';

export default function FxferLayout({ children }: { children: ReactNode }) {
  return (
    <main className='p-6 space-y-6'>
      <header>
        <h1 className='text-xl font-heading'>FXFER</h1>
      </header>
      {children}
    </main>
  );
}

