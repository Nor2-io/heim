import { defineConfig } from "vite";

export default defineConfig({
  build: {
    emptyOutDir: false,
    outDir: "../heim-dist",
    lib: {
      entry: "../heim-dist/main.js",
      name: "main",
      fileName: "main",
      formats: ["es"],
    },
    rollupOptions: {
      output: {
        globals: {
          "wasi:http/types@0.2.1": "wasi:http",
          "wasi:cli/environment@0.2.1": "wasi:cli",
          "wasi:clocks/monotonic-clock@0.2.1": "wasi:clocks",
          "wasi:clocks/wall-clock@0.2.1": "wasi:clocks",
          "wasi:random/random@0.2.1": "wasi:random"
        },
      },
      // make sure to externalize deps that shouldn't be bundled
      // into your library
      external: [
        "wasi:http/types@0.2.1",
        "wasi:cli/environment@0.2.1",
        "wasi:clocks/monotonic-clock@0.2.1",
        "wasi:clocks/wall-clock@0.2.1",
        "wasi:random/random@0.2.1"],
    },
  },
});
