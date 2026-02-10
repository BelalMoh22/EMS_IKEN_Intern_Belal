CREATE TABLE Departments
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    DepartmentName NVARCHAR(150) NOT NULL,

    ManagerId INT NOT NULL,

    IsActive BIT NULL DEFAULT 1
);

CREATE TABLE Positions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    PositionName NVARCHAR(150) NOT NULL,

    MinSalary DECIMAL(18,2) NOT NULL,

    MaxSalary DECIMAL(18,2) NOT NULL,

    DepartmentId INT NOT NULL,

    CONSTRAINT FK_Employee_Department
    FOREIGN KEY (DepartmentId)
    REFERENCES Departments(Id),

    CONSTRAINT CK_Position_Salary
        CHECK (MinSalary <= MaxSalary)
);


CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,

    FirstName NVARCHAR(100) NOT NULL,

    Lastname NVARCHAR(100) NOT NULL,

    NationalId INT NOT NULL UNIQUE,

    Email NVARCHAR(150) NOT NULL UNIQUE,

    PhoneNumber NVARCHAR(20) NOT NULL,

    DateOfBirth DATETIME2 NOT NULL,

    Address NVARCHAR(250) NOT NULL,

    Salary DECIMAL(18,2) NOT NULL,

    HireDate DATETIME2 NULL DEFAULT SYSUTCDATETIME(),

    Status INT NULL DEFAULT 1,

    PositionId INT NOT NULL,

    -- Soft Delete
    IsDeleted BIT NOT NULL DEFAULT 0,

    DeletedAt DATETIME2 NULL,

    CONSTRAINT FK_Employee_Position
        FOREIGN KEY (PositionId)
        REFERENCES Positions(Id)
);

INSERT INTO Departments (DepartmentName, ManagerId, IsActive)
VALUES
('Human Resources', 1, 1),
('Information Technology', 2, 1),
('Finance', 3, 1);

INSERT INTO Positions (PositionName, MinSalary, MaxSalary, DepartmentId)
VALUES
-- HR Department (Id = 1)
('HR Specialist', 5000, 9000, 1),

-- IT Department (Id = 2)
('Software Engineer', 8000, 15000, 2),
('Senior Software Engineer', 12000, 22000, 2),

-- Finance Department (Id = 3)
('Accountant', 6000, 12000, 3),
('Finance Manager', 15000, 25000, 3);


INSERT INTO Employees
(
    FirstName,
    Lastname,
    NationalId,
    Email,
    PhoneNumber,
    DateOfBirth,
    Address,
    Salary,
    HireDate,
    Status,
    PositionId
)
VALUES
-- HR Employees
('Ahmed', 'Hassan', 111111111, 'ahmed.hassan@company.com', '01011111111',
 '1995-04-10', 'Cairo, Egypt', 7000, SYSDATETIME(), 1, 1),

('Mona', 'Ali', 222222222, 'mona.ali@company.com', '01022222222',
 '1993-09-15', 'Giza, Egypt', 8500, SYSDATETIME(), 1, 1),

-- IT Employees
('Omar', 'Youssef', 333333333, 'omar.youssef@company.com', '01033333333',
 '1998-01-20', 'Nasr City, Cairo', 12000, SYSDATETIME(), 1, 2),

('Sara', 'Mahmoud', 444444444, 'sara.mahmoud@company.com', '01044444444',
 '1996-06-05', 'Maadi, Cairo', 14000, SYSDATETIME(), 1, 3),

('Khaled', 'Mostafa', 555555555, 'khaled.mostafa@company.com', '01055555555',
 '1990-11-30', 'Heliopolis, Cairo', 20000, SYSDATETIME(), 1, 3),

-- Finance Employees
('Nour', 'Adel', 666666666, 'nour.adel@company.com', '01066666666',
 '1994-03-18', 'Dokki, Giza', 9000, SYSDATETIME(), 1, 4),

('Hany', 'Fathy', 777777777, 'hany.fathy@company.com', '01077777777',
 '1988-07-22', 'Mohandessin, Giza', 16000, SYSDATETIME(), 1, 5),

('Laila', 'Samir', 888888888, 'laila.samir@company.com', '01088888888',
 '1992-12-01', 'New Cairo', 18000, SYSDATETIME(), 1, 5);