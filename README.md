# Employee Management System with Enterprise Database Security

This project is a comprehensive Employee Management System built with a 3-tier C# WinForms architecture, backed by an Oracle 19c database. Its primary focus is on implementing advanced, enterprise-grade database security techniques to protect sensitive HR and payroll data.

## 🔐 5-Layer Database Security Architecture

This project stands out by implementing five distinct layers of Oracle database security, simulating a high-security enterprise environment:

1. **Role-Based Access Control (RBAC)**
   - Designed a robust role hierarchy (`dept_member_role`, `dept_manager_role`, `hr_staff_role`, `hr_manager_role`, `fin_staff_role`).
   - Ensures users only have the privileges necessary for their job functions.

2. **Virtual Private Database (VPD)**
   - Implemented Row-Level Security (RLS) using Application Contexts and Policy Functions.
   - Restricts data access dynamically based on the user's department and role (e.g., HR staff can see other departments but not their own payroll).

3. **Oracle Label Security (OLS)**
   - Applied Mandatory Access Control (MAC) using data classifications (PUBLIC, INTERNAL, CONFIDENTIAL, SECRET) and departmental groups.
   - Provides a secondary, coarse-grained defense layer complementing VPD.

4. **Data Redaction & Fine-Grained Auditing (FGA)**
   - **Redaction:** Masks sensitive data (like salaries and contact info) on the fly for unauthorized roles using wrapper roles.
   - **FGA:** Audits specific, sensitive actions (e.g., querying the payroll table) without modifying application code.

5. **Immutable Audit Logging**
   - Utilizes Oracle Blockchain Tables (`system_audit_log`) to ensure audit trails cannot be altered or deleted, even by DBAs.

## 🛠️ Technology Stack

* **Frontend:** C# Windows Forms (.NET Framework 4.7.2)
* **Backend Architecture:** 3-Tier (DTO - Data Transfer Objects, DAL - Data Access Layer, BLL - Business Logic Layer)
* **Database:** Oracle Database 19c (Enterprise Edition recommended for OLS)
* **Data Access:** Oracle.ManagedDataAccess (ODP.NET)

## 📂 Project Structure

* `/database/`: Contains all SQL scripts for database initialization and security configuration.
  * `SYS_SETUP.sql`: System user and audit schema setup.
  * `HR_ADMIN_DDL_INSERT.sql`: Table creation and sample data.
  * `RBAC.sql`: Role definitions and privilege grants.
  * `VPD_SETUP_FIXED.sql`: Virtual Private Database policies.
  * `OLS_SETUP.sql`: Oracle Label Security configuration.
  * `SETUP_REDACTION_FGA_NEW.sql`: Data masking and auditing policies.
  * `User_Data.sql`, `AddUser.sql`: Demo user provisioning.
* `/project/EmployeeManagementSystem/`: The C# source code.
  * `/DTO/`: Data structures.
  * `/DAL/`: Database interaction logic (`OracleConnectionFactory`, DAOs).
  * `/BLL/`: Business rules and validation.
  * `/EmployeeManagementSystem/`: WinForms UI components.
* `/reports/`: Project documentation and reports.

## 🚀 Setup & Installation

### 1. Database Setup
1. You need an Oracle Database instance (Note: Oracle Express Edition (XE) does *not* support Oracle Label Security).
2. Connect as `SYS AS SYSDBA` and execute the scripts in the `/database/` folder in the following order:
   - `SYS_SETUP.sql`
   - `HR_ADMIN_DDL_INSERT.sql`
   - `User_Data.sql`
   - `RBAC.sql`
   - `AddUser.sql`
   - `VPD_SETUP_FIXED.sql`
   - `OLS_SETUP.sql` *(Requires OLS option enabled via `chopt`)*
   - `SETUP_REDACTION_FGA_NEW.sql`

### 2. Application Setup
1. Open `project/EmployeeManagementSystem/EmployeeManagementSystem.sln` in Visual Studio.
2. Update the `OracleDataSource` connection string in `App.config` if your Oracle listener is on a different host/port.
3. Build and Run the application.

## 👤 Demo Credentials

*(Note: These are hardcoded for demonstration purposes only)*

* **minhlh** / `minhlh` (HR Manager - Full Access)
* **maintt** / `maintt` (Accounting Manager)
* **haitv**  / `haitv` (Finance Staff)
* **vietpq** / `vietpq` (Standard Employee - IT Dept)
