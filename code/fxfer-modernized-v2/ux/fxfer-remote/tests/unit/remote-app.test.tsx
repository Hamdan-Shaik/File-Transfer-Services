import React from 'react';
import { render, screen } from '@testing-library/react';

import { App } from '../../src/remote/App';

test('renders ftp servers screen for /fxfer/ftp-servers', () => {
  window.history.pushState({}, '', '/fxfer/ftp-servers');

  render(
    <App
      context={{
        accessToken: 'test-token',
        bffBaseUrl: 'http://localhost',
        correlationId: 'corr',
      }}
    />,
  );

  expect(screen.getByText('FTP Servers')).toBeInTheDocument();
});

