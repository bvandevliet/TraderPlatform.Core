FROM nginx:alpine AS traderplatform.nginx
COPY nginx.conf /etc/nginx/nginx.conf

# https://certbot.eff.org/instructions?ws=nginx&os=pip
# problem is `certbot --nginx` requires additional user sh input
# 
# Install system dependencies
# RUN apk -U add \
#     python3 \
#     python3-dev \
#     py3-pip \
#     augeas