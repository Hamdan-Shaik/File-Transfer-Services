import { type ReactNode, useEffect, useId, useRef } from 'react';

export function Modal(props: {
  title: string;
  onClose: () => void;
  children: ReactNode;
  width?: number;
}) {
  const titleId = useId();
  const dialogRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const root = dialogRef.current;
    if (!root) {
      return;
    }

    const focusable = root.querySelector<HTMLElement>(
      'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    focusable?.focus();

    const onKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        e.preventDefault();
        props.onClose();
      }
    };

    document.addEventListener('keydown', onKeyDown);
    return () => {
      document.removeEventListener('keydown', onKeyDown);
    };
  }, [props]);

  return (
    <div
      role={'presentation'}
      style={{
        position: 'fixed',
        inset: 0,
        background: 'rgba(0, 0, 0, 0.45)',
        display: 'flex',
        alignItems: 'flex-start',
        justifyContent: 'center',
        padding: 24,
        overflow: 'auto',
        zIndex: 50
      }}
      onMouseDown={(e) => {
        if (e.target === e.currentTarget) {
          props.onClose();
        }
      }}
    >
      <div
        ref={dialogRef}
        role={'dialog'}
        aria-modal={true}
        aria-labelledby={titleId}
        style={{
          background: '#fff',
          borderRadius: 8,
          width: props.width ?? 720,
          maxWidth: '95vw',
          padding: 16,
          boxShadow: '0 10px 30px rgba(0,0,0,0.35)'
        }}
      >
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 12 }}>
          <h2 id={titleId} style={{ margin: 0, fontSize: 18 }}>
            {props.title}
          </h2>
          <button type={'button'} onClick={props.onClose} aria-label={'Close'}>
            Close
          </button>
        </div>
        <div style={{ marginTop: 16 }}>{props.children}</div>
      </div>
    </div>
  );
}
