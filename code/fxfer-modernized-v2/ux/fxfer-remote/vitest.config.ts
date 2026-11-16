import { defineConfig } from 'vitest/config';

export default defineConfig({
  esbuild: {
    jsx: 'automatic',
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: ['./tests/unit/setup.ts'],
    include: ['./tests/unit/**/*.test.{ts,tsx}'],
  },
});

