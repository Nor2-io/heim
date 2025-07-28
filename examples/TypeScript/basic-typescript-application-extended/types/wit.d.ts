/// <reference path="./interfaces/wasi-cli-environment.d.ts" />
/// <reference path="./interfaces/wasi-cli-stderr.d.ts" />
/// <reference path="./interfaces/wasi-cli-stdin.d.ts" />
/// <reference path="./interfaces/wasi-cli-stdout.d.ts" />
/// <reference path="./interfaces/wasi-clocks-monotonic-clock.d.ts" />
/// <reference path="./interfaces/wasi-clocks-wall-clock.d.ts" />
/// <reference path="./interfaces/wasi-http-incoming-handler.d.ts" />
/// <reference path="./interfaces/wasi-http-outgoing-handler.d.ts" />
/// <reference path="./interfaces/wasi-http-types.d.ts" />
/// <reference path="./interfaces/wasi-io-error.d.ts" />
/// <reference path="./interfaces/wasi-io-poll.d.ts" />
/// <reference path="./interfaces/wasi-io-streams.d.ts" />
/// <reference path="./interfaces/wasi-random-random.d.ts" />
declare module 'nor2:heim/nor2-world' {
  export type * as WasiCliEnvironment021 from 'wasi:cli/environment@0.2.1'; // import wasi:cli/environment@0.2.1
  export type * as WasiCliStderr021 from 'wasi:cli/stderr@0.2.1'; // import wasi:cli/stderr@0.2.1
  export type * as WasiCliStdin021 from 'wasi:cli/stdin@0.2.1'; // import wasi:cli/stdin@0.2.1
  export type * as WasiCliStdout021 from 'wasi:cli/stdout@0.2.1'; // import wasi:cli/stdout@0.2.1
  export type * as WasiClocksMonotonicClock021 from 'wasi:clocks/monotonic-clock@0.2.1'; // import wasi:clocks/monotonic-clock@0.2.1
  export type * as WasiClocksWallClock021 from 'wasi:clocks/wall-clock@0.2.1'; // import wasi:clocks/wall-clock@0.2.1
  export type * as WasiHttpOutgoingHandler021 from 'wasi:http/outgoing-handler@0.2.1'; // import wasi:http/outgoing-handler@0.2.1
  export type * as WasiHttpTypes021 from 'wasi:http/types@0.2.1'; // import wasi:http/types@0.2.1
  export type * as WasiIoError021 from 'wasi:io/error@0.2.1'; // import wasi:io/error@0.2.1
  export type * as WasiIoPoll021 from 'wasi:io/poll@0.2.1'; // import wasi:io/poll@0.2.1
  export type * as WasiIoStreams021 from 'wasi:io/streams@0.2.1'; // import wasi:io/streams@0.2.1
  export type * as WasiRandomRandom021 from 'wasi:random/random@0.2.1'; // import wasi:random/random@0.2.1
  export * as incomingHandler from 'wasi:http/incoming-handler@0.2.1'; // export wasi:http/incoming-handler@0.2.1
}
