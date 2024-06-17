# Elasticsearch Integration with .NET Core using NEST

This project demonstrates how to integrate Elasticsearch into a .NET Core application using the NEST (Elasticsearch .NET Client) package. Below are the features and functionalities developed:

## Features

### 1. Setup Elasticsearch Configuration

- **Configuration Setup**: 
  - Configured Elasticsearch client in `Program.cs` using dependency injection.
  - Read Elasticsearch server URL and optional authentication credentials (username/password) from `appsettings.json`.

### 2. Search Operations

- **Search by Category**:
  - Implemented a method to search for products by category in Elasticsearch.
  - Used NEST query builders (`Match` and `Term`) to construct search queries dynamically.
  - Handled search responses and errors appropriately using `IsValid` property of the response object.

### 3. Elasticsearch Client Setup

- **Elasticsearch Client Configuration**:
  - Created an instance of `ElasticClient` in `Program.cs`.
  - Configured basic authentication if provided in the configuration.
  - Used `ConnectionSettings` to define Elasticsearch server URL and authentication settings.

### 4. Error Handling

- **Error Handling**:
  - Implemented error handling for Elasticsearch search operations.
  - Logged Elasticsearch server errors and exceptions to console or logging framework (e.g., Serilog, NLog).

## Installation and Usage

To use this project, follow these steps:

1. **Clone Repository**:
   ```bash
   git clone https://github.com/your/repository.git
   cd repository-name
