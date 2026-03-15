import { useNavigate } from "react-router-dom";
import { useForm, FormProvider } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useCreateEmployee } from "@/hooks/useEmployees";
import { useDepartments } from "@/hooks/useDepartments";
import { usePositions } from "@/hooks/usePositions";
import { FormInput } from "@/components/shared/FormInput";
import { FormSelect } from "@/components/shared/FormSelect";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ArrowLeft, Loader2 } from "lucide-react";

const schema = z.object({
  firstName: z.string().min(1, "First name is required"),
  lastName: z.string().min(1, "Last name is required"),
  email: z.string().email("Invalid email"),
  password: z.string().min(6, "Password must be at least 6 characters"),
  phone: z.string().min(1, "Phone is required"),
  departmentId: z.string().min(1, "Department is required"),
  positionId: z.string().min(1, "Position is required"),
  hireDate: z.string().min(1, "Hire date is required"),
  role: z.enum(["Admin", "HR", "Manager"]),
});

export default function CreateEmployee() {
  const navigate = useNavigate();
  const createMutation = useCreateEmployee();
  const { data: departments } = useDepartments();
  const { data: positions } = usePositions();

  const methods = useForm({
    resolver: zodResolver(schema),
    defaultValues: {
      firstName: "", lastName: "", email: "", password: "",
      phone: "", departmentId: "", positionId: "", hireDate: "", role: "Manager" as const,
    },
  });

  const onSubmit = (values: z.infer<typeof schema>) => {
    createMutation.mutate(values, { onSuccess: () => navigate("/employees") });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <Button variant="ghost" size="icon" onClick={() => navigate("/employees")}>
          <ArrowLeft className="h-4 w-4" />
        </Button>
        <div>
          <h1 className="text-2xl font-bold text-foreground">Create Employee</h1>
          <p className="text-muted-foreground">Add a new team member</p>
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
                <FormInput name="firstName" label="First Name" placeholder="John" />
                <FormInput name="lastName" label="Last Name" placeholder="Doe" />
                <FormInput name="email" label="Email" type="email" placeholder="john@company.com" />
                <FormInput name="password" label="Password" type="password" placeholder="••••••••" />
                <FormInput name="phone" label="Phone" placeholder="+1234567890" />
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
                <FormSelect
                  name="role"
                  label="Role"
                  options={[
                    { label: "Admin", value: "Admin" },
                    { label: "HR", value: "HR" },
                    { label: "Manager", value: "Manager" },
                  ]}
                />
              </div>
              <div className="flex gap-3 pt-4">
                <Button type="submit" disabled={createMutation.isPending}>
                  {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                  Create Employee
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
