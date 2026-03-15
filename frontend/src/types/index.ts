export type Role = "Admin" | "HR" | "Manager";

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  user: User;
}

export interface Employee {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  departmentId: string;
  departmentName?: string;
  positionId: string;
  positionName?: string;
  hireDate: string;
  status: string;
}

export interface CreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone: string;
  departmentId: string;
  positionId: string;
  hireDate: string;
  role: Role;
}

export interface UpdateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  departmentId: string;
  positionId: string;
  hireDate: string;
}

export interface Department {
  id: string;
  name: string;
  description: string;
}

export interface CreateDepartmentRequest {
  name: string;
  description: string;
}

export interface Position {
  id: string;
  title: string;
  departmentId: string;
  departmentName?: string;
  description: string;
}

export interface CreatePositionRequest {
  title: string;
  departmentId: string;
  description: string;
}

export interface PaginatedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
