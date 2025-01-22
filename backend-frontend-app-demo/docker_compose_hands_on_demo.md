
# Hands-On Session: Docker Compose Demo

Welcome to the hands-on session! This guide will walk you through setting up and running a simple multi-container application using Docker Compose. The application includes:

1. **Frontend**: A static web page served by `http-server`.
2. **Backend**: A Node.js server exposing an API.
3. **Database**: A PostgreSQL database.

---

## Objectives

By the end of this session, you will:
- Understand Docker Compose basics.
- Run a multi-container application.
- Explore frontend, backend, and database services.

---

## Prerequisites

Ensure you have the following installed on your github workspace:
- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/)
- A GitHub repository where you can create a Codespace.
- Docker installed in your GitHub Codespace (pre-installed in most setups).

---

## Step 1: Set Up the Project

### 1. Create the following files and folder

Create the files and folder in your code repo as shown below.

### Project Structure
```
docker-compose-demo/
├── docker-compose.yml
├── frontend/
│   ├── Dockerfile
│   ├── package.json
│   ├── index.html
│   └── src/
│       └── index.js
├── backend/
│   ├── Dockerfile
│   ├── package.json
│   ├── server.js
├── init.sql
```

---

## Step 2: Copy and Create the Files

### `docker-compose.yml`
```yaml
version: '3.9'
services:
  frontend:
    build: ./frontend
    ports:
      - "3000:3000"
    depends_on:
      - backend

  backend:
    build: ./backend
    ports:
      - "5000:5000"
    environment:
      - DATABASE_URL=postgresql://postgres:password@db:5432/demo
    depends_on:
      - db

  db:
    image: postgres:15
    container_name: demo_db
    restart: always
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: password
      POSTGRES_DB: demo
    volumes:
      - db_data:/var/lib/postgresql/data
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql:ro

volumes:
  db_data:
```

### `frontend/Dockerfile`
```dockerfile
FROM node:18
WORKDIR /app
COPY package.json .
RUN npm install
RUN npm install -g http-server
COPY . .
EXPOSE 3000
CMD ["http-server", "-p", "3000", "-a", "0.0.0.0"]
```

### `frontend/package.json`
```json
{
  "name": "frontend",
  "version": "1.0.0",
  "scripts": {
    "start": "http-server -p 3000 -a 0.0.0.0"
  },
  "dependencies": {
    "http-server": "^14.1.1"
  }
}
```

### `frontend/index.html`
```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Frontend Demo</title>
</head>
<body>
  <h1>Welcome to the Frontend Demo!</h1>
  <p>This is a simple static page served by http-server.</p>
</body>
</html>
```

### `frontend/src/index.js`
```log
console.log('Frontend is running!');
```

### `backend/Dockerfile`
```dockerfile
FROM node:18
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
EXPOSE 5000
CMD ["node", "server.js"]
```

### `backend/package.json`
```json
{
  "name": "backend",
  "version": "1.0.0",
  "scripts": {
    "start": "node server.js"
  },
  "dependencies": {
    "express": "^4.18.2"
  }
}
```

### `backend/server.js`
```javascript
const express = require('express');
const app = express();
const PORT = 5000;

app.get('/', (req, res) => {
  res.send('Hello from Backend!');
});

app.listen(PORT, () => {
  console.log(`Backend running on http://localhost:${PORT}`);
});
```

### `init.sql`
```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100)
);

INSERT INTO users (name) VALUES ('Alice'), ('Bob');
```

---

## Step 3: Build and Run the Project

### Build the Docker Images
```bash
docker-compose build
```

### Start the Containers
```bash
docker-compose up
```

---

## Step 4: Access the Services

### Frontend
- Open [http://localhost:3000](http://localhost:3000).

### Backend
- Test the backend API at [http://localhost:5000](http://localhost:5000).

### Database
- PostgreSQL is running internally on port `5432`.

---

## Troubleshooting

### GitHub Codespaces
If you’re using GitHub Codespaces, make sure to forward the necessary ports:
1. Open the **Ports** tab in Codespaces.
2. Forward **Port 3000** to access the frontend.

---

## Step 5: Stop the Services

To stop all containers:
```bash
docker-compose down
```

---

Congratulations! You’ve completed the hands-on session.
