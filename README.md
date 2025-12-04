# 🔐 MyPass – Secure Password Vault

MyPass is a full-featured secure password management application inspired by Bitwarden.  
Developed for CIS-476 Software Engineering.

---

## 🚀 Features
- User registration + login
- Password strength meter + generator
- Three security questions for account recovery
- Fully encrypted vault (AES encryption)
- CRUD for Login, Credit Card, Identity, and Secure Notes
- Mask/unmask sensitive data
- Copy to clipboard + auto-clear
- Responsive UI (Bootstrap 5)

---

## 🧩 Design Patterns Used
| Pattern | Location | Purpose |
|--------|----------|---------|
| Singleton | WebSessionManager | Central session control |
| Builder | PasswordBuilder | Strong password generation |
| Proxy | SensitiveFieldProxy | Mask/unmask data |
| Observer | PasswordStrengthMonitor | Weak password detection |
| Chain of Responsibility | PasswordRecoveryService | Multi-step recovery |

---

## 🛠 Technology
- ASP.NET Core MVC (C#)
- Entity Framework Core
- SQL Server
- Bootstrap 5
- Dependency Injection

---

## 📦 How to Run
1. Clone repo  
2. Update DB connection in `appsettings.json`  
3. Run migrations  
4. Start project (`dotnet run` or Visual Studio)

---

## 📚 License
Educational project – not intended for production use.
