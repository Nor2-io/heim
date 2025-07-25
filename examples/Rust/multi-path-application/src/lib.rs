use serde::{Deserialize, Serialize};
use std::{collections::HashMap, time::Duration};
use waki::{handler, header::ACCEPT, header::CONTENT_TYPE, ErrorCode, Method, Request, Response};

pub const GET_QUERY_PATH: &str = "/multi-path-app/v1/get";
pub const POST_BODY_PATH: &str = "/multi-path-app/v1/post";

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct SearchQuery {
    search_text: String,
    result_limit: i32,
}

#[handler]
fn hello(req: Request) -> Result<Response, ErrorCode> {
    let path = &req.path().to_string();
    let query = &req.query();
    let method = &req.method();

    //Here we have implemented a really simple router based on method and path in the request.
    match (method, path.as_str()) {
        (Method::Post, POST_BODY_PATH) => {
            let body = match &req.body() {
                Ok(body_value) => body_value.clone(),
                Err(err) => {
                    return bad_request(format!("Invalid body request sent, {err:#?}").as_str())
                }
            };
            post_body_handler(&body)
        }
        (Method::Get, GET_QUERY_PATH) => get_body_handler(query),
        _ => not_found(),
    }
}

pub fn not_found() -> Result<Response, ErrorCode> {
    Response::builder()
        .status_code(404)
        .body("NOT FOUND!".to_string())
        .build()
}

/// Bad request response
pub fn bad_request(msg: &str) -> Result<Response, ErrorCode> {
    Response::builder()
        .status_code(400)
        .body(msg.to_string())
        .build()
}

pub fn get_body_handler(query: &HashMap<String, String>) -> Result<Response, ErrorCode> {
    let _search: String = match query.get("search") {
        Some(search) => search.to_string(),
        None => return bad_request("We are missing the required query param called Search"),
    };

    let client = waki::Client::new();
    let result = client
        .get("https://dummyjson.com/recipes/1")
        .headers([("Content-Type", "application/json"), ("Accept", "*")])
        .connect_timeout(Duration::from_secs(1))
        .send()
        .and_then(|res| res.body().map_err(|e| e.into()))
        .and_then(|body| String::from_utf8(body).map_err(|e| e.into()));
    let body = match result {
        Ok(text_body) => text_body,
        Err(err) => format!("We failed to retrieve the response: {err}"),
    };

    Response::builder()
        .status_code(200)
        .headers([(CONTENT_TYPE, "application/json")])
        .body(body)
        .build()
}

/// Post body response
pub fn post_body_handler(body: &Vec<u8>) -> Result<Response, ErrorCode> {
    match serde_json::from_slice::<SearchQuery>(&body) {
        Ok(value) => Response::builder()
            .status_code(200)
            .body(format!(
                "We received the following search query: {value:#?}"
            ))
            .build(),
        Err(err) => bad_request(&format!("Failed deserialize the json body: {err}")),
    }
}
