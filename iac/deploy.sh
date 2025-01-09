
# Base docker-compose file
COMPOSE_FILES="-f docker-compose.yml"

# Check if the override file exists and add it if it does
if [[ -f docker-compose.override.yml ]]; then
    COMPOSE_FILES="$COMPOSE_FILES -f docker-compose.override.yml"
fi

docker compose $COMPOSE_FILES up -d --build