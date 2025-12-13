# Heim Rust-job temlate.

This example show the usage of arguments together with the clap crate.

If the command `dummy-json` we will fetch the `https://dummyjson.com/recipe` endpoint.
And if we also provide the `--path test` for example we will fetch the `https://dummyjson.com/test` endpoint.

We are showcasing how you can make your code works both in a regular CLI and in Heim here
depending of target.

Run locally with cargo directly and not run it as an wasm module.
```
cargo run -- dummy-json
```

Deploy it to Heim locally and trigger the execution in Heim-portal
```
heim deploy
```


**Happy coding**
