🎁 Gift Voting Application for Employee Birthdays 🎂

Welcome to the `Gift Voting Application!` This app is designed to help coworkers organize a group decision on what gift to choose for an employee's upcoming birthday. It ensures a seamless, transparent, and fun voting process without spoiling the surprise for the birthday person.

## 🏗️ Tech Stack

| **Component**       | **Technology**                        |
|---------------------|---------------------------------------|
| **Framework**       | C# with .NET 6 MVC                    |
| **Frontend**        | Razor Views (HTML, CSS, AJAX)         |
| **Backend**         | ASP.NET Core MVC                      |
| **Database**        | SQL Server                            |
| **Authentication**  | ASP.NET Identity for user management  |


🛠️ Design Patterns
This project leverages design patterns to ensure scalable and maintainable code. One key pattern used is:

Decorator Pattern for Caching
The Decorator Pattern is implemented to manage in-memory caching across several services. This helps improve performance by caching frequently accessed data (such as vote session details and voting results), reducing redundant database calls, and improving overall application responsiveness.

How It's Implemented:
Two key classes, [VoteCachedService](https://github.com/Jelev123/BirthdayPresent/blob/main/BirthdayPresent.Core/Services/Cached/VoteCachedService.cs) and [VoteSessionCachedService](https://github.com/Jelev123/BirthdayPresent/blob/main/BirthdayPresent.Core/Services/Cached/VoteSessionCachedService.cs), wrap around the core services (VoteService and VoteSessionService respectively) to add caching functionality without modifying the core business logic. Here’s a breakdown of how it works:

`VoteCachedService:`

This class decorates the VoteService by adding a memory cache layer.
It caches the results of a vote session (GetVoteResultsAsync) to reduce repeated database queries when users check voting results.
After a vote is cast (VoteForGiftAsync), the relevant cache entries are invalidated to ensure fresh data on subsequent requests.

`VoteSessionCachedService:`

This class decorates the VoteSessionService to cache information about vote sessions.
It caches details of active and closed voting sessions, session details (GetSessionDetailsAsync), and the user's vote for a session (GetUserVoteAsync).
The cache is invalidated when a session is created, closed, or deleted to ensure that updates reflect immediately.


## 🎉 How It Works

| **Feature**               | **Description**                                                                                          |
|---------------------------|----------------------------------------------------------------------------------------------------------|
| **Pre-loaded Data**        | The app comes with pre-loaded employee profiles and available gift suggestions. This data is stored in the database, so you can immediately start a voting session for an employee’s birthday. |
| **Initiating a Vote**      | Any registered user selects an upcoming birthday from the list of employees. They propose multiple gift ideas for the team to vote on. |
| **Voting Process**         | All users (except the birthday person) get notified of the active voting session. Each user selects their preferred gift via an AJAX-driven process for fast, seamless voting. |
| **Closing the Vote**       | The vote initiator can close the voting session when they believe enough votes have been collected. Users can see the voting results, revealing the most popular gift. |
| **Employee Registration**  | New employees can register through the app, allowing them to participate in future birthday voting sessions. |

🛠️ Installation
To get the project up and running locally:

Clone this repository: https://github.com/Jelev123/BirthdayPresent.git
After that it have to change the `ConnectionStrings` which is located in `Manage user secrets`
