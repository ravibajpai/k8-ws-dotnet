# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) to the container
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application to a self-contained executable
RUN dotnet publish -c Release -o out --self-contained -r linux-x64 /p:PublishSingleFile=true /p:EnableCompressionInSingleFile=true

# Runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app/out .

# Expose ports if your app needs networking (e.g., for a web app)
# EXPOSE 5000

# Make the executable runnable
RUN chmod +x OrderService

# Set the entry point to your application executable
ENTRYPOINT ["./OrderService"]