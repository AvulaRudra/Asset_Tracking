# TODO: Implement Admin-Only Role Updates

## Steps:
- [x] 1. Update DTOs: Add Role to UserDto.cs, create UpdateUserRoleDto.cs
- [x] 2. Update Repositories: IUserRepository.cs, UserRepository.cs (add GetByProviderIdOrEmailAsync, UpdateRoleAsync)
- [x] 3. Handle LocalUsers: Update ILocalUserRepository.cs, LocalUserRepository.cs similarly
- [x] 4. Update Controllers: Add PATCH endpoint in UsersController.cs
- [ ] 5. Test endpoints
- [ ] 6. Run migrations if needed
- [ ] 7. Frontend admin UI (optional)
- [ ] 8. Complete

Completed: DTOs, Repos (SSO & Local), UsersController with PATCH /users/{identifier}/role (Admin-only)
[ ] Ran migrations

