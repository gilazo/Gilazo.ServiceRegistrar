FROM microsoft/dotnet:latest
COPY . /app
WORKDIR /app/Gilazo.ServiceRegistrar.Presentation.WebApi
RUN ["dotnet", "restore"]

WORKDIR /app/Gilazo.ServiceRegistrar.Application
RUN ["dotnet", "restore"]

WORKDIR /app/Gilazo.ServiceRegistrar.Infrastructure
RUN ["dotnet", "restore"]

WORKDIR /app/Gilazo.ServiceRegistrar.Presentation.WebApi
RUN ["dotnet", "build"]

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000

ENTRYPOINT ["dotnet", "run"]
