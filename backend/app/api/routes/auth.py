from fastapi import APIRouter, Depends, HTTPException, Request
from fastapi.responses import RedirectResponse
from authlib.integrations.starlette_client import OAuth, OAuthError
from sqlalchemy.orm import Session

from app.api.deps import get_auth_service, get_db
from app.core.settings import settings
from app.services.auth_service import AuthService

router = APIRouter(prefix="/auth", tags=["auth"])

oauth = OAuth()

if settings.google_client_id and settings.google_client_secret:
    oauth.register(
        name="google",
        server_metadata_url="https://accounts.google.com/.well-known/openid-configuration",
        client_id=settings.google_client_id,
        client_secret=settings.google_client_secret,
        client_kwargs={"scope": "openid email profile"},
    )

if settings.azure_client_id and settings.azure_client_secret:
    oauth.register(
        name="azure",
        server_metadata_url=(
            f"https://login.microsoftonline.com/{settings.azure_tenant_id}/v2.0/.well-known/openid-configuration"
        ),
        client_id=settings.azure_client_id,
        client_secret=settings.azure_client_secret,
        client_kwargs={"scope": "openid email profile"},
    )


@router.get("/google/login")
async def google_login(request: Request):
    if not hasattr(oauth, "google"):
        raise HTTPException(status_code=500, detail="Google SSO is not configured")

    redirect_uri = f"{settings.app_base_url}/auth/google/callback"
    return await oauth.google.authorize_redirect(request, redirect_uri)


@router.get("/google/callback")
async def google_callback(
    request: Request,
    db: Session = Depends(get_db),
    auth_service: AuthService = Depends(get_auth_service),
):
    try:
        token = await oauth.google.authorize_access_token(request)
        userinfo = token.get("userinfo")
        if not userinfo:
            userinfo = await oauth.google.userinfo(token=token)

        subject = str(userinfo.get("sub") or userinfo.get("id") or "")
        email = userinfo.get("email")
        if not subject:
            raise HTTPException(status_code=400, detail="Missing subject in Google profile")

        jwt_token = auth_service.handle_sso_login(db, provider="google", subject=subject, email=email)
        redirect_to = f"{settings.frontend_base_url}/login?token={jwt_token}"
        return RedirectResponse(url=redirect_to)
    except OAuthError as e:
        raise HTTPException(status_code=400, detail=str(e))


@router.get("/azure/login")
async def azure_login(request: Request):
    if not hasattr(oauth, "azure"):
        raise HTTPException(status_code=500, detail="Azure AD SSO is not configured")

    redirect_uri = f"{settings.app_base_url}/auth/azure/callback"
    return await oauth.azure.authorize_redirect(request, redirect_uri)


@router.get("/azure/callback")
async def azure_callback(
    request: Request,
    db: Session = Depends(get_db),
    auth_service: AuthService = Depends(get_auth_service),
):
    try:
        token = await oauth.azure.authorize_access_token(request)
        userinfo = token.get("userinfo")
        if not userinfo:
            userinfo = await oauth.azure.userinfo(token=token)

        subject = str(userinfo.get("sub") or userinfo.get("oid") or "")
        email = userinfo.get("email") or userinfo.get("preferred_username")
        if not subject:
            raise HTTPException(status_code=400, detail="Missing subject in Azure profile")

        jwt_token = auth_service.handle_sso_login(db, provider="azure", subject=subject, email=email)
        redirect_to = f"{settings.frontend_base_url}/login?token={jwt_token}"
        return RedirectResponse(url=redirect_to)
    except OAuthError as e:
        raise HTTPException(status_code=400, detail=str(e))
