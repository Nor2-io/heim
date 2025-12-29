use axum::{
    http::StatusCode,
    response::IntoResponse,
    routing::{Router, get},
};

#[wstd_axum::http_server]
fn main() -> Router {
    Router::new().route("/myapp", get(handler))
}

async fn handler() -> impl IntoResponse {
    (StatusCode::OK, "Hello, World!").into_response()
}
