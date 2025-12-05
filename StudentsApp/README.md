# StudentsAPP – QA Automation Technical Exercise

<a name="introduction"></a>
## Introduction

This solution contains:
- Unit test framework project called TestApp
- Component test framework project called TestAppAPI
- Solution project containing some classes that modelate business logic and data to be tested. 

Both automation projects are built with **C#**, and **NUnit Framework**.

---
<a name="Project Structure"></a>

### 📁 Project Structure

```bash
📁 StudentsApp
├── 📁 Controllers/       # Controllers classes exposing endpoints and handling communication to services
├── 📁 Dtos/              # Classes holding structured test data (e.g., users, study groups.)
├── 📁 Repositories/      # Repository classes implemeting data access
├── 📁 Services/          # Classes implementing application rules for business logic
📁 TestApp
├── 📁 Tests/             # Unit tests for domain entities and service logic
📁 TestAppAPI
├── 📁 Tests/             # Component tests validating controller behavior and API responses
│
└📄 README.md             # Documentation file
```
