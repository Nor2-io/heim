use waki::{handler, ErrorCode, Request, Response};

#[handler]
fn hello(req: Request) -> Result<Response, ErrorCode> {
    Response::builder()
        .status_code(200)
        .body("Hello World")
        .build()
}