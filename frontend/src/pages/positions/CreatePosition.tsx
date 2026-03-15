import { useNavigate } from "react-router-dom";
import { useForm, FormProvider } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useCreatePosition } from "@/hooks/usePositions";
import { useDepartments } from "@/hooks/useDepartments";
import { FormInput } from "@/components/shared/FormInput";
import { FormSelect } from "@/components/shared/FormSelect";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { ArrowLeft, Loader2 } from "lucide-react";

const schema = z.object({
  title: z.string().min(1, "Title is required"),
  departmentId: z.string().min(1, "Department is required"),
  description: z.string().min(1, "Description is required"),
});

export default function CreatePosition() {
  const navigate = useNavigate();
  const createMutation = useCreatePosition();
  const { data: departments } = useDepartments();

  const methods = useForm({
    resolver: zodResolver(schema),
    defaultValues: { title: "", departmentId: "", description: "" },
  });

  const onSubmit = (values: z.infer<typeof schema>) => {
    createMutation.mutate(values, { onSuccess: () => navigate("/positions") });
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center gap-4">
        <Button variant="ghost" size="icon" onClick={() => navigate("/positions")}>
          <ArrowLeft className="h-4 w-4" />
        </Button>
        <div>
          <h1 className="text-2xl font-bold text-foreground">Create Position</h1>
          <p className="text-muted-foreground">Add a new job position</p>
        </div>
      </div>
      <Card>
        <CardHeader><CardTitle>Position Details</CardTitle></CardHeader>
        <CardContent>
          <FormProvider {...methods}>
            <form onSubmit={methods.handleSubmit(onSubmit)} className="space-y-4">
              <FormInput name="title" label="Position Title" placeholder="Software Engineer" />
              <FormSelect
                name="departmentId"
                label="Department"
                options={departments?.map((d) => ({ label: d.name, value: d.id })) ?? []}
              />
              <FormInput name="description" label="Description" placeholder="Description..." />
              <div className="flex gap-3 pt-4">
                <Button type="submit" disabled={createMutation.isPending}>
                  {createMutation.isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                  Create Position
                </Button>
                <Button type="button" variant="outline" onClick={() => navigate("/positions")}>Cancel</Button>
              </div>
            </form>
          </FormProvider>
        </CardContent>
      </Card>
    </div>
  );
}
