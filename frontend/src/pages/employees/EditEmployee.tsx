import { useNavigate, useParams } from "react-router-dom";
import { useForm, FormProvider } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useEmployee, useUpdateEmployee } from "@/hooks/useEmployees";
import { useDepartments } from "@/hooks/useDepartments";
import { usePositions } from "@/hooks/usePositions";
import { FormInput } from "@/components/shared/FormInput";
import { FormSelect } from "@/components/shared/FormSelect";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { LoadingSpinner } from "@/components/shared/LoadingSpinner";
import { ArrowLeft, Loader2 } from "lucide-react";
import { useEffect } from "react";

const schema = z.object({
  firstName: z.string().min(1, "First name is required"),
  lastName: z.string().min(1, "Last name is required"),
  email: z.string().email("Invalid email"),
  phone: z.string().min(1, "Phone is required"),
  departmentId: z.string().min(1, "Department is required"),
  positionId: z.string().min(1, "Position is required"),
  hireDate: z.string().min(1, "Hire date is required"),
});

export default function EditEmployee() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { data: employee, isLoading } = useEmployee(id!);
  const updateMutation = useUpdateEmployee();
  const { data: departments } = useDepartments();
  const { data: positions } = usePositions();

  const methods = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      firstName: "", lastName: "", email: "",
      phone: "", departmentId: "", positionId: "", hireDate: "",
    },
  });

  useEffect(() => {
    if (employee) {
      methods.reset({
        firstName: employee.firstName,
        lastName: employee.lastName,
        email: employee.email,
        phone: employee.phone,
        departmentId: employee.departmentId,
        positionId: employee.positionId,
        hireDate: employee.hireDate?.split("T")[0] ?? "",
      });
    }
  }, [employee, methods]);

  const onSubmit = (values: z.infer<typeof schema>) => {
    updateMutation.mutate({ id: id!, data: values }, { onSuccess: () => navigate("/employees") });
  };

  if (isLoading) return <LoadingSpinner />;

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <Button variant="ghost" size="icon" onClick={() => navigate("/employees")}>
          <ArrowLeft className="h-4 w-4" />
        </Button>
        <div>
          <h1 className="text-2xl font-bold text-foreground">Edit Employee</h1>
          <p className="text-muted-foreground">Update employee information</p>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Employee Details</CardTitle>
        </CardHeader>
        <CardContent>
          <FormProvider {...methods}>
            <form onSubmit={methods.handleSubmit(onSubmit)} className="space-y-4">
              <div className="grid gap-4 sm:grid-cols-2">
                <FormInput name="firstName" label="First Name" />
                <FormInput name="lastName" label="Last Name" />
                <FormInput name="email" label="Email" type="email" />
                <FormInput name="phone" label="Phone" />
                <FormInput name="hireDate" label="Hire Date" type="date" />
                <FormSelect
                  name="departmentId"
                  label="Department"
                  options={departments?.map((d) => ({ label: d.name, value: d.id })) ?? []}
                />
                <FormSelect
                  name="positionId"
                  label="Position"
                  options={positions?.map((p) => ({ label: p.title, value: p.id })) ?? []}
                />
              </div>
              <div className="flex gap-3 pt-4">
                <Button type="submit" disabled={updateMutation.isPending}>
                  {updateMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                  Save Changes
                </Button>
                <Button type="button" variant="outline" onClick={() => navigate("/employees")}>
                  Cancel
                </Button>
              </div>
            </form>
          </FormProvider>
        </CardContent>
      </Card>
    </div>
  );
}
