import type { Config } from 'tailwindcss';

const config: Config = {
  content: ['./src/**/*.{ts,tsx}'],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Arial', 'Helvetica Neue', 'Helvetica', 'sans-serif'],
        heading: ['Poppins Bold', 'Arial', 'Helvetica', 'sans-serif'],
        mono: ['monospace'],
      },
    },
  },
  plugins: [],
};

export default config;

