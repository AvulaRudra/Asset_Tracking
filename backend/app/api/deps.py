from collections.abc import Generator

from fastapi import Depends, HTTPException
from fastapi.security import HTTPAuthorizationCredentials, HTTPBearer
from sqlalchemy.orm import Session

from app.core.security import decode_access_token
from app.db.session import SessionLocal
from app.repositories.local_user_repository import LocalUserRepository
from app.repositories.user_repository import UserRepository
from app.schemas.user_dto import UserDTO
from app.services.auth_service import AuthService
from app.services.local_auth_service import LocalAuthService


def get_db() -> Generator[Session, None, None]:
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()


def get_auth_service() -> AuthService:
    return AuthService(UserRepository())


def get_local_auth_service() -> LocalAuthService:
    return LocalAuthService(LocalUserRepository())


bearer_scheme = HTTPBearer(auto_error=False)


def get_current_token_payload(
    creds: HTTPAuthorizationCredentials | None = Depends(bearer_scheme),
) -> dict:
    if creds is None or not creds.credentials:
        raise HTTPException(status_code=401, detail="Missing bearer token")
    try:
        return decode_access_token(creds.credentials)
    except ValueError:
        raise HTTPException(status_code=401, detail="Invalid bearer token")


def get_current_user(
    payload: dict = Depends(get_current_token_payload),
    db: Session = Depends(get_db),
) -> UserDTO:
    provider = str(payload.get("provider"))
    subject = str(payload.get("sub"))
    email = payload.get("email")

    if not provider or not subject:
        raise HTTPException(status_code=401, detail="Invalid token payload")

    if provider == "local":
        repo = LocalUserRepository()
        user = repo.get_by_id(db, user_id=subject)
        if user is None:
            raise HTTPException(status_code=401, detail="User not found")
        return UserDTO(id=user.id, provider="local", email=user.email)

    repo = UserRepository()
    user = repo.get_by_provider_id(db, provider=provider, user_id=subject)
    if user is None:
        raise HTTPException(status_code=401, detail="User not found")
    return UserDTO(id=user.id, provider=user.provider, email=user.email)
