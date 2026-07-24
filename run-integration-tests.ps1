# run-integration-tests.ps1
docker compose -f docker-compose.integration-tests.yml run --rm --build integration-tests
exit $LASTEXITCODE