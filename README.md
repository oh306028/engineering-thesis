# Web Application Supporting Early School Children's Learning with Elements of Gamification

## 📝 About the Project

This project is the practical component of an engineering thesis completed at the **Silesian University of Technology**. It is a modern Single Page Application (SPA) designed to address the challenges of distraction and lack of motivation among students in grades 1-3.

The system combines core education in mathematics, spelling, and logical thinking with mechanics known from video games to create an engaging and safe digital environment.

## 🚀 Key Features

* 
**Adaptive Learning System:** A custom algorithm called "Time Traveler" analyzes the number of attempts, errors, and the time since the last session to calculate a `FinalScore`, serving the five most challenging tasks to the student.


* 
**Gamification:** An integrated system of experience points (XP), levels, and unique badges (e.g., "Rocket Start", "Math Magician") to motivate consistent learning.


* 
**Virtual Classroom:** Students can join a digital counterpart of their physical class, fostering a sense of community and healthy competition through class rankings.


* 
**Communication Module:** A simplified student-teacher notification system based on pre-defined messages (e.g., "I have a problem with a task"), ensuring ease of use for the youngest users.


* 
**Parental & Teacher Control:** Dedicated panels for monitoring progress, managing educational content, and a verification process for teacher accounts.



## 🛠 Tech Stack

### Backend

* 
**Platform:** .NET 6 / ASP.NET Core.


* 
**Architecture:** **CQRS** (Command Query Responsibility Segregation) implemented via the **MediatR** library.


* 
**Database:** SQL Server with **Entity Framework Core** (Code First approach).


* 
**Cache:** **Redis** used for session management and performance optimization.


* 
**Cloud Storage:** **Azure Blob Storage** for secure handling of teacher certification documents and media files.



### Frontend

* 
**Library:** **React** + Vite.


* 
**Communication:** REST API with **JWT (JSON Web Token)** for stateless authentication.



### DevOps & Tools

* 
**Containerization:** **Docker & Docker Compose** for local infrastructure orchestration (SQL, Redis, Grafana).


* 
**Monitoring:** **Grafana** for real-time system metrics and administrative oversight.


* 
**Testing:** **xUnit** for unit and integration testing, alongside **Moq** for dependency isolation.



## 🏗 Project Structure

The application follows **Clean Code** principles, ensuring modularity and scalability.

```text
├── EngineeringThesis.API      # Punkty końcowe (Endpoints) i konfiguracja AutoMapper [cite: 337, 338]
├── EngineeringThesis.Logic    # Logika biznesowa, system adaptacyjny i zdarzenia (Events) [cite: 340, 341]
├── EngineeringThesis.Data     # Modele encji i kontekst bazy danych (EF Core) [cite: 343, 346]
├── EngineeringThesis.Web      # Frontend w React [cite: 40, 206]
└── EngineeringThesis.Tests    # Scenariusze testowe xUnit [cite: 347]

```

## ⚙️ Local Setup

1. 
**Clone the repository:** `git clone https://github.com/oh306028/engineering-thesis.git`.


2. 
**Infrastructure:** Run `docker-compose up -d` to launch required services.


3. 
**Configuration:** Update `appsettings.json` with your SQL Server and Azure Storage connection strings.


4. 
**Database Migration:** Execute `dotnet ef database update` within the Data project folder.


5. 
**Run Backend:** Execute `dotnet run` in the API folder.


6. 
**Run Frontend:** Execute `npm install` followed by `npm run dev` in the Web folder.



---

**Author:** Oskar Hamerla 
**Supervisor:** Dr. inż. Alina Momot **University:** Silesian University of Technology 

---
