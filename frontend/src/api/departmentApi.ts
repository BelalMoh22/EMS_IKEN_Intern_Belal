import api from "./axios";
import type { Department, CreateDepartmentRequest } from "@/types";

export const departmentApi = {
  getAll: () => api.get<Department[]>("/departments").then((r) => r.data),
  getById: (id: string) => api.get<Department>(`/departments/${id}`).then((r) => r.data),
  create: (data: CreateDepartmentRequest) => api.post<Department>("/departments", data).then((r) => r.data),
  update: (id: string, data: CreateDepartmentRequest) => api.put<Department>(`/departments/${id}`, data).then((r) => r.data),
  delete: (id: string) => api.delete(`/departments/${id}`),
};
