from app.core.settings import settings

APP_ENV = settings.app_env
APP_BASE_URL = settings.app_base_url
FRONTEND_BASE_URL = settings.frontend_base_url

SESSION_SECRET = settings.session_secret

JWT_SECRET = settings.jwt_secret
JWT_ALGORITHM = settings.jwt_algorithm
JWT_EXPIRES_MINUTES = settings.jwt_expires_minutes

GOOGLE_CLIENT_ID = settings.google_client_id
GOOGLE_CLIENT_SECRET = settings.google_client_secret

AZURE_TENANT_ID = settings.azure_tenant_id
AZURE_CLIENT_ID = settings.azure_client_id
AZURE_CLIENT_SECRET = settings.azure_client_secret

DATABASE_URL = settings.database_url
