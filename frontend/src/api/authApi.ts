import api from "./axios";
import type { LoginRequest, AuthResponse } from "@/types";

export const authApi = {
  login: (data: LoginRequest) => api.post<AuthResponse>("/auth/login", data).then((r) => r.data),
  refreshToken: (refreshToken: string) =>
    api.post<AuthResponse>("/auth/refresh", { refreshToken }).then((r) => r.data),
};
