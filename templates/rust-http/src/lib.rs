use axum::{
    http::StatusCode,
    response::IntoResponse,
    routing::{get, Router},
};

#[wstd_axum::http_server]
fn main() -> Router {
    Router::new().route("/hello", get(handler))
}

async fn handler() -> impl IntoResponse {
    (StatusCode::Ok, "Hello, World!").into_response()
}
