export class AuthResult {
  token = '';
  message = '';
}

export interface LoginModel {
  email: string;
  password: string;
}

export interface RegisterModel {
  email: string;
  password: string;
  lastName: string;
  firstName: string;
}
