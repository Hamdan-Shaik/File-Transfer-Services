import { NextFederationPlugin } from '@module-federation/nextjs-mf';

// Required when using a locally-installed `webpack` with Next.js 15+.
process.env.NEXT_PRIVATE_LOCAL_WEBPACK ||= 'true';

class StripNodeProtocolPlugin {
  apply(compiler) {
    compiler.hooks.normalModuleFactory.tap('StripNodeProtocolPlugin', (factory) => {
      factory.hooks.beforeResolve.tap('StripNodeProtocolPlugin', (result) => {
        if (!result) return;
        if (typeof result.request === 'string' && result.request.startsWith('node:')) {
          result.request = result.request.slice('node:'.length);
        }
        // Mutate `result` only; bailing hook must return `undefined`.
      });
    });
  }
}

/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  trailingSlash: true,
  output: 'export',
  images: {
    unoptimized: true
  },
  webpack: (config, options) => {
    if (!options.isServer) {
      config.resolve ??= {};
      config.resolve.fallback = {
        ...(config.resolve.fallback ?? {}),
        module: false
      };
      config.plugins.push(new StripNodeProtocolPlugin());

      config.plugins.push(
        new NextFederationPlugin({
          name: 'fxfer',
          filename: 'static/chunks/remoteEntry.js',
          exposes: {
            './FxferApp': './src/FxferApp',
            './FxferRoutes': './src/FxferRoutes'
          },
          shared: {
            react: { singleton: true, requiredVersion: false },
            'react-dom': { singleton: true, requiredVersion: false },
            'react-router-dom': { singleton: true, requiredVersion: false }
          }
        })
      );
    }

    return config;
  }
};

export default nextConfig;

