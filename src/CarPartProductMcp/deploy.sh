#!/usr/bin/env bash
set -euo pipefail

# ============================
# Configurações
# ============================
SUBSCRIPTION_ID="6ac2e820-f9ff-4ca6-8db1-b81246d1aca9"
RG="rg-ai-training-day-maf"                  # Resource Group existente
APP="app-cars-part-product-mcp-ai-training-day-maf"         # Web App existente
PUB_DIR="./publish"
ZIP_FILE="app.zip"

# ============================
# Login e subscription
# ============================
az account set --subscription "$SUBSCRIPTION_ID" || { echo "Faça 'az login' primeiro."; exit 1; }

# ============================
# Build & Package
# ============================
echo "==> Restaurando, buildando e publicando (Release)"
dotnet restore
dotnet publish -c Release -o "$PUB_DIR"

echo "==> Empacotando em $ZIP_FILE"
rm -f "$ZIP_FILE"
( cd "$PUB_DIR" && zip -qr "../$ZIP_FILE" . )

# ============================
# Deploy
# ============================
echo "==> Fazendo Zip Deploy para $APP"
az webapp deploy -g "$RG" -n "$APP" --type zip --src-path "$ZIP_FILE"

URL="https://$APP.azurewebsites.net"
echo "==> Deploy concluído!"
echo "==> Acesse: $URL"
