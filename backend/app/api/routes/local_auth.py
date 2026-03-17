from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session

from app.api.deps import get_db, get_local_auth_service
from app.schemas.local_auth_dto import AuthResponseDTO, SignInRequestDTO, SignUpRequestDTO
from app.services.local_auth_service import LocalAuthService

router = APIRouter(prefix="/auth/local", tags=["auth"])


@router.post("/signup", response_model=AuthResponseDTO)
def signup(
    body: SignUpRequestDTO,
    db: Session = Depends(get_db),
    service: LocalAuthService = Depends(get_local_auth_service),
):
    try:
        token = service.sign_up(db, username=body.username, email=str(body.email) if body.email else None, password=body.password)
        return AuthResponseDTO(access_token=token)
    except ValueError as exc:
        raise HTTPException(status_code=400, detail=str(exc))


@router.post("/login", response_model=AuthResponseDTO)
def login(
    body: SignInRequestDTO,
    db: Session = Depends(get_db),
    service: LocalAuthService = Depends(get_local_auth_service),
):
    try:
        token = service.sign_in(db, username=body.username, password=body.password)
        return AuthResponseDTO(access_token=token)
    except ValueError as exc:
        raise HTTPException(status_code=400, detail=str(exc))
