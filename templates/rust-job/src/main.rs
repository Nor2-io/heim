use clap::{Parser, Subcommand};
use std::{env, error::Error};

#[derive(Parser, Debug)]
#[command(name = "Clap args", about = "A Heim job demo, for arguments")]
struct Cli {
    #[command(subcommand)]
    command: Commands,
}

#[derive(Subcommand, Debug)]
enum Commands {
    DummyJson {
        #[arg(short, long, default_value = "recipe")]
        path: String,
    },
}

const DUMMY_JSON_BASE_URL: &str = "https://dummyjson.com";

fn main() {
    if let Err(err) = runtime::run() {
        eprintln!("{err}");
        std::process::exit(1);
    }
}

#[cfg(target_os = "wasi")]
mod runtime {
    use std::error::Error;
    #[wstd::main]
    pub async fn run() -> Result<(), Box<dyn Error>> {
        super::async_main().await
    }
}

#[cfg(not(target_os = "wasi"))]
mod runtime {
    use std::error::Error;
    #[tokio::main]
    pub async fn run() -> Result<(), Box<dyn Error>> {
        super::async_main().await
    }
}

async fn async_main() -> Result<(), Box<dyn Error>> {
    let cli = Cli::try_parse_from(env::args())?;

    match cli.command {
        Commands::DummyJson { path } => {
            let url_with_path = format!("{DUMMY_JSON_BASE_URL}/{}", path);
            let contents = http_get_request(&url_with_path).await?;
            println!("We performed a GET operation against {url_with_path}");
            println!("Response: {contents}");
        }
    }

    Ok(())
}

pub async fn http_get_request(url: &str) -> Result<String, Box<dyn Error>> {
    // Wasi
    #[cfg(target_os = "wasi")]
    {
        use wstd::http::{Body, Client, Request};
        let request = Request::get(&url_with_path).body(Body::empty())?;
        let response = Client::new().send(request).await?;
        let mut body = response.into_body();
        let contents = body.str_contents().await?;
        Ok(contents.to_string())
    }
    // Not Wasi
    #[cfg(not(target_os = "wasi"))]
    {
        use reqwest::Client;
        let client = Client::builder().build()?;
        let response = client.get(url).send().await?;
        let text = response.text().await?;
        Ok(text)
    }
}
