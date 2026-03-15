import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import { Toaster as Sonner } from "@/components/ui/sonner";
import { Toaster } from "@/components/ui/toaster";
import { TooltipProvider } from "@/components/ui/tooltip";
import { ProtectedRoute } from "@/components/auth/ProtectedRoute";
import { RoleBasedRoute } from "@/components/auth/RoleBasedRoute";
import { DashboardLayout } from "@/components/layout/DashboardLayout";
import Login from "@/pages/Login";
import Dashboard from "@/pages/Dashboard";
import EmployeeList from "@/pages/employees/EmployeeList";
import CreateEmployee from "@/pages/employees/CreateEmployee";
import EditEmployee from "@/pages/employees/EditEmployee";
import DepartmentList from "@/pages/departments/DepartmentList";
import CreateDepartment from "@/pages/departments/CreateDepartment";
import PositionList from "@/pages/positions/PositionList";
import CreatePosition from "@/pages/positions/CreatePosition";
import NotFound from "@/pages/NotFound";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: { retry: 1, refetchOnWindowFocus: false },
  },
});

const App = () => (
  <QueryClientProvider client={queryClient}>
    <TooltipProvider>
      <Toaster />
      <Sonner />
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route
            element={
              <ProtectedRoute>
                <DashboardLayout />
              </ProtectedRoute>
            }
          >
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/employees" element={<EmployeeList />} />
            <Route
              path="/employees/create"
              element={
                <RoleBasedRoute allowedRoles={["Admin", "HR"]}>
                  <CreateEmployee />
                </RoleBasedRoute>
              }
            />
            <Route
              path="/employees/edit/:id"
              element={
                <RoleBasedRoute allowedRoles={["Admin", "HR"]}>
                  <EditEmployee />
                </RoleBasedRoute>
              }
            />
            <Route
              path="/departments"
              element={
                <RoleBasedRoute allowedRoles={["Admin"]}>
                  <DepartmentList />
                </RoleBasedRoute>
              }
            />
            <Route
              path="/departments/create"
              element={
                <RoleBasedRoute allowedRoles={["Admin"]}>
                  <CreateDepartment />
                </RoleBasedRoute>
              }
            />
            <Route
              path="/positions"
              element={
                <RoleBasedRoute allowedRoles={["Admin"]}>
                  <PositionList />
                </RoleBasedRoute>
              }
            />
            <Route
              path="/positions/create"
              element={
                <RoleBasedRoute allowedRoles={["Admin"]}>
                  <CreatePosition />
                </RoleBasedRoute>
              }
            />
          </Route>
          <Route path="/" element={<Navigate to="/dashboard" replace />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </TooltipProvider>
  </QueryClientProvider>
);

export default App;
