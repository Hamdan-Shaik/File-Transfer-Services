import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { FxferRoutes } from '@/FxferRoutes';

describe('fxfer-mfe smoke', () => {
  it('renders a known route', () => {
    render(
      <MemoryRouter initialEntries={['/source-locations']}>
        <FxferRoutes />
      </MemoryRouter>
    );

    expect(screen.getByRole('heading', { name: 'Source Locations' })).toBeInTheDocument();
  });
});

