#!/bin/sh

set -e

echo "Waiting for database and applying migrations..."

MAX_RETRIES=10
RETRY=0
SLEEP_SECONDS=2
CONNECTION_STRING="${CONNECTION_STRING:-$ConnectionStrings__TaskManagerMediatRDbContext}"

if [ -z "$CONNECTION_STRING" ]; then
  echo "ERROR: Connection string is not set (CONNECTION_STRING or ConnectionStrings__TaskManagerMediatRDbContext)"
  exit 1
fi

until /app/efbundle --connection "$CONNECTION_STRING"; do
  RETRY=$((RETRY + 1))

  if [ "$RETRY" -ge "$MAX_RETRIES" ]; then
    echo "ERROR: Failed to apply database migrations after $MAX_RETRIES attempts."
    exit 1
  fi

  echo "Database is not ready yet. Retry $RETRY/$MAX_RETRIES..."
  sleep $SLEEP_SECONDS
done

echo "Database migrations applied successfully."
echo "Starting application..."

exec dotnet TaskManagerMediatR.API.dll