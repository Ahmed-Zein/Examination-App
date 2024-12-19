import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {map, Observable} from 'rxjs';
import {JsonResponse} from '../models/jsonResponse';
import {AuthResult, LoginModel, RegisterModel} from '../models/auth.model';
import {JwtHelperService} from '@auth0/angular-jwt';
import {Router} from '@angular/router';

@Injectable({providedIn: 'root'})
export class AuthService {
  private tokenSecretKey = 'Bearer-Token';

  constructor(private http: HttpClient, private jwtHelperService: JwtHelperService, private router: Router) {
  }

  public UserRole(): string {
    const token = localStorage.getItem(this.tokenSecretKey);
    if (token === null) {
      return '';
    }
    const decodedToken = this.jwtHelperService.decodeToken(token) as { role: string };
    return decodedToken.role;
  }

  public GetUserId(): string {
    const token: string = this.GetAuthToken();

    const decodedToken = this.jwtHelperService.decodeToken(token) as { sub: string };
    return decodedToken.sub;
  }

  public GetAuthToken(): string {
    return localStorage.getItem(this.tokenSecretKey) as string ?? '';
  }

  public IsAuthenticated() {
    return localStorage.getItem(this.tokenSecretKey) != null;
  }

  public Login(loginModel: LoginModel): Observable<boolean> {
    return this.http
      .post<JsonResponse<AuthResult>>('/auth/login', loginModel)
      .pipe(
        map((res) => {
          if (!res.success || !res.data) return false;
          const {data} = res;
          this.storeToken(data.token);
          return res.success;
        })
      );
  }

  public Register(registerModel: RegisterModel): Observable<boolean> {
    return this.http
      .post<JsonResponse<AuthResult>>('/auth/signup', registerModel)
      .pipe(
        map((res) => {
          if (!res.success || !res.data) return false;
          const {data} = res;
          this.storeToken(data.token);
          return res.success;
        })
      );
  }

  public async Logout() {
    localStorage.removeItem(this.tokenSecretKey);
    return await this.router.navigate(['/auth/login']);
  }

  private storeToken(token: string) {
    localStorage.setItem(this.tokenSecretKey, token);
  }
}
