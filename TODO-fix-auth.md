# Fix Swagger Policy Error & Enable Auth

## Current Issues:
- No AddAuthorization → 'Viewer' policy missing
- Missing Asset services/repos
- Middleware order: HttpsRedir before Routing
- No Swagger JWT config

## Steps:
- [ ] Edit Program.cs with fixes
- [ ] dotnet build
- [ ] Test Swagger

