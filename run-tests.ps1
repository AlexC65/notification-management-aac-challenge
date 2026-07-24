Write-Host "Building and starting the application..." -ForegroundColor Cyan
docker compose up --build -d

Write-Host "`nRunning unit tests..." -ForegroundColor Cyan
docker compose -f docker-compose.unit-tests.yml run --rm --build unit-tests
$unitExitCode = $LASTEXITCODE

Write-Host "`nRunning integration tests..." -ForegroundColor Cyan
docker compose -f docker-compose.integration-tests.yml run --rm --build integration-tests
$integrationExitCode = $LASTEXITCODE

if ($unitExitCode -ne 0 -or $integrationExitCode -ne 0) {
    Write-Host "`nSome tests failed." -ForegroundColor Red
    exit 1
} else {
    Write-Host "`nAll tests passed!" -ForegroundColor Green
    exit 0
}