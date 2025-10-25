#!/bin/bash
set -e

echo "================================================="
echo "=== Iniciando Buber Dinner Application ==="
echo "================================================="

MAX_RETRIES=30
RETRY_COUNT=0

until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$DB_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  RETRY_COUNT=$((RETRY_COUNT+1))
  if [ $RETRY_COUNT -ge $MAX_RETRIES ]; then
    echo "❌ ERRO: SQL Server não respondeu após $MAX_RETRIES tentativas"
    exit 1
  fi
  echo "⏳ SQL Server ainda não está pronto... tentativa $RETRY_COUNT/$MAX_RETRIES"
  sleep 2
done

echo "✅ SQL Server está disponível e respondendo!"

# Instala o dotnet-ef globalmente se ainda não estiver instalado
echo ""
echo "🔧 Verificando instalação do Entity Framework Core Tools..."
if ! command -v dotnet-ef &> /dev/null; then
    echo "📦 Instalando dotnet-ef..."
    dotnet tool install --global dotnet-ef --version 6.0.*
    export PATH="$PATH:/root/.dotnet/tools"
else
    echo "✅ dotnet-ef já está instalado"
fi

# Aplica as migrations
echo ""
echo "🔄 Aplicando migrations do Entity Framework..."
cd /app

# Tenta aplicar as migrations
if dotnet ef database update --no-build --verbose; then
    echo "✅ Migrations aplicadas com sucesso!"
else
    echo "⚠️  Aviso: Falha ao aplicar migrations via CLI"
    echo "ℹ️  A aplicação tentará aplicar via código..."
fi

# Inicia a aplicação
echo ""
echo "🚀 Iniciando aplicação Buber Dinner..."
echo "================================================="
exec dotnet BuberDinner.Api.dll