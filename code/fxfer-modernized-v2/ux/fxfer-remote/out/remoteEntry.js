// src/remote/App.tsx
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { useMemo, useState } from "react";

// src/flows/manage-ftp-servers/FtpServersScreen.tsx
function FtpServersScreen() {
  return /* @__PURE__ */ React.createElement("section", { className: "space-y-2" }, /* @__PURE__ */ React.createElement("h2", { className: "text-lg font-heading" }, "FTP Servers"), /* @__PURE__ */ React.createElement("p", { className: "text-sm" }, "Placeholder screen; will call fxfer-bff."));
}

// src/flows/manage-source-locations/TransferConfigurationDetailScreen.tsx
function TransferConfigurationDetailScreen({
  transferConfigurationId
}) {
  return /* @__PURE__ */ React.createElement("section", { className: "space-y-2" }, /* @__PURE__ */ React.createElement("h2", { className: "text-lg font-heading" }, "Transfer Configuration"), /* @__PURE__ */ React.createElement("p", { className: "text-sm" }, "ID: ", transferConfigurationId), /* @__PURE__ */ React.createElement("p", { className: "text-sm" }, "Placeholder screen; will call fxfer-bff."));
}

// src/flows/manage-source-locations/TransferConfigurationsScreen.tsx
function TransferConfigurationsScreen() {
  return /* @__PURE__ */ React.createElement("section", { className: "space-y-2" }, /* @__PURE__ */ React.createElement("h2", { className: "text-lg font-heading" }, "Transfer Configurations"), /* @__PURE__ */ React.createElement("p", { className: "text-sm" }, "Placeholder screen; will call fxfer-bff."));
}

// src/lib/auth/FxferRemoteContext.tsx
import { createContext, useContext } from "react";
var FxferRemoteContext = createContext(null);
function FxferRemoteContextProvider({
  value,
  children
}) {
  return /* @__PURE__ */ React.createElement(FxferRemoteContext.Provider, { value }, children);
}

// src/remote/App.tsx
function resolveRoute() {
  if (typeof window === "undefined") {
    return { kind: "index" };
  }
  const pathname = window.location.pathname;
  const rel = pathname.startsWith("/fxfer") ? pathname.slice("/fxfer".length) : pathname;
  const clean = rel.replace(/\/$/, "");
  if (clean === "" || clean === "/") {
    return { kind: "index" };
  }
  if (clean.startsWith("/ftp-servers")) {
    return { kind: "ftp-servers" };
  }
  if (clean === "/transfer-configurations") {
    return { kind: "transfer-configurations" };
  }
  const match = clean.match(/^\/transfer-configurations\/(.+)$/);
  if (match) {
    return { kind: "transfer-configuration", id: match[1] };
  }
  return { kind: "not-found", path: clean };
}
function App({ context }) {
  const [queryClient] = useState(
    () => new QueryClient({
      defaultOptions: {
        queries: {
          retry: 2
        },
        mutations: {
          retry: false
        }
      }
    })
  );
  const route = useMemo(resolveRoute, []);
  return /* @__PURE__ */ React.createElement(FxferRemoteContextProvider, { value: context }, /* @__PURE__ */ React.createElement(QueryClientProvider, { client: queryClient }, /* @__PURE__ */ React.createElement("div", { className: "p-6 space-y-4" }, /* @__PURE__ */ React.createElement("header", null, /* @__PURE__ */ React.createElement("h1", { className: "text-xl font-heading" }, "FXFER")), route.kind === "index" ? /* @__PURE__ */ React.createElement("div", { className: "text-sm" }, "Select a route under /fxfer/*.") : null, route.kind === "ftp-servers" ? /* @__PURE__ */ React.createElement(FtpServersScreen, null) : null, route.kind === "transfer-configurations" ? /* @__PURE__ */ React.createElement(TransferConfigurationsScreen, null) : null, route.kind === "transfer-configuration" ? /* @__PURE__ */ React.createElement(TransferConfigurationDetailScreen, { transferConfigurationId: route.id }) : null, route.kind === "not-found" ? /* @__PURE__ */ React.createElement("div", { className: "text-sm" }, "Not found: ", route.path) : null)));
}

// src/remote/remoteEntry.ts
var moduleMap = {
  "./App": async () => () => App
};
async function get(module) {
  const factory = moduleMap[module];
  if (!factory) {
    throw new Error(`Unknown module: ${module}`);
  }
  return factory();
}
function init() {
}
var container = { get, init };
globalThis.fxferRemote = container;
var remoteEntry_default = container;
export {
  remoteEntry_default as default,
  get,
  init
};
//# sourceMappingURL=remoteEntry.js.map
