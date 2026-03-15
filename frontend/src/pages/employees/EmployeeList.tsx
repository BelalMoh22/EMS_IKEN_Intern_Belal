import { useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useEmployees, useDeleteEmployee } from "@/hooks/useEmployees";
import { useAuthStore } from "@/stores/auth";
import { DataTable, type Column } from "@/components/shared/DataTable";
import { SearchInput } from "@/components/shared/SearchInput";
import { ConfirmDialog } from "@/components/shared/ConfirmDialog";
import { Button } from "@/components/ui/button";
import { Plus, Pencil, Trash2 } from "lucide-react";
import type { Employee } from "@/types";

export default function EmployeeList() {
  const navigate = useNavigate();
  const user = useAuthStore((s) => s.user);
  const canCreate = user?.role === "Admin" || user?.role === "HR";
  const canDelete = user?.role === "Admin";

  const [search, setSearch] = useState("");
  const [page, setPage] = useState(1);
  const [deleteTarget, setDeleteTarget] = useState<string | null>(null);

  const { data, isLoading } = useEmployees({ page, pageSize: 10, search });
  const deleteMutation = useDeleteEmployee();

  const handleDelete = () => {
    if (deleteTarget) {
      deleteMutation.mutate(deleteTarget, { onSuccess: () => setDeleteTarget(null) });
    }
  };

  const columns: Column<Employee>[] = [
    { header: "Name", cell: (row) => `${row.firstName} ${row.lastName}` },
    { header: "Email", accessorKey: "email" },
    { header: "Department", accessorKey: "departmentName" },
    { header: "Position", accessorKey: "positionName" },
    { header: "Status", cell: (row) => (
      <span className={`rounded-full px-2 py-0.5 text-xs font-medium ${
        row.status === "Active" ? "bg-success/10 text-success" : "bg-muted text-muted-foreground"
      }`}>
        {row.status}
      </span>
    )},
    ...(canCreate ? [{
      header: "Actions" as const,
      cell: (row: Employee) => (
        <div className="flex gap-1">
          <Button variant="ghost" size="icon" onClick={() => navigate(`/employees/edit/${row.id}`)}>
            <Pencil className="h-4 w-4" />
          </Button>
          {canDelete && (
            <Button variant="ghost" size="icon" onClick={() => setDeleteTarget(row.id)}>
              <Trash2 className="h-4 w-4 text-destructive" />
            </Button>
          )}
        </div>
      ),
    }] : []),
  ];

  const handleSearch = useCallback((v: string) => { setSearch(v); setPage(1); }, []);

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-foreground">Employees</h1>
          <p className="text-muted-foreground">Manage your team members</p>
        </div>
        {canCreate && (
          <Button onClick={() => navigate("/employees/create")}>
            <Plus className="mr-2 h-4 w-4" /> Add Employee
          </Button>
        )}
      </div>

      <div className="max-w-sm">
        <SearchInput value={search} onChange={handleSearch} placeholder="Search employees..." />
      </div>

      <DataTable
        columns={columns}
        data={data?.data ?? []}
        loading={isLoading}
        page={page}
        totalPages={data ? Math.ceil(data.totalCount / data.pageSize) : 1}
        onPageChange={setPage}
      />

      <ConfirmDialog
        open={!!deleteTarget}
        onOpenChange={(open) => !open && setDeleteTarget(null)}
        title="Delete Employee"
        description="Are you sure you want to delete this employee? This action cannot be undone."
        onConfirm={handleDelete}
        loading={deleteMutation.isPending}
      />
    </div>
  );
}
