use std::io::Write as _;

wasi::cli::command::export!(HelloWorld);

struct HelloWorld;

impl wasi::exports::cli::run::Guest for HelloWorld {
    fn run() -> Result<(), ()> {
        let mut stdout = wasi::cli::stdout::get_stdout();
        stdout.write_all(b"Hello World!").unwrap();
        stdout.flush().unwrap();
        Ok(())
    }
}
