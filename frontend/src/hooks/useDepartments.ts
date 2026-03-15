import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentApi } from "@/api/departmentApi";
import type { CreateDepartmentRequest } from "@/types";
import { toast } from "sonner";

export function useDepartments() {
  return useQuery({ queryKey: ["departments"], queryFn: departmentApi.getAll });
}

export function useDepartment(id: string) {
  return useQuery({
    queryKey: ["departments", id],
    queryFn: () => departmentApi.getById(id),
    enabled: !!id,
  });
}

export function useCreateDepartment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateDepartmentRequest) => departmentApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["departments"] });
      toast.success("Department created successfully");
    },
    onError: () => toast.error("Failed to create department"),
  });
}

export function useUpdateDepartment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: CreateDepartmentRequest }) => departmentApi.update(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["departments"] });
      toast.success("Department updated successfully");
    },
    onError: () => toast.error("Failed to update department"),
  });
}

export function useDeleteDepartment() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => departmentApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["departments"] });
      toast.success("Department deleted successfully");
    },
    onError: () => toast.error("Failed to delete department"),
  });
}
