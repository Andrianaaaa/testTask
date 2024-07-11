# TestingTask

## How to run the project

1. Clone the repository
2. Ensure you have docker installed on your machine
3. Run the following command to run the system
```bash
docker-compose up -d --build
docker-compose logs -f notification
```
4. Run the CLI with following command:
```bash
dotnet run --project CLI
```
5. Fill in the credentials as requested by the CLI with next data:
- Username: `andriana`
- Password: `123`

6. Observe the logs in the notification service and the CLI output.

## Architecture

```mermaid
sequenceDiagram
    participant CLI
    participant AuthService
    participant NotificationService

    CLI->>AuthService: Login Request
    AuthService-->>CLI: Validate Credentials
    AuthService-->>CLI: Auth Successful / Failure Response
    AuthService->>NotificationService: Notify of Successful Login
    NotificationService-->>AuthService: Acknowledge Notification
    CLI->>AuthService: Get user profile
    AuthService->>NotificationService: Notify of requesting user profile
    NotificationService-->>AuthService: Acknowledge Notification
    AuthService-->>CLI: Return user profile
```
