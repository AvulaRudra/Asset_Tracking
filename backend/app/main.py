from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from starlette.middleware.sessions import SessionMiddleware

from app.core.settings import settings
from app.db.base import Base
from app.db.session import engine
from app.api.routes import auth as auth_routes
from app.api.routes import local_auth as local_auth_routes
from app.api.routes import users as users_routes
from app import models  # noqa: F401
from sqlalchemy.exc import OperationalError

app = FastAPI(title="Asset Tracker API")

app.add_middleware(
    CORSMiddleware,
    allow_origins=[settings.frontend_base_url],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.add_middleware(SessionMiddleware, secret_key=settings.session_secret)

app.include_router(auth_routes.router)
app.include_router(local_auth_routes.router)
app.include_router(users_routes.router)


@app.on_event("startup")
def on_startup():
    try:
        Base.metadata.create_all(bind=engine)
    except OperationalError:
        pass


@app.get("/health")
def health():
    return {"status": "ok"}
