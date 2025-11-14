# PowerShell script to build Docker image securely with build secrets
# This script demonstrates how to use Docker BuildKit secrets for secure credential handling

# Ensure BuildKit is enabled
$env:DOCKER_BUILDKIT = 1

# Check if required environment variables are set
if (-not $env:GITHUB_USERNAME) {
    Write-Error "GITHUB_USERNAME environment variable is required"
    exit 1
}

if (-not $env:GITHUB_TOKEN) {
    Write-Error "GITHUB_TOKEN environment variable is required"
    exit 1
}

Write-Host "Building Docker image with secure credentials..." -ForegroundColor Green

# Create temporary files for secrets (these will be deleted after build)
$usernameFile = [System.IO.Path]::GetTempFileName()
$tokenFile = [System.IO.Path]::GetTempFileName()

try {
    # Write secrets to temporary files
    $env:GITHUB_USERNAME | Out-File -FilePath $usernameFile -NoNewline -Encoding utf8
    $env:GITHUB_TOKEN | Out-File -FilePath $tokenFile -NoNewline -Encoding utf8

    # Build the Docker image with secrets mounted
    docker build `
        --secret "id=github_username,src=$usernameFile" `
        --secret "id=github_token,src=$tokenFile" `
        -t itshubert/pinoytodo.reader:latest `
        .

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Docker image built successfully!" -ForegroundColor Green
    } else {
        Write-Error "Docker build failed"
        exit $LASTEXITCODE
    }
}
finally {
    # Clean up temporary files
    if (Test-Path $usernameFile) { Remove-Item $usernameFile -Force }
    if (Test-Path $tokenFile) { Remove-Item $tokenFile -Force }
    Write-Host "Temporary credential files cleaned up" -ForegroundColor Yellow
}