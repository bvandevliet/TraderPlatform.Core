#!/usr/bin/env bash

echo "#############################################"
echo "# Temporary proxy for database access.      #"
echo "#############################################"
echo ""

docker build -t traderplatform-support -f nginx/nginx-support.Dockerfile .
docker run --net traderplatformcore -p 27017:27017 --rm traderplatform-support
