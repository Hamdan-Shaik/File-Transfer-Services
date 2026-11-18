import argparse
import io
import pathlib
import urllib.parse
from pathlib import PurePosixPath
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer


OUT_DIR = pathlib.Path(__file__).resolve().parent.parent / "out"
FXFER_SPA_FALLBACK = OUT_DIR / "fxfer" / "[[...slug]]" / "index.html"


class SpaExportHandler(SimpleHTTPRequestHandler):
    def translate_path(self, path: str) -> str:
        url_path = urllib.parse.urlparse(path).path
        posix_path = PurePosixPath(urllib.parse.unquote(url_path))
        parts = [p for p in posix_path.parts if p not in ("/", ".", "..")]
        full_path = OUT_DIR

        for part in parts:
            full_path = full_path / part

        return str(full_path)

    def send_head(self):
        request_path = urllib.parse.urlparse(self.path).path
        resolved = pathlib.Path(self.translate_path(self.path))

        if resolved.is_dir():
            index_file = resolved / "index.html"
            if index_file.exists():
                resolved = index_file

        if resolved.is_file():
            return self._send_file(resolved)

        if request_path.startswith("/fxfer") and FXFER_SPA_FALLBACK.is_file():
            return self._send_file(FXFER_SPA_FALLBACK)

        self.send_error(404, "File not found")
        return None

    def _send_file(self, path: pathlib.Path):
        data = path.read_bytes()
        ctype = self.guess_type(str(path))

        self.send_response(200)
        self.send_header("Content-type", ctype)
        self.send_header("Content-Length", str(len(data)))
        self.end_headers()

        return io.BytesIO(data)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--port", type=int, default=3000)
    args = parser.parse_args()

    server = ThreadingHTTPServer(("localhost", args.port), SpaExportHandler)
    server.serve_forever()


if __name__ == "__main__":
    main()

