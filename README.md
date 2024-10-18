# 🎁 Gift Voting Application for Employee Birthdays 🎂

Welcome to the `Gift Voting Application!` This app is designed to help coworkers organize a group decision on what gift to choose for an employee's upcoming birthday. It ensures a seamless, transparent, and fun voting process without spoiling the surprise for the birthday person.

## 🏗️ Tech Stack

| **Component**       | **Technology**                        |
|---------------------|---------------------------------------|
| **Framework**       | C# with .NET 6 MVC                    |
| **Frontend**        | Razor Views (HTML, CSS, AJAX)         |
| **Backend**         | ASP.NET Core MVC                      |
| **Database**        | SQL Server                            |
| **Authentication**  | ASP.NET Identity for user management  |

## 🚀 Caching with Decorator Pattern

The application optimizes performance using the **Decorator Pattern** to introduce a memory cache layer without modifying the core business logic. Two caching services are used in the application:

- **VoteCachedService**: This service decorates the `VoteService` to cache vote results, ensuring that frequently accessed data is cached and retrieved efficiently. After a user casts a vote, the cached results are invalidated to guarantee consistency.
- **VoteSessionCachedService**: This service decorates the `VoteSessionService` and caches details of active and closed voting sessions, reducing database queries on repeated accesses.

#### How Caching Works:
1. **Caching Duration**: Cached data is stored for 5 minutes (configurable) to ensure data freshness while improving performance.
2. **Cache Invalidation**: Whenever a vote is cast or a vote session is created, updated, or closed, the relevant cache entries are automatically invalidated to ensure users see updated results.

#### Cached Methods:
- `GetVoteResultsAsync(voteSessionId, currentUserId)`
- `GetSessionDetailsAsync(sessionId, currentUserId)`
- `GetAllActiveSessionsAsync(currentUserId)`
- `GetAllClosedSessionsAsync(currentUserId)`

## 🔥 Error Handling and Validation

The application has robust error handling to ensure that invalid operations are not allowed. Some key validations include:

- **Session Validation**: Ensures only one active vote session per birthday employee is allowed in a year. Error messages such as `ActiveSessionExist` and `SessionAlreadyCreated` are used to inform users about existing sessions.
- **User Validation**: Users cannot vote for their own birthday sessions, and the application throws an error if the birthday employee tries to access session details (error message `BirthdayEmployeeRestrict`).
- **Vote Validation**: Users cannot vote more than once in the same session, and only active gifts can be voted on. The app handles cases like "already voted" and "invalid gift" with meaningful error messages (`AlreadyVoted`, `InvalidGift`).

The application ensures that all exceptions are caught and meaningful error messages are provided to users.

## 🛠 Service Layer Architecture

The application follows a service-oriented architecture where core business logic is encapsulated in service classes. These services follow the **Separation of Concerns** principle to ensure that data access, business logic, and presentation are clearly separated.

#### Core Services:
1. **VoteService**: Manages voting logic, including casting votes, validating votes, and retrieving vote results.
2. **VoteSessionService**: Manages voting sessions, such as creating, closing, and deleting sessions, as well as fetching session details.
3. **GiftService**: Manages gift data, including fetching available gifts for the voting process.
4. **EmployeeService**: Handles employee-related data, including fetching employee information for voting sessions.
5. **BaseService**: Provides shared functionality for all services, such as CRUD operations.

Each service implements its respective interface, allowing flexibility and adherence to SOLID principles. Additionally, some services are decorated with caching services (`VoteCachedService`, `VoteSessionCachedService`) to enhance performance.

## 🎉 How It Works

| **Feature**               | **Description**                                                                                          |
|---------------------------|----------------------------------------------------------------------------------------------------------|
| **Pre-loaded Data**        | The app comes with pre-loaded employee profiles and available gift suggestions. This data is stored in the database, so you can immediately start a voting session for an employee’s birthday. |
| **Initiating a Vote**      | Any registered user selects an upcoming birthday from the list of employees. They propose multiple gift ideas for the team to vote on. |
| **Voting Process**         | All users (except the birthday person) get notified of the active voting session. Each user selects their preferred gift via an AJAX-driven process for fast, seamless voting. |
| **Closing the Vote**       | The vote initiator can close the voting session when they believe enough votes have been collected. Users can see the voting results, revealing the most popular gift. |
| **Employee Registration**  | New employees can register through the app, allowing them to participate in future birthday voting sessions. |

## 🛠️ Installation and Setup

To get the project up and running locally:

1. **Clone the repository**:
    ```bash
    git clone https://github.com/Jelev123/BirthdayPresent.git
    cd BirthdayPresent
    ```

2. **Set up the database connection**:
   The application uses SQL Server for data storage. You need to update the `ConnectionStrings` section in your `appsettings.json` or `user secrets` file with your database configuration.
