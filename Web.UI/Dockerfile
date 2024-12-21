FROM node:18-alpine
EXPOSE 4200

RUN npm install -g @angular/cli

WORKDIR /usr/src/app
COPY package*.json ./
RUN npm install
COPY . .

ENV NG_CLI_ANALYTICS=false
CMD ["ng", "serve", "--host", "0.0.0.0"]
