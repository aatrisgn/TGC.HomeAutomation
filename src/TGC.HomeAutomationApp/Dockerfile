### STAGE 1:BUILD ###
FROM node:18.19-alpine AS build

WORKDIR /dist/src/app

RUN npm cache clean --force

COPY . .
RUN yarn install
RUN npm install -g "@angular/cli@latest"
RUN npm run build --prod

### STAGE 2:RUN ###
FROM nginx:latest AS ngi
COPY --from=build /dist/src/app/wwwroot/assets/config /usr/share/nginx/html/assets/config

COPY --from=build /dist/src/app/wwwroot /usr/share/nginx/html
COPY --from=build /dist/src/app/wwwroot/assets/config/runtime.config.json /usr/share/nginx/html/assets/config/runtime.config.json

COPY /nginx.conf /etc/nginx/conf.d/default.conf

CMD ["/bin/sh",  "-c",  "envsubst < /usr/share/nginx/html/assets/config/dist.runtime.config.json > /usr/share/nginx/html/assets/config/runtime.config.json && exec nginx -g 'daemon off;'"]
