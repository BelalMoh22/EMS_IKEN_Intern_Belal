# HR Management System - Frontend Architecture & System Documentation

## 1. Project Overview
The **HR Management System (HRMS)** is a production-grade, enterprise-level frontend application designed to streamline human resource operations. It serves as the primary interface for managing employees, organizational structures (departments), and job roles (positions). 

The system provides a secure, role-based environment where administrative tasks such as employee registration, data modification, and organizational oversight are centralized. Built with a focus on efficiency and security, it leverages modern frontend patterns to ensure a high-performance user experience while maintaining strict adherence to business rules and security protocols.

---

## 2. Technology Stack Explanation

To build a reliable and scalable HR system, the following technologies were selected:

*   **React (v18+)**: The core library. Its component-based architecture and efficient reconciliation (Virtual DOM) allow for a highly responsive UI that can handle complex state updates gracefully.
*   **Vite**: The build tool of choice. Vite provides an extremely fast development environment via ES modules and highly optimized production builds using Rollup, significantly reducing build times compared to legacy tools like Webpack.
*   **TypeScript**: Provides static typing, which is critical for an enterprise application to prevent runtime errors, document data structures (like `Employee` and `User` types), and improve developer productivity through advanced IDE tooling.
*   **React Router (v6+)**: Handles client-side navigation. It provides a declarative way to manage complex routing hierarchies, protected routes, and role-based access control.
*   **Axios**: A promise-based HTTP client. It is used for backend communication due to its powerful interceptor support, allowing for centralized JWT handling and automatic token refreshing.
*   **TanStack Query (React Query)**: The state management layer for asynchronous data. It handles caching, synchronization, and "stale-while-revalidate" logic, removing the need for manual loading/error states in global stores.
*   **Zustand**: A lightweight, performant state management library for synchronous global state (Authentication, UI settings). It was chosen over Redux for its simplicity and minimal boilerplate.
*   **TailwindCSS**: A utility-first CSS framework that ensures design consistency and rapid UI development without leaving the HTML/JSX.
*   **Shadcn UI**: A collection of re-usable components built with Radix UI and Tailwind. It provides high-quality, accessible UI primitives that are fully customizable, ensuring a premium "Enterprise" look and feel.
*   **React Hook Form**: A library for managing form state. It minimizes re-renders and provides a robust solution for complex validation logic.
*   **Zod**: A TypeScript-first schema validation library. It is used in conjunction with React Hook Form to ensure strict data validation both at the UI level and before API submission.

---

## 3. Frontend Architecture

The project follows a **Modular Separation of Concerns (SoC)** architecture. While traditional systems often colocate everything by type, this architecture organizes code by technical responsibility, allowing for easy transitions toward a strictly **Feature-Based Architecture** as the system grows.

### Why this is Scalable:
1.  **Decoupled API Layer**: Business logic for data fetching is separated from the UI components.
2.  **Centralized State**: Global concerns like Authentication are isolated in dedicated Stores.
3.  **Primitive Reusability**: Common UI elements are abstracted into `components/ui` and `components/shared`, ensuring that updates to the design system propagate globally.
4.  **Hook-Driven Logic**: Complex logic is encapsulated in custom hooks, making components "thin" and focused purely on presentation.

---

## 4. Complete Project Folder Structure

```text
src/
├── api/            # API client and individual service modules
├── components/     # Reusable UI components
│   ├── auth/       # Authentication-specific guards (RBAC)
│   ├── layout/     # Core application wrappers (Sidebar, Header)
│   ├── shared/     # Form controls and general-purpose components
│   └── ui/         # Shadcn/UI primitive components
├── hooks/          # TanStack Query custom hooks for data fetching
├── lib/            # Third-party library configurations (utils, shadcn)
├── pages/          # Full page components (Views)
├── stores/         # Zustand global state (Auth, UI)
├── types/          # Global TypeScript interfaces and enums
└── App.tsx         # Root component and Routing configuration
```

### Folder Purposes:
*   **api/**: Contains the Axios instance and functions to interact with the .NET backend.
*   **components/**: Houses UI building blocks. Divided into `ui` (atomic), `shared` (molecular), and `layout` (organismic).
*   **hooks/**: The "brain" of data fetching. Every API entity has a corresponding hook file.
*   **pages/**: Represents the different routes. These components orchestrate the hooks and display the layout.
*   **stores/**: Manages non-persisted and persisted state like the user session.
*   **types/**: The single source of truth for data models shared across the application.

---

## 5. Detailed File-by-File Explanation

### API Layer (`src/api/`)
*   **`axios.ts`**: The core HTTP client. Configures `baseURL` and implements **Request/Response Interceptors**. It handles JWT injection and 401 token refresh logic.
*   **`authApi.ts`**: Dedicated service for Authentication. Handles `login` and `refreshToken` payload structures.
*   **`employeeApi.ts`**: Service for Employee management. Contains methods for fetching paginated lists, single details, creating (registering), and updating employee records.
*   **`departmentApi.ts`**: Service for Department CRUD. Communicates with the `/departments` backend endpoints.
*   **`positionApi.ts`**: Service for Position CRUD. Communicates with the `/positions` backend endpoints.

### State Management (`src/stores/`)
*   **`auth.ts`**: The central vault for the user's session. It defines the `AuthState` interface and uses Zustand's `persist` middleware to ensure the login persists across browser restarts.

### Authentication Guards (`src/components/auth/`)
*   **`ProtectedRoute.tsx`**: A higher-order component that restricts access to authenticated users only.
*   **`RoleBasedRoute.tsx`**: A granular permission guard. It compares the current user's role against an array of `allowedRoles` to permit or deny access to specific routes.

### Custom Hooks (`src/hooks/`)
*   **`useEmployees.ts`**: Encapsulates TanStack Query logic for employees. Contains `useEmployees` (fetch), `useEmployee` (fetch single), `useCreateEmployee`, `useUpdateEmployee`, and `useDeleteEmployee`.
*   **`useDepartments.ts`**: Manages department data state, including automatic cache invalidation after a department is created or updated.
*   **`usePositions.ts`**: Manages position data state. Handles the relationship between positions and their parent departments.
*   **`use-toast.ts`**: A utility hook from Shadcn for triggering UI notifications.
*   **`use-mobile.tsx`**: Responsive utility hook to detect screen size changes for sidebar behavior.

### Shared Components (`src/components/shared/`)
*   **`DataTable.tsx`**: A powerful, generic table component used throughout the app for listing data with support for sorting and actions.
*   **`FormInput.tsx`**: A wrapper around the standard HTML Input that integrates seamlessly with React Hook Form and displays Zod validation errors.
*   **`FormSelect.tsx`**: A customized select dropdown with support for dynamic options (e.g., selecting a Department by ID).
*   **`ConfirmDialog.tsx`**: A reusable modal for destructive actions (like deleting an employee) to prevent accidental data loss.
*   **`SearchInput.tsx`**: A debounced input used to filter tables and lists.
*   **`LoadingSpinner.tsx`**: A visual indicator used for asynchronous operations.

### Global Layout (`src/components/layout/`)
*   **`DashboardLayout.tsx`**: The master template for the authenticated area of the app.
*   **`AppSidebar.tsx`**: The navigation backbone. It computes which links to show based on the user's RBAC permissions.
*   **`Header.tsx`**: Top navigation bar containing breadcrumbs and the user's profile/logout menu.

### UI Primitives (`src/components/ui/`)
*   Includes atomic components like `Button.tsx`, `Card.tsx`, `Dialog.tsx`, etc. These are strictly presentational and based on Shadcn UI / Radix primitives.

### Pages & Routing (`src/pages/`)
*   **`Login.tsx`**: The login interface. It handles the initial authentication handshake.
*   **`Dashboard.tsx`**: The landing page showing high-level stats (Total Employees, etc.).
*   **`NotFound.tsx`**: A fallback "404" page.
*   **`employees/EmployeeList.tsx`**: The master view for browsing staff.
*   **`employees/CreateEmployee.tsx`**: The "Registration" form for adding new employees (Restricted to Admin/HR).
*   **`employees/EditEmployee.tsx`**: Form for updating existing records.
*   **`departments/DepartmentList.tsx`**: CRUD interface for managing company departments (Restricted to Admin).
*   **`departments/CreateDepartment.tsx`**: Form for defining new organizational units.
*   **`positions/PositionList.tsx`**: CRUD interface for job roles (Restricted to Admin).
*   **`positions/CreatePosition.tsx`**: Linkage view to create roles within departments.

### Utilities and Types (`src/lib/`, `src/types/`)
*   **`lib/utils.ts`**: Contains helper functions like `cn` for Tailwind class merging.
*   **`types/index.ts`**: The TypeScript source of truth for the entire project. Defines all API Request/Response interfaces and the `Role` enum.

---

## 6. Authentication Flow

1.  **Submission**: User enters credentials in the `Login` page.
2.  **Request**: Data is validated by **Zod** and sent via `authApi.login`.
3.  **Response**: Backend returns a `token`, `refreshToken`, and `User` object.
4.  **Storage**: Zustand's `setAuth` updates the global state and persists it to `localStorage`.
5.  **Interception**: The `axios.ts` interceptor detects the new token and starts attaching `Authorization: Bearer <token>` to all subsequent requests.
6.  **Navigation**: The user is redirected to `/dashboard`.

---

## 7. Role-Based Authorization (RBAC)

The system enforces permissions at the **UI level** via the `RoleBasedRoute` and dynamic navigation.

| Role | Permissions | Navigation Visibility |
| :--- | :--- | :--- |
| **Admin** | Full CRUD on all modules | All links visible |
| **HR** | Create/Update/View Employees | Employees (Write access), Departments (Read Only) |
| **Manager** | View Employees only | Employees (Read Only) |

### Sidebar Implementation:
The `AppSidebar` component checks `user.role` from the Zustand store. Links like "Departments" or "Settings" are conditionally rendered only for the `Admin` role.

---

## 8. API Communication

The system uses a centralized Axios instance located in `src/api/axios.ts`. 

*   **Instance Setup**: Configured with a `baseURL` pointing to the .NET Minimal API.
*   **Security**: The Request Interceptor automatically injects the Bearer token from the `authStore`.
*   **Robustness**: If a request fails due to an expired token, the Response Interceptor pauses the request, calls the refresh endpoint, updates the store, and retries the original request seamlessly.

---

## 9. Data Fetching Architecture

**TanStack Query** acts as the server-state cache manager.

*   **Queries**: Used for fetching data (e.g., `useEmployees`). It handles loading states and caching. Data is automatically refreshed when the window is refocused.
*   **Mutations**: Used for modifying data (e.g., `createEmployee`). After a successful mutation, `queryClient.invalidateQueries` is called to force a background refresh of relevant lists, ensuring the UI is always up-to-date.

---

## 10. Form Handling

Forms are the backbone of the HRMS. The synergy between **React Hook Form** and **Zod** provides:

1.  **Type Safety**: Zod schemas generate TypeScript types, ensuring the data sent to the backend matches the API contracts exactly.
2.  **Performance**: Uncontrolled components are used to avoid unnecessary React re-renders during typing.
3.  **User Experience**: Real-time error messages are displayed per field as the user types or on submit.

---

## 11. Dashboard UI Architecture

The Dashboard uses a **Composite Layout Strategy**:

*   **Sidebar**: A persistent left-hand navigation. Uses `lucide-react` for iconography.
*   **Header**: High-level actions and path navigation (Breadcrumbs).
*   **Main Content**: Encapsulated in the `DashboardLayout`, which provides consistent padding and scroll behavior for all sub-pages.

---

## 12. Application Routing

Routing is defined in `src/App.tsx` using a nested approach:

```tsx
<Routes>
  <Route path="/login" element={<Login />} />
  <Route element={<ProtectedRoute><DashboardLayout /></ProtectedRoute>}>
    <Route path="/dashboard" element={<Dashboard />} />
    <Route path="/employees" element={<EmployeeList />} />
    {/* Admin/HR Restricted */}
    <Route path="/employees/create" element={<RoleBasedRoute allowedRoles={['Admin', 'HR']}><CreateEmployee /></RoleBasedRoute>} />
    {/* Admin Only Restricted */}
    <Route path="/departments" element={<RoleBasedRoute allowedRoles={['Admin']}><DepartmentList /></RoleBasedRoute>} />
  </Route>
</Routes>
```

---

## 13. Complete System Flow

1.  **Initialization**: App loads; Zustand checks `localStorage` for an existing session.
2.  **Navigation Guard**: `ProtectedRoute` checks if the user is allowed.
3.  **Data Hydration**: Dashboard renders; `useEmployees` and other hooks fetch initial data from the .NET backend.
4.  **Transaction**: Admin clicks "Create Employee". The form validates via Zod. On submit, a POST request is sent.
5.  **State Sync**: TanStack Query invalidates the `employees` cache; the list UI automatically re-fetches and shows the new employee.

---

## 14. Conclusion & Scalability

The HR Management System frontend is architected for long-term growth. By isolating state, styling, logic, and API communication into distinct layers, new modules (e.g., Payroll, Recruiting) can be added by simply creating new pages and hooks without risk of breaking existing functionality.

The use of **TypeScript** and **Zod** ensures that as the backend .NET API evolves, the frontend can be updated with high confidence, maintaining a robust bridge zwischen the database and the user interface.
