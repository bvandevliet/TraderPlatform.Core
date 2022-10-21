# syntax=docker/dockerfile:1

FROM node:alpine AS build
ENV NODE_ENV=development
# ENV PORT=1337
WORKDIR /usr/src
COPY TraderPlatform.ExpressApp/ .
RUN yarn --${NODE_ENV}
RUN npm run docker-build
RUN cp -R ./src/public/ ./dist/
RUN cp -R ./src/views/ ./dist/

FROM node:alpine AS final
ENV NODE_ENV=production
ENV PORT=1337
WORKDIR /usr/app
COPY TraderPlatform.ExpressApp/package*.json .
RUN yarn --${NODE_ENV}
RUN mkdir ./dist
COPY --from=build /usr/src/dist ./dist
EXPOSE ${PORT}
ENTRYPOINT [ "node", "./dist/app.js" ]