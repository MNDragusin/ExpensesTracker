# 💰 ExpensesTracker

**ExpensesTracker** is a lightweight virtual wallet designed to help you keep track of your expenses and savings. It provides a simple and intuitive interface for managing your personal finances.

---

## 📌 Features

- Track daily expenses and monitor your savings
- Temporary support for **SQLite** during early development
- Future plan to migrate storage to a **container-based database system**

---

## 🛠 Setup Instructions

### ⚙️ Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- Entity Framework tools: install with command:
```bash
    dotnet tool install --global dotnet-ef  
```

### 🧱 Database Initialization

> ⚠️ Currently, the project uses **SQLite** for development and testing purposes. A containerized database (e.g., PostgreSQL or SQL Server) is planned for future releases.

To set up the SQLite database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🧼 Cleaning the Build
### If you experience issues during debugging or encounter unexpected build behavior, it’s often helpful to clean the solution by removing the /bin and /obj folders.

To simplify this process, two cleanup scripts are provided:

🖥️ Windows
Run the following script:
```bash
    clean_src.bat
```

🐧 Linux / 🍎 macOS
Make the script executable (only needed once):

```bash
    chmod +x clean-src.sh
```
Then run it:
```bash
    ./clean-src.sh
```

These scripts scan the src directory and remove bin and obj folders from its immediate subdirectories (one level deep).