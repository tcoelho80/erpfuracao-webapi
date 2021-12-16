#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ERP.Furacao.WebApi/ERP.Furacao.WebApi.csproj", "src/ERP.Furacao.WebApi/"]
COPY ["src/ERP.Furacao.Infrastructure.Data/ERP.Furacao.Infrastructure.Data.csproj", "src/ERP.Furacao.Infrastructure.Data/"]
COPY ["src/ERP.Furacao.Domain/ERP.Furacao.Domain.csproj", "src/ERP.Furacao.Domain/"]
COPY ["src/ERP.Furacao.Application/ERP.Furacao.Application.csproj", "src/ERP.Furacao.Application/"]
COPY ["src/ERP.Furacao.Infrastructure.Crosscutting/ERP.Furacao.Infrastructure.Crosscutting.csproj", "src/ERP.Furacao.Infrastructure.Crosscutting/"]
COPY ["src/ERP.Furacao.Infrastructure.Identity/ERP.Furacao.Infrastructure.Identity.csproj", "src/ERP.Furacao.Infrastructure.Identity/"]
RUN dotnet restore "src/ERP.Furacao.WebApi/ERP.Furacao.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ERP.Furacao.WebApi"
RUN dotnet build "ERP.Furacao.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.Furacao.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.Furacao.WebApi.dll"]