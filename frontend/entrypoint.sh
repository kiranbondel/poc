#!/bin/sh
echo "Setting REACT_APP_API_BASE_URL"
sed -i "s|\"http://localhost:5000\"|\"$REACT_APP_API_BASE_URL\"|g" /usr/share/nginx/html/runtime-config.tsx
exec "$@"
