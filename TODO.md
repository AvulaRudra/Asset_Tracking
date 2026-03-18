# Asset/AssetTracking Repositories & Controllers Plan
## Steps:
1. [x] Create IAssetRepository.cs & AssetRepository.cs
2. [x] Create IAssetTrackingRepository.cs & AssetTrackingRepository.cs (fixed DTO/PK)
3. [x] Create AssetsController.cs (Admin CRUD, Viewer GET)
4. [x] Create AssetTrackingController.cs (Viewer GET, Admin CRUD)
5. [x] Update Program.cs for role policies and DI
6. [ ] dotnet build & test
7. [ ] Commit/push

## Notes:
- Async CRUD.
- Use existing DTOs.
- [Authorize(Policy = "Admin")] for CRUD, "Viewer" for GET.
- Dependency injection via constructor.

