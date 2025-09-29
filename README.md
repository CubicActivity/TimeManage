# Интернет Технологии – Проект за повисока оценка
**Изработил: Ангел Стојмановски (Индекс: 236059)**

## TimeManage
Manage your weekly schedules, long term goals and tasks, including a pomodoro timer to boost your productivity.

<img width="1657" height="892" alt="image" src="https://github.com/user-attachments/assets/b59048bc-15c6-43de-bcbd-a94ed8da2a78" />

## Installation and Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/CubicActivity/TimeManage.git
   cd TimeManage

2. **Open the project:**

    Open the solution file in Visual Studio 2019 (or later) with .NET Framework 4.7.2 support.

3. **Restore NuGet packages:**
   
    Restore the required NuGet packages either via Visual Studio (Tools > NuGet Package Manager > Manage NuGet Packages for Solution) or by running the following command in the Package Manager Console:
    ```
    Update-Package -reinstall
    ```
5. **Run the following command in the Package Manager Console to apply migrations and create the database schema:**

    ```
    Add-Migration start
    Update-Database
    ```


## Features

### Features

- **Timetable Management:** Easily create, edit, and view your daily and weekly schedules to make the most of your time.

- **Task Management:** Keep track of completed and pending tasks effortlessly.

- **Pomodoro Timer:** Boost productivity with timed work sessions. Use the default 25-minute Pomodoro or customize the duration to suit your workflow.

- **User-Friendly Interface:** Navigate your weekly schedule and tasks effortlessly with a clean, minimalistic design.

- **Automatic Database Setup:** The database is created and updated automatically, making deployment straightforward and hassle-free.
 

## Prerequisites

- Windows operating system  
- Visual Studio 2019 (or later) with .NET Framework 4.7.2 support  
- SQL Server Express LocalDB (typically included with Visual Studio installations)  




 
