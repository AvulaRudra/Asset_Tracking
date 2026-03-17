from pydantic import BaseModel


class UserDTO(BaseModel):
    id: str
    provider: str
    email: str | None = None


class TokenDTO(BaseModel):
    access_token: str
    token_type: str = "bearer"
