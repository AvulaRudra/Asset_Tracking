from pydantic import BaseModel, EmailStr, Field

from .user_dto import TokenDTO


class SignUpRequestDTO(BaseModel):
    username: str = Field(min_length=3, max_length=50)
    email: EmailStr | None = None
    password: str = Field(min_length=8, max_length=128)


class SignInRequestDTO(BaseModel):
    username: str
    password: str


class AuthResponseDTO(TokenDTO):
    pass
