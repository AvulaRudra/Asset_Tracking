import secrets

from sqlalchemy.orm import Session

from app.core.passwords import hash_password, verify_password
from app.core.security import create_access_token
from app.models import LocalUser
from app.repositories.local_user_repository import LocalUserRepository


class LocalAuthService:
    def __init__(self, repo: LocalUserRepository):
        self._repo = repo

    def sign_up(self, db: Session, *, username: str, email: str | None, password: str) -> str:
        existing = self._repo.get_by_username(db, username=username)
        if existing is not None:
            raise ValueError("Username already exists")
        if email is not None:
            existing_email = self._repo.get_by_email(db, email=email)
            if existing_email is not None:
                raise ValueError("Email already exists")

        user = LocalUser(
            id=secrets.token_urlsafe(16),
            username=username,
            email=email,
            password_hash=hash_password(password),
        )
        self._repo.create(db, user=user)
        db.commit()

        return create_access_token(subject=user.id, provider="local", email=user.email)

    def sign_in(self, db: Session, *, username: str, password: str) -> str:
        user = self._repo.get_by_username(db, username=username)
        if user is None:
            raise ValueError("Invalid username or password")
        if not verify_password(password, user.password_hash):
            raise ValueError("Invalid username or password")

        return create_access_token(subject=user.id, provider="local", email=user.email)
