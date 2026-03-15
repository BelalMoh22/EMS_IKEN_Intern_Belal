import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { positionApi } from "@/api/positionApi";
import type { CreatePositionRequest } from "@/types";
import { toast } from "sonner";

export function usePositions() {
  return useQuery({ queryKey: ["positions"], queryFn: positionApi.getAll });
}

export function usePosition(id: string) {
  return useQuery({
    queryKey: ["positions", id],
    queryFn: () => positionApi.getById(id),
    enabled: !!id,
  });
}

export function useCreatePosition() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreatePositionRequest) => positionApi.create(data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["positions"] });
      toast.success("Position created successfully");
    },
    onError: () => toast.error("Failed to create position"),
  });
}

export function useUpdatePosition() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: CreatePositionRequest }) => positionApi.update(id, data),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["positions"] });
      toast.success("Position updated successfully");
    },
    onError: () => toast.error("Failed to update position"),
  });
}

export function useDeletePosition() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => positionApi.delete(id),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ["positions"] });
      toast.success("Position deleted successfully");
    },
    onError: () => toast.error("Failed to delete position"),
  });
}
