# Fix Unauthorized Asset APIs

## Steps:
- [ ] 1. Program.cs: Viewer policy accepts Admin too
- [ ] 2. JwtService.cs: Use user.Role not hardcoded Admin
- [ ] 3. LocalAuthService.cs: Pass user.Role to JwtService
- [ ] 4. Build & migrate DB
- [ ] 5. Create admin/viewer users
- [ ] 6. Test Swagger

**Progress: Starting edits**

