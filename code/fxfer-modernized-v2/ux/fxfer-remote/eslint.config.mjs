import { FlatCompat } from '@eslint/eslintrc';
import js from '@eslint/js';
import tailwindcss from 'eslint-plugin-tailwindcss';

const compat = new FlatCompat({
  baseDirectory: import.meta.dirname,
});

export default [
  js.configs.recommended,
  ...compat.extends('next/core-web-vitals', 'next/typescript'),
  {
    plugins: { tailwindcss },
    rules: {
      'tailwindcss/classnames-order': 'warn',
      'tailwindcss/no-arbitrary-value': 'warn',
      'tailwindcss/no-contradicting-classname': 'error',
    },
  },
];

