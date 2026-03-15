import { Input } from "@/components/ui/input";
import { Search } from "lucide-react";
import { useEffect, useState } from "react";

interface Props {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

export function SearchInput({ value, onChange, placeholder = "Search..." }: Props) {
  const [internal, setInternal] = useState(value);

  useEffect(() => {
    const timer = setTimeout(() => onChange(internal), 300);
    return () => clearTimeout(timer);
  }, [internal, onChange]);

  useEffect(() => setInternal(value), [value]);

  return (
    <div className="relative">
      <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
      <Input
        value={internal}
        onChange={(e) => setInternal(e.target.value)}
        placeholder={placeholder}
        className="pl-9"
      />
    </div>
  );
}
