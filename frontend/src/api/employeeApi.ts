import api from "./axios";
import type { Employee, CreateEmployeeRequest, UpdateEmployeeRequest, PaginatedResponse } from "@/types";

export const employeeApi = {
  getAll: (params?: { page?: number; pageSize?: number; search?: string }) =>
    api.get<PaginatedResponse<Employee>>("/employees", { params }).then((r) => r.data),
  getById: (id: string) => api.get<Employee>(`/employees/${id}`).then((r) => r.data),
  create: (data: CreateEmployeeRequest) => api.post<Employee>("/auth/register", data).then((r) => r.data),
  update: (id: string, data: UpdateEmployeeRequest) => api.put<Employee>(`/employees/${id}`, data).then((r) => r.data),
  delete: (id: string) => api.delete(`/employees/${id}`),
};
