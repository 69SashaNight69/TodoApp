export interface RegisterDto {
  email: string;
  password?: string;
}

export interface LoginDto {
  email: string;
  password?: string;
}

export interface AuthResponseDto {
  id: string;
  email: string;
  token: string;
}
