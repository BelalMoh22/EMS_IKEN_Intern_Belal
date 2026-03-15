import api from "./axios";
import type { Position, CreatePositionRequest } from "@/types";

export const positionApi = {
  getAll: () => api.get<Position[]>("/positions").then((r) => r.data),
  getById: (id: string) => api.get<Position>(`/positions/${id}`).then((r) => r.data),
  create: (data: CreatePositionRequest) => api.post<Position>("/positions", data).then((r) => r.data),
  update: (id: string, data: CreatePositionRequest) => api.put<Position>(`/positions/${id}`, data).then((r) => r.data),
  delete: (id: string) => api.delete(`/positions/${id}`),
};
