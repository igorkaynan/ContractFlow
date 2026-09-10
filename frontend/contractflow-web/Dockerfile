FROM node:24-alpine AS build
WORKDIR /app
COPY frontend/contractflow-web/package*.json ./
RUN npm ci
COPY frontend/contractflow-web/ ./
RUN npm run build
FROM nginx:alpine
COPY --from=build /app/dist/contractflow-web/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
