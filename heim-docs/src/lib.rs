use http::Response;
use include_dir::{include_dir, Dir};
use wstd::http::{
    body::IncomingBody,
    server::{Finished, Responder},
    IntoBody, Request, StatusCode,
};
static DIST_DIR: Dir = include_dir!("./docs/dist");
const HEIM_NOT_FOUND_TEMPLATE: &str = r#"
<html lang="en">
	<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Heim.dev 404 Not Found</title>
    <meta name="description" content="Heim.dev not found">
		<link rel="icon" href="https://cloud.heim.dev/heim/portal/favicon.ico" sizes "any">
		<link rel="icon" type="image/svg+xml" href="https://cloud.heim.dev/heim/portal/favicon.svg">
	</head>
	<body style="--tw-bg-opacity: 1; background-color: rgb(255 255 255);max-height: -moz-fit-content;max-height: fit-content;min-height: -moz-fit-content;min-height: fit-content;padding: 0;margin: 0 auto;line-height: inherit;box-sizing: border-box;border-width: 0;border-style: solid;box-sizing: border-box;border-width: 0;border-style: solid;border-color: #e5e7eb;">
		<div style="flex-direction: column;display: flex;height: 100%;width: 100%;">
			<div style="display: flex;height: 100%;width: 100%;">
				<main style="flex-direction: column;align-items: center;padding-right: 5rem;padding-left: 5rem;padding-bottom: 4rem;background-blend-mode: darken;padding-top: 2.5rem;width: 100%;height: 100%; display: flex; position:relative;">
					<img style="-o-object-fit: cover;
    object-fit: cover;width: 100%;height:100%;top:0;right:0;bottom:0;left:0;position:absolute;max-width:100%;display:block;vertical-align:middle;" src="https://cdn.builder.io/api/v1/image/assets/78c46f537fc84c2caeea7ef986634bfc/688f1a7414f38de566fcce1cf14776655686b79616406d40397934f605c357b9?apiKey=78c46f537fc84c2caeea7ef986634bfc&amp;" alt="">
					<section style="width: 498px;display: flex;position: relative;flex-direction: column;align-items: center;">
						<svg xmlns="http://www.w3.org/2000/svg" aria-label="Heim Logo" viewBox="0 0 77 24" style="--tw-text-opacity: 1; color: rgb(127 86 217 / 1);fill: 100%; width: 8rem;height:4.0rem;display:block;vertical-align:middle;">
							<title>Heim Logo</title>
							<path d="M56.3094 13.7284V23.6379H51.0547V6.2881H56.3094V8.03295C57.2618 6.91361 58.8382 5.92596 61.0386 5.92596C63.1076 5.92596 64.8154 6.81485 65.8007 8.29633C66.7859 7.20991 68.4937 5.92596 71.1539 5.92596C74.6679 5.92596 76.9997 8.39509 76.9997 12.1811V23.6379H71.745V13.6955C71.745 11.9506 71.0554 10.6667 69.3804 10.6667C68.0011 10.6667 66.6546 11.6873 66.6546 13.7284V23.6379H61.3999V13.6955C61.3999 11.9506 60.743 10.6667 59.0352 10.6667C57.6559 10.6667 56.3094 11.6873 56.3094 13.7284Z" fill="currentColor"></path>
							<path d="M42.3306 4.51033V3.8147e-05L47.6838 1.31803V4.51033H42.3306ZM47.6181 6.28811V23.6379H42.3634V6.28811H47.6181Z" fill="currentColor"></path>
							<path d="M31.9626 24C26.7408 24 23.1938 20.3457 23.1938 14.9465C23.1938 9.81072 26.8721 5.92594 31.8641 5.92594C37.1188 5.92594 39.976 9.81072 39.976 14.7161V16.0988H28.2515C28.6127 18.4692 30.0249 19.8189 32.0283 19.8189C33.6704 19.8189 34.9512 19.0288 35.4438 17.5144L39.8446 18.9301C38.5638 22.2552 35.411 24 31.9626 24ZM31.7984 10.0082C30.1892 10.0082 28.9412 10.963 28.4157 12.8724H34.787C34.7542 11.3251 33.7361 10.0082 31.7984 10.0082Z" fill="currentColor"></path>
							<path d="M15.0744 0H20.559V23.6379H15.0744V13.893H5.48458V23.6379H0V0H5.48458V9.21811H15.0744V0Z" fill="currentColor"></path>
						</svg>
						<div data-testid="form-wrapper" 
						style="margin-top: 3rem;display: flex;--tw-shadow: 0 1px 2px 0 rgb(0 0 0 / .05);height: 100%;min-height: fit-content;width: 100%;--tw-shadow-colored: 0 1px 2px 0;min-width: fit-content;flex-direction: column;align-items: center;justify-content: center;box-shadow: 0 0 #0000, 0 0 #0000, 0 0 #0000;align-self: stretch;border-radius: .75rem;padding-top: 1.25rem;padding-bottom: 1.25rem;padding-left: 1.5rem;padding-right: 1.5rem;--tw-bg-opacity: 1;background-color: rgb(255 255 255)">
							<div>
								<form data-testid="form" style="width: 100%; height: 100%;box-sizing: border-box; border-width: 0; border-style: solid;border-color: #e5e7eb;;">
									<h1 style="line-height: 2;--tw-text-opacity: 1;color: rgb(19 7 42);font-weight: 600;align-self: flex-start;margin: 0;font-size: 120%;margin-block-start: 0.83em;margin-block-end: 0.83em;display: block;margin-inline-start: 0px;margin-inline-end: 0px;">404 Not found</h1>
									<div style="margin-top: 0.5rem;display: flex;flex-direction: column;"><p>Content is not found on Heim</p></div>
									<div style="margin-top: 2rem;display: flex;flex-direction: column;">
										<label for="host" style="padding-bottom: .5rem;--tw-text-opacity: 1;color: rgb(19 7 42);line-height: 1;font-weight: 600;font-size: .8rem;">Host</label>
										<p id="host" style="padding-bottom: .5rem;--tw-text-opacity: 1;color: rgb(19 7 42);line-height: 1;font-weight: 400;font-size: .8rem;">
											{{HOST}}
										</p>
									</div>
									<div style="margin-top: 2rem;display: flex;flex-direction: column;">
										<label for="path" style="padding-bottom: .5rem;--tw-text-opacity: 1;color: rgb(19 7 42);line-height: 1;font-weight: 600;font-size: .8rem;">Path</label>
										<p id="host" style="padding-bottom: .5rem;--tw-text-opacity: 1;color: rgb(19 7 42);line-height: 1;font-weight: 400;font-size: .8rem;">
											{{PATH}}
										</p>
									</div>
								</form>
							</div>
						</div>
					</section>
				</main>
			</div>
		</div>
	</body>
</html>
"#;

#[wstd::http_server]
async fn main(request: Request<IncomingBody>, responder: Responder) -> Finished {
    let start_path = request.uri().path();
    let mut path = request.uri().path().replace("/heim/docs/", "");
    if path.is_empty() {
        path = "index.html".to_string();
    }
    let host = request.uri().host().unwrap_or_default();

    let file = match DIST_DIR.get_file(&path) {
        Some(file) => file,
        None => {
            let path_temp = &format!("{}index.html", &path);
            match DIST_DIR.get_file(path_temp) {
                Some(index_file) => {
                    path = path_temp.to_owned();
                    index_file
                }
                None => {
                    let data = Response::builder()
                        .header(http::header::CONTENT_TYPE, "text/html")
                        .header(http::header::ACCESS_CONTROL_ALLOW_ORIGIN, "*")
                        .status(StatusCode::NOT_FOUND)
                        .body(
                            HEIM_NOT_FOUND_TEMPLATE
                                .replace("{{HOST}}", host)
                                .replace("{{PATH}}", start_path)
                                .into_body(),
                        )
                        .unwrap();

                    return responder.respond(data).await;
                }
            }
        }
    };

    let data = match file.contents_utf8() {
        Some(file_data) => file_data.as_bytes().to_vec(),
        None => file.contents().to_vec(),
    };
    let guess = mime_guess::from_path(&path);

    let response = match guess.first() {
        Some(guess) => Response::builder()
            .status(StatusCode::OK)
            .header(http::header::CONTENT_TYPE, guess.essence_str())
            .header(http::header::ACCESS_CONTROL_ALLOW_ORIGIN, "*")
            .body(data.into_body())
            .unwrap(),
        None => Response::builder()
            .status(StatusCode::OK)
            .header(http::header::ACCESS_CONTROL_ALLOW_ORIGIN, "*")
            .body(data.into_body())
            .unwrap(),
    };

    responder.respond(response).await
}
