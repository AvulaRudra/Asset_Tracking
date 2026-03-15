from sqlalchemy.orm import Session

from app.models import User


class UserRepository:
    def get_by_provider_id(self, db: Session, *, provider: str, user_id: str) -> User | None:
        return db.get(User, {"id": user_id, "provider": provider})

    def upsert(self, db: Session, *, provider: str, user_id: str, email: str | None) -> User:
        user = self.get_by_provider_id(db, provider=provider, user_id=user_id)
        if user is None:
            user = User(id=user_id, provider=provider, email=email)
            db.add(user)
        else:
            user.email = email
        return user
