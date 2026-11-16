'use client';

export function TransferConfigurationDetailScreen({
  transferConfigurationId,
}: {
  transferConfigurationId: string;
}) {
  return (
    <section className='space-y-2'>
      <h2 className='text-lg font-heading'>Transfer Configuration</h2>
      <p className='text-sm'>ID: {transferConfigurationId}</p>
      <p className='text-sm'>Placeholder screen; will call fxfer-bff.</p>
    </section>
  );
}

