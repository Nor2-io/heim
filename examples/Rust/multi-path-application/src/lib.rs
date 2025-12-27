use axum::{
    Json,
    extract::Query,
    http::{Response, StatusCode, header::CONTENT_TYPE},
    response::IntoResponse,
    routing::{Router, get, post},
};
use serde::{Deserialize, Serialize};
use std::{collections::HashMap, time::Duration};
use wstd::http::Body;

#[wstd_axum::http_server]
fn main() -> Router {
    Router::new()
        .route("/multi-path-app/v1/get", get(get_body_handler))
        .route("/multi-path-app/v1/post", post(post_body_handler))
        .fallback(async || (StatusCode::NOT_FOUND, "NOT FOUND!").into_response())
}

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct SearchQuery {
    search_text: String,
    result_limit: i32,
}

#[axum::debug_handler]
async fn get_body_handler(Query(params): Query<HashMap<String, String>>) -> Response<String> {
    let _search: String = match params.get("search") {
        Some(search) => search.to_string(),
        None => {
            return Response::builder()
                .status(StatusCode::BAD_REQUEST)
                .body("We are missing the required query param called Search".to_string())
                .unwrap();
        }
    };

    let mut client = wstd::http::Client::new();
    client.set_connect_timeout(Duration::from_secs(1));

    let request = wstd::http::Request::get("https://dummyjson.com/recipes/1")
        .header("Content-Type", "application/json")
        .header("Accept", "*")
        .body(Body::empty())
        .unwrap();

    let result = match client.send(request).await {
        Ok(result) => result,
        Err(e) => {
            return Response::builder()
                .status(StatusCode::INTERNAL_SERVER_ERROR)
                .body(format!("Failed to send request with error: {e}"))
                .unwrap();
        }
    };

    let body = match result.into_body().str_contents().await {
        Ok(text_body) => text_body.to_string(),
        Err(e) => {
            return Response::builder()
                .status(StatusCode::INTERNAL_SERVER_ERROR)
                .body(format!("We failed to retrieve the response: {e}"))
                .unwrap();
        }
    };

    Response::builder()
        .status(StatusCode::OK)
        .header(CONTENT_TYPE, "application/json")
        .body(body)
        .unwrap()
}

/// Post body response
async fn post_body_handler(Json(body): Json<SearchQuery>) -> impl IntoResponse {
    (
        StatusCode::OK,
        format!("We received the following search query: {body:#?}"),
    )
        .into_response()
}
