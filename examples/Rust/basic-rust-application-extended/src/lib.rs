use serde::{Deserialize, Serialize};
use waki::{handler, ErrorCode, Request, Response};

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

#[handler]
fn hello(req: Request) -> Result<Response, ErrorCode> {
    let message = std::env::var("HELLO_MESSAGE").expect("HELLO_MESSAGE is not set");
    let body = match req.json::<MyRequestBody>() {
        Ok(b) => b,
        Err(_) => {
            return Response::builder()
                .status_code(400)
                .json(&MyResponseBody {
                    error: Some("Missing request body".to_string()),
                    name: None,
                    msg: None,
                })
                .build();
        }
    };

    Response::builder()
        .status_code(200)
        .json(&MyResponseBody {
            error: None,
            name: Some(body.name.clone()),
            msg: Some(format!("{} {}", message, body.name)),
        })
        .build()
}
