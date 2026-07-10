export interface Provider {
  id: string;
  name: string;
  nit: string;
  pageUrl: string;
  email: string;
  role: string;
  isActive: boolean;
}

export interface CreateProviderRequest {
  name: string;
  nit: string;
  pageUrl: string;
  email: string;
  password: string;
}

export interface UpdateProviderRequest {
  id: string;
  name: string;
  pageUrl: string;
}

export interface ProviderLoginRequest {
  email: string;
  password: string;
}

export interface ProviderAuthResponse {
  providerId: string;
  name: string;
  nit: string;
  email: string;
  role: string;
  token: string;
  expiresAt: string;
}