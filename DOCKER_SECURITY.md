# Docker Build Security Guide

This document explains how to securely build the Docker image without exposing credentials.

## The Problem
Previously, the Dockerfile contained hardcoded credentials on line 19:
```dockerfile
RUN dotnet nuget add source https://nuget.pkg.github.com/itshubert/index.json --username itshubert --password <access token> --store-password-in-clear-text --name itshubert
```

This is insecure because:
- Credentials are visible in the Dockerfile
- They get stored in Docker image layers
- Anyone with access to the image can extract the tokens

## The Solution: Docker BuildKit Secrets

The updated Dockerfile uses Docker BuildKit's secret mount feature:
```dockerfile
RUN --mount=type=secret,id=github_username \
    --mount=type=secret,id=github_token \
    dotnet nuget add source https://nuget.pkg.github.com/itshubert/index.json \
    --username "$(cat /run/secrets/github_username)" \
    --password "$(cat /run/secrets/github_token)" \
    --store-password-in-clear-text --name itshubert
```

## How to Use

### Method 1: Using the PowerShell Script (Windows)
```powershell
# Set environment variables
$env:GITHUB_USERNAME = "itshubert"
$env:GITHUB_TOKEN = "your_github_token_here"

# Run the build script
.\build-secure.ps1
```

### Method 2: Using the Bash Script (Linux/Mac)
```bash
# Set environment variables
export GITHUB_USERNAME="itshubert"
export GITHUB_TOKEN="your_github_token_here"

# Make script executable and run
chmod +x build-secure.sh
./build-secure.sh
```

### Method 3: Manual Docker Build
```powershell
# Windows PowerShell
$env:DOCKER_BUILDKIT = 1

# Create temporary files
$usernameFile = [System.IO.Path]::GetTempFileName()
$tokenFile = [System.IO.Path]::GetTempFileName()
"itshubert" | Out-File -FilePath $usernameFile -NoNewline
"your_token_here" | Out-File -FilePath $tokenFile -NoNewline

# Build with secrets
docker build `
    --secret "id=github_username,src=$usernameFile" `
    --secret "id=github_token,src=$tokenFile" `
    -t pinoytodo-reader:latest `
    .

# Cleanup
Remove-Item $usernameFile, $tokenFile -Force
```

## Alternative Methods

### 1. Build Arguments (Less Secure)
If BuildKit secrets aren't available, you can use build arguments:

```dockerfile
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN
RUN dotnet nuget add source https://nuget.pkg.github.com/itshubert/index.json --username ${GITHUB_USERNAME} --password ${GITHUB_TOKEN} --store-password-in-clear-text --name itshubert
```

Build with:
```powershell
docker build --build-arg GITHUB_USERNAME=itshubert --build-arg GITHUB_TOKEN=your_token -t pinoytodo-reader:latest .
```

⚠️ **Warning**: Build arguments are still visible in `docker history` and should be avoided for sensitive data.

### 2. NuGet.Config File
Create a `NuGet.Config` file with placeholder values and mount the real one at build time:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/itshubert/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="itshubert" />
      <add key="ClearTextPassword" value="TOKEN_PLACEHOLDER" />
    </github>
  </packageSourceCredentials>
</configuration>
```

## Best Practices

1. **Never commit credentials** to version control
2. **Use environment variables** for local development
3. **Use CI/CD secrets** in automated builds
4. **Regularly rotate tokens** and update them in your secret management system
5. **Use minimal permissions** for GitHub tokens (only package read access needed)

## CI/CD Integration

For GitHub Actions, Azure DevOps, or other CI/CD systems:

```yaml
# GitHub Actions example
- name: Build Docker image
  env:
    DOCKER_BUILDKIT: 1
  run: |
    echo "${{ secrets.GITHUB_USERNAME }}" | docker secret create github_username -
    echo "${{ secrets.GITHUB_TOKEN }}" | docker secret create github_token -
    docker build --secret id=github_username --secret id=github_token -t pinoytodo-reader .
```

## Security Benefits

✅ **Credentials are not stored in image layers**  
✅ **Secrets are only available during build**  
✅ **No credential exposure in docker history**  
✅ **Supports credential rotation without code changes**  
✅ **Compatible with CI/CD secret management**  