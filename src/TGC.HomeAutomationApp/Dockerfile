### STAGE 1:BUILD ###
FROM node:18.19-alpine AS build

WORKDIR /app

COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

COPY . .
RUN npm run build --prod

### STAGE 2:RUN ###
FROM nginx:alpine AS ngi
COPY --from=build /app/wwwroot /usr/share/nginx/html
COPY /nginx.conf /etc/nginx/conf.d/default.conf

CMD ["/bin/sh", "-c", "envsubst < /usr/share/nginx/html/assets/config/dist.runtime.config.json > /usr/share/nginx/html/assets/config/runtime.config.json && exec nginx -g 'daemon off;'"]
