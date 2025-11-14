# Alternative: Bash script for Linux/Mac users
#!/bin/bash

# Ensure BuildKit is enabled
export DOCKER_BUILDKIT=1

# Check if required environment variables are set
if [ -z "$GITHUB_USERNAME" ]; then
    echo "Error: GITHUB_USERNAME environment variable is required"
    exit 1
fi

if [ -z "$GITHUB_TOKEN" ]; then
    echo "Error: GITHUB_TOKEN environment variable is required"
    exit 1
fi

echo "Building Docker image with secure credentials..."

# Create temporary files for secrets
username_file=$(mktemp)
token_file=$(mktemp)

# Cleanup function
cleanup() {
    rm -f "$username_file" "$token_file"
    echo "Temporary credential files cleaned up"
}

# Set trap to cleanup on exit
trap cleanup EXIT

# Write secrets to temporary files
echo -n "$GITHUB_USERNAME" > "$username_file"
echo -n "$GITHUB_TOKEN" > "$token_file"

# Build the Docker image with secrets mounted
docker build \
    --secret "id=github_username,src=$username_file" \
    --secret "id=github_token,src=$token_file" \
    -t pinoytodo-reader:latest \
    .

if [ $? -eq 0 ]; then
    echo "Docker image built successfully!"
else
    echo "Docker build failed"
    exit 1
fi