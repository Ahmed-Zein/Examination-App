import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot,} from '@angular/router';
import {Observable} from 'rxjs';
import {AuthService} from '../services/auth.service';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private jwtService: JwtHelperService
  ) {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    _: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const token = this.authService.GetAuthToken();

    if (!this.isAuthenticatedWithValidToken(token)) {
      return this.redirectHandler();
    }
    const authorizedRoles: [] = route.data['roles'];
    const decodedToken = this.jwtService.decodeToken(token);
    if (!this.isTokenHasAllowedRole(authorizedRoles, decodedToken)) {
      return this.redirectHandler();
    }
    return true;
  }

  private isAuthenticatedWithValidToken(token: string) {
    return (
      this.authService.IsAuthenticated() &&
      !this.jwtService.isTokenExpired(token)
    );
  }

  private isTokenHasAllowedRole(authorizedRoles: [], decodedToken: { role: string }) {
    return authorizedRoles.some((role) => decodedToken.role === role);
  }

  private redirectHandler(): boolean {
    this.router.navigate(['/auth/login']).catch((err) => console.log(err));
    return false;
  }
}
