#!/usr/bin/env bash
set -e

echo "Building and starting the application..."
docker compose up --build -d

echo ""
echo "Running unit tests..."
docker compose -f docker-compose.unit-tests.yml run --rm --build unit-tests

echo ""
echo "Running integration tests..."
docker compose -f docker-compose.integration-tests.yml run --rm --build integration-tests

echo ""
echo "All tests passed!"