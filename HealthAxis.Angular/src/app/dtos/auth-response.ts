export interface AuthResponse {

  message: string;

  data: {
    token: string;
  };

  expiresIn: number;
}