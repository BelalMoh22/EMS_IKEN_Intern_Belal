import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { employeeApi } from "@/api/employeeApi";
import type { CreateEmployeeRequest, UpdateEmployeeRequest } from "@/types";
import { toast } from "sonner";

export function useEmployees(params?: { page?: number; pageSize?: number; search?: string }) {
  return useQuery({
    queryKey: ["employees", params],
    queryFn: () => employeeApi.getAll(params),
  });
}

export function useEmployee(id: string) {
  return useQuery({
    queryKey: ["employees", id],
    queryFn: () => employeeApi.getById(id),
    enabled: !!id,
  });
}

export function useCreateEmployee() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateEmployeeRequest) => employeeApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["employees"] });
      toast.success("Employee created successfully");
    },
    onError: () => toast.error("Failed to create employee"),
  });
}

export function useUpdateEmployee() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateEmployeeRequest }) => employeeApi.update(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["employees"] });
      toast.success("Employee updated successfully");
    },
    onError: () => toast.error("Failed to update employee"),
  });
}

export function useDeleteEmployee() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => employeeApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["employees"] });
      toast.success("Employee deleted successfully");
    },
    onError: () => toast.error("Failed to delete employee"),
  });
}
