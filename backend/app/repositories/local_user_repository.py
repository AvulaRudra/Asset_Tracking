from sqlalchemy import select
from sqlalchemy.orm import Session

from app.models import LocalUser


class LocalUserRepository:
    def get_by_id(self, db: Session, *, user_id: str) -> LocalUser | None:
        return db.get(LocalUser, user_id)

    def get_by_username(self, db: Session, *, username: str) -> LocalUser | None:
        stmt = select(LocalUser).where(LocalUser.username == username)
        return db.execute(stmt).scalars().first()

    def get_by_email(self, db: Session, *, email: str) -> LocalUser | None:
        stmt = select(LocalUser).where(LocalUser.email == email)
        return db.execute(stmt).scalars().first()

    def create(self, db: Session, *, user: LocalUser) -> LocalUser:
        db.add(user)
        return user
