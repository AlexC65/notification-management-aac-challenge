# run-unit-tests.ps1
docker compose -f docker-compose.unit-tests.yml run --rm --build unit-tests
exit $LASTEXITCODE