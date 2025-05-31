server {
    listen              443 ssl;
    server_name         homeautomation.dev.tgcportal.com;
    ssl_certificate     /etc/nginx/ssl/fullchain.pem;
    ssl_certificate_key /etc/nginx/ssl/privkey.pem;

    location / {
        proxy_pass http://localhost:9001;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }
}

server {
    listen       80;
    server_name  homeautomation.dev.tgcportal.com;

    location / {
        return 301 https://$host$request_uri;
    }
}

server {
    listen 80;
    server_name api.homeautomation.dev.tgcportal.com;

    location / {
        proxy_pass http://localhost:9000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    }
}