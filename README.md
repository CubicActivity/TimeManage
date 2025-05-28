# TimeManage

## Installation and Setup

1. **Clone the repository:**

   ```bash
   git clone https://github.com/CubicActivity/TimeManage.git
   cd TimeManage

2. **Open the solution:**

    Open the solution file in Visual Studio 2019 (or later) with .NET Framework 4.7.2 support.

3. **Restore NuGet packages:**

    Restore the required NuGet packages either via Visual Studio (Tools > NuGet Package Manager > Manage NuGet Packages for Solution) or by running the following command in the Package Manager Console:
    ```
    Update-Package -reinstall
    ```
4. **Run the following command in the Package Manager Console to apply migrations and create the database schema:**

    ```
    Update-Database
    ```


## Features

- **Timetable Management:** Enables users to create, modify, and visualize daily and weekly schedules to optimize time allocation.  
- **Task Management (To-Do Lists):** Facilitates organization and prioritization of tasks through customizable to-do lists.  
- **Pomodoro Timer:** Incorporates Pomodoro timer to improve focus and productivity by dividing work into timed 25 minute intervals.

- Intuitive user interface implemented via ASP.NET MVC for seamless navigation.  
- Robust data handling using Entity Framework Core Code First approach.  
- Automatic database creation and migration management to streamline deployment.
 

## Prerequisites

- Windows operating system  
- Visual Studio 2019 (or later) with .NET Framework 4.7.2 support  
- SQL Server Express LocalDB (typically included with Visual Studio installations)  
 
