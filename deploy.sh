#!/bin/bash

# Schemes Management System Deployment Script
# Usage: ./deploy.sh [environment] [action]
# Environments: dev, qa, uat, prod
# Actions: up, down, restart, logs, build

set -e

ENVIRONMENT=${1:-dev}
ACTION=${2:-up}

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Validate environment
validate_environment() {
    case $ENVIRONMENT in
        dev|qa|uat|prod)
            print_status "Environment: $ENVIRONMENT"
            ;;
        *)
            print_error "Invalid environment: $ENVIRONMENT"
            echo "Valid environments: dev, qa, uat, prod"
            exit 1
            ;;
    esac
}

# Validate action
validate_action() {
    case $ACTION in
        up|down|restart|logs|build)
            print_status "Action: $ACTION"
            ;;
        *)
            print_error "Invalid action: $ACTION"
            echo "Valid actions: up, down, restart, logs, build"
            exit 1
            ;;
    esac
}

# Set compose files
set_compose_files() {
    COMPOSE_FILES="-f docker-compose.yml -f docker/docker-compose.${ENVIRONMENT}.yml"

    if [ "$ENVIRONMENT" = "prod" ]; then
        COMPOSE_FILES="$COMPOSE_FILES --profile production"
    fi

    export COMPOSE_PROJECT_NAME=schemes-${ENVIRONMENT}
}

# Build images
build_images() {
    print_status "Building Docker images for $ENVIRONMENT..."

    # Build backend
    if [ -d "schemes-python-backend" ]; then
        docker build -t schemes-backend:${ENVIRONMENT} schemes-python-backend/
        print_status "Backend image built successfully"
    fi

    # Build frontend
    if [ -d "schemes-react-frontend" ]; then
        docker build -t schemes-frontend:${ENVIRONMENT} schemes-react-frontend/
        print_status "Frontend image built successfully"
    fi
}

# Deploy services
deploy_up() {
    print_status "Starting services for $ENVIRONMENT environment..."

    docker-compose $COMPOSE_FILES up -d

    print_status "Services started successfully"

    # Wait for services to be healthy
    print_status "Waiting for services to be healthy..."
    sleep 10

    # Run database migrations
    print_status "Running database migrations..."
    docker-compose $COMPOSE_FILES exec -T backend alembic upgrade head

    print_status "Deployment completed successfully!"
    print_status "Frontend: http://localhost:3000"
    print_status "Backend API: http://localhost:8000"
    print_status "API Docs: http://localhost:8000/docs"
}

# Stop services
deploy_down() {
    print_status "Stopping services for $ENVIRONMENT environment..."
    docker-compose $COMPOSE_FILES down
    print_status "Services stopped successfully"
}

# Restart services
deploy_restart() {
    print_status "Restarting services for $ENVIRONMENT environment..."
    docker-compose $COMPOSE_FILES restart
    print_status "Services restarted successfully"
}

# Show logs
show_logs() {
    print_status "Showing logs for $ENVIRONMENT environment..."
    docker-compose $COMPOSE_FILES logs -f
}

# Main execution
main() {
    validate_environment
    validate_action
    set_compose_files

    case $ACTION in
        build)
            build_images
            ;;
        up)
            deploy_up
            ;;
        down)
            deploy_down
            ;;
        restart)
            deploy_restart
            ;;
        logs)
            show_logs
            ;;
    esac
}

# Run main function
main "$@"
