#!/usr/bin/env bash
CONFIGURATION=${1:-Release}
RUNTIME=${2:-win-x64}
ROOT_DIR=$(dirname "$0")
set -e

echo "Publishing AuthDesk project..."
PUBLISH_DIR="$ROOT_DIR/artifacts/publish"
rm -rf "$PUBLISH_DIR"
dotnet publish "$ROOT_DIR/AuthDesk/AuthDesk.csproj" -c $CONFIGURATION -r $RUNTIME --self-contained false -o "$PUBLISH_DIR"

echo "Preparing package folder..."
PACKAGE_DIR="$ROOT_DIR/AuthDesk.Packaging.Net/Package"
rm -rf "$PACKAGE_DIR"
mkdir -p "$PACKAGE_DIR"

echo "Copying published outputs to package folder..."
cp -r "$PUBLISH_DIR/"* "$PACKAGE_DIR/"

echo "Package created at: $PACKAGE_DIR"