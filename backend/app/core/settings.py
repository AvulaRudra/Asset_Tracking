from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", extra="ignore")

    app_env: str = "development"
    app_base_url: str = "http://localhost:8000"
    frontend_base_url: str = "http://localhost:4200"

    session_secret: str = "change-me"

    jwt_secret: str = "change-me"
    jwt_algorithm: str = "HS256"
    jwt_expires_minutes: int = 120

    database_url: str = "postgresql+psycopg://postgres:Rudra%40123@localhost:5432/asset_tracker"

    google_client_id: str | None = None
    google_client_secret: str | None = None

    azure_tenant_id: str = "common"
    azure_client_id: str | None = None
    azure_client_secret: str | None = None


settings = Settings()
