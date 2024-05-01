FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

EXPOSE 8080
EXPOSE 8081

WORKDIR /src

# copy csproj and restore as distinct layers
COPY ConsultorioUI.csproj .
RUN dotnet restore ConsultorioUI.csproj

COPY . .
RUN dotnet build ConsultorioUI.csproj -c Release -o /app/build

FROM build as publish
RUN dotnet publish ConsultorioUI.csproj -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf