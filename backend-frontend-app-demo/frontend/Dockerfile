# Use Node.js as the base image
FROM node:18

# Set the working directory
WORKDIR /app

# Copy package.json and install dependencies
COPY package.json .
RUN npm install

# Install http-server globally to serve the frontend
RUN npm install -g http-server

# Copy the rest of the application files
COPY . .

# Expose the application port
EXPOSE 3000

# Start the application using http-server
CMD ["http-server", "-p", "3000", "-a", "0.0.0.0"]
