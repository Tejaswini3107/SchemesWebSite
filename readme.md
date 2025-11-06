# Schemes Management System

A comprehensive web application for managing government schemes with customer authentication, admin panel, and multilingual support.

## Architecture

- **Backend**: Python FastAPI with SQLAlchemy ORM
- **Frontend**: React with TypeScript
- **Database**: PostgreSQL
- **Cache**: Redis
- **Deployment**: Docker + Docker Compose
- **CI/CD**: Jenkins + GitLab CI/CD

## Project Structure

```
schemes-website/
├── schemes-python-backend/     # FastAPI backend
├── schemes-react-frontend/     # React frontend
├── docker/                     # Docker configurations
├── jenkins/                    # Jenkins pipeline
├── nginx/                      # Nginx configuration
├── db/                         # Database initialization
├── docker-compose.yml          # Main compose file
└── .gitlab-ci.yml             # GitLab CI/CD pipeline
```

## Quick Start (Development)

### Prerequisites

- Docker and Docker Compose
- Git

### Setup

1. **Clone the repository**
   ```bash
   git clone https://gitlab.com/your-org/schemes-website.git
   cd schemes-website
   ```

2. **Create environment file**
   ```bash
   cp .env.dev .env
   # Edit .env with your configuration
   ```

3. **Start services**
   ```bash
   docker-compose -f docker-compose.yml -f docker/docker-compose.dev.yml up -d
   ```

4. **Run database migrations**
   ```bash
   docker-compose -f docker-compose.yml -f docker/docker-compose.dev.yml exec backend alembic upgrade head
   ```

5. **Access the application**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:8000
   - API Docs: http://localhost:8000/docs

## Environment Setup

### Development
```bash
docker-compose -f docker-compose.yml -f docker/docker-compose.dev.yml up -d
```

### QA
```bash
docker-compose -f docker-compose.yml -f docker/docker-compose.qa.yml up -d
```

### UAT
```bash
docker-compose -f docker-compose.yml -f docker/docker-compose.uat.yml up -d
```

### Production
```bash
docker-compose -f docker-compose.yml -f docker/docker-compose.prod.yml --profile production up -d
```

## Git Branch Strategy

- `main` (protected): Production releases
- `release/dev`: Development branch
- `qa`: QA environment
- `uat`: UAT environment
- Feature branches: `feature/*`

## CI/CD Pipeline

### Jenkins Setup

1. Install required plugins:
   - GitLab Plugin
   - Docker Pipeline
   - Slack Notification
   - JUnit
   - Cobertura

2. Configure credentials:
   - GitLab credentials
   - Docker registry credentials
   - Slack webhook

3. Create pipeline job using `jenkins/Jenkinsfile`

### GitLab CI/CD

The `.gitlab-ci.yml` file contains the complete CI/CD pipeline for GitLab.

## Deployment

### Manual Deployment

```bash
# For development
export COMPOSE_PROJECT_NAME=schemes-dev
docker-compose -f docker-compose.yml -f docker/docker-compose.dev.yml up -d

# For production
export COMPOSE_PROJECT_NAME=schemes-prod
docker-compose -f docker-compose.yml -f docker/docker-compose.prod.yml --profile production up -d
```

### Automated Deployment

- **Jenkins**: Use the provided Jenkinsfile
- **GitLab CI/CD**: Automatic deployment on merge to respective branches

## Environment Variables

See `.env.example` for all required environment variables.

### Key Variables

- `DATABASE_URL`: PostgreSQL connection string
- `SECRET_KEY`: JWT secret key
- `REDIS_URL`: Redis connection string
- `SMTP_*`: Email configuration
- `TWILIO_*`: SMS configuration

## Database

### Migrations

```bash
# Create new migration
alembic revision --autogenerate -m "migration message"

# Apply migrations
alembic upgrade head

# Rollback
alembic downgrade -1
```

## API Documentation

- Swagger UI: http://localhost:8000/docs
- ReDoc: http://localhost:8000/redoc

## Testing

### Backend Tests

```bash
cd schemes-python-backend
python -m pytest tests/ -v --cov=app
```

### Frontend Tests

```bash
cd schemes-react-frontend
npm test
```

## Security

- JWT authentication
- Password hashing with bcrypt
- CORS configuration
- Rate limiting
- Security headers
- SSL/TLS encryption

## Monitoring

- Health checks for all services
- Docker health checks
- Application logging
- Error tracking

## Backup and Recovery

### Database Backup

```bash
# Create backup
docker exec schemes-db pg_dump -U schemes_user schemes_db > backup.sql

# Restore backup
docker exec -i schemes-db psql -U schemes_user schemes_db < backup.sql
```

## Troubleshooting

### Common Issues

1. **Port conflicts**: Change ports in docker-compose files
2. **Database connection**: Check DATABASE_URL in environment files
3. **Permission issues**: Ensure proper file permissions for Docker volumes

### Logs

```bash
# View all logs
docker-compose logs

# View specific service logs
docker-compose logs backend
docker-compose logs frontend
docker-compose logs db
```

## Contributing

1. Create feature branch from `release/dev`
2. Make changes and test
3. Create merge request to appropriate environment branch
4. Wait for approval and deployment

## License

[Your License Here]
