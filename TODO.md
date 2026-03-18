# Login Fix Progress

## Completed
- [x] Update API base URL to http://localhost:5000 (environment*.ts)
- [x] Fix AuthResponseDto casing to PascalCase (AccessToken)
- [x] Update login.component.ts to use res.AccessToken

## Next
- Restart Angular dev server: cd frontend && ng serve
- Test login at http://localhost:4200/login
- Check browser Network tab for POST /auth/local/login -> 200 OK
- Verify localStorage 'access_token' set
- If error, provide new console/network info.

Backend endpoints work (Swagger confirmed).

Task complete once login succeeds from frontend.

