from sqlalchemy.orm import Session

from app.core.security import create_access_token
from app.repositories.user_repository import UserRepository


class AuthService:
    def __init__(self, user_repository: UserRepository):
        self._user_repository = user_repository

    def handle_sso_login(self, db: Session, *, provider: str, subject: str, email: str | None) -> str:
        self._user_repository.upsert(db, provider=provider, user_id=subject, email=email)
        db.commit()
        return create_access_token(subject=subject, provider=provider, email=email)
