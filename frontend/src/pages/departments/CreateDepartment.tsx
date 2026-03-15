import { useNavigate } from "react-router-dom";
import { useForm, FormProvider } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useCreateDepartment } from "@/hooks/useDepartments";
import { FormInput } from "@/components/shared/FormInput";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ArrowLeft, Loader2 } from "lucide-react";

const schema = z.object({
  name: z.string().min(1, "Name is required"),
  description: z.string().min(1, "Description is required"),
});

export default function CreateDepartment() {
  const navigate = useNavigate();
  const createMutation = useCreateDepartment();

  const methods = useForm({ resolver: zodResolver(schema), defaultValues: { name: "", description: "" } });

  const onSubmit = (values: z.infer<typeof schema>) => {
    createMutation.mutate(values, { onSuccess: () => navigate("/departments") });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <Button variant="ghost" size="icon" onClick={() => navigate("/departments")}>
          <ArrowLeft className="h-4 w-4" />
        </Button>
        <div>
          <h1 className="text-2xl font-bold text-foreground">Create Department</h1>
          <p className="text-muted-foreground">Add a new department</p>
        </div>
      </div>
      <Card>
        <CardHeader><CardTitle>Department Details</CardTitle></CardHeader>
        <CardContent>
          <FormProvider {...methods}>
            <form onSubmit={methods.handleSubmit(onSubmit)} className="space-y-4">
              <FormInput name="name" label="Department Name" placeholder="Engineering" />
              <FormInput name="description" label="Description" placeholder="Description..." />
              <div className="flex gap-3 pt-4">
                <Button type="submit" disabled={createMutation.isPending}>
                  {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                  Create Department
                </Button>
                <Button type="button" variant="outline" onClick={() => navigate("/departments")}>Cancel</Button>
              </div>
            </form>
          </FormProvider>
        </CardContent>
      </Card>
    </div>
  );
}
