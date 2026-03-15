import { useFormContext } from "react-hook-form";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

interface Props {
  name: string;
  label: string;
  type?: string;
  placeholder?: string;
}

export function FormInput({ name, label, type = "text", placeholder }: Props) {
  const { register, formState: { errors } } = useFormContext();
  const error = errors[name];

  return (
    <div className="space-y-1.5">
      <Label htmlFor={name}>{label}</Label>
      <Input id={name} type={type} placeholder={placeholder} {...register(name)} />
      {error && <p className="text-sm text-destructive">{error.message as string}</p>}
    </div>
  );
}
