from fastapi import APIRouter, Depends

from app.api.deps import get_current_user
from app.schemas.user_dto import UserDTO

router = APIRouter(prefix="/users", tags=["users"])


@router.get("/me", response_model=UserDTO)
def me(
    user: UserDTO = Depends(get_current_user),
):
    return user
