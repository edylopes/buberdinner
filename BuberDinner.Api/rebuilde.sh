#!/usr/bin/env bash
set -e

# Nome amigável do projeto (só pra log)
PROJECT_NAME="BuberDinner"

# Cores pro terminal
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # sem cor

echo -e "${YELLOW}🧹 Stopping and cleaning old containers for $PROJECT_NAME...${NC}"
docker compose down -v || true

echo -e "${YELLOW}🧱 Rebuilding Docker images...${NC}"
docker compose build --no-cache

echo -e "${YELLOW}🚀 Starting containers...${NC}"
docker compose up -d

echo -e "${GREEN}✅ $PROJECT_NAME environment rebuilt successfully!${NC}"
echo -e "${GREEN}📦 Containers running:${NC}"
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

echo -e "${YELLOW}📋 Docker volumes currently in use:${NC}"
docker volume ls
