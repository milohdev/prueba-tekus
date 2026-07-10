export interface Service {
  id: string;
  name: string;
  costPerHour: number;
  providerId: string;
  isActive: boolean;
}

export interface CreateServiceRequest {
  name: string;
  costPerHour: number;
}

export interface UpdateServiceRequest {
  id: string;
  name: string;
  costPerHour: number;
}