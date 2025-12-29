use axum::{
    Json,
    routing::{Router, get},
};
use serde::{Deserialize, Serialize};

#[derive(Deserialize)]
struct MyRequestBody {
    name: String,
}

#[derive(Serialize)]
struct MyResponseBody {
    error: Option<String>,
    name: Option<String>,
    msg: Option<String>,
}

#[wstd_axum::http_server]
fn main() -> Router {
    Router::new().route("/mypath", get(handler))
}

async fn handler(Json(body): Json<MyRequestBody>) -> Json<MyResponseBody> {
    let message = std::env::var("HELLO_MESSAGE").expect("HELLO_MESSAGE is not set");

    MyResponseBody {
        error: None,
        msg: Some(format!("{message} {}", body.name)),
        name: Some(body.name),
    }
    .into()
}
