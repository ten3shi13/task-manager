FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_BUNDLE_EXTRACT_BASE_DIR=/tmp/.net
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG EF_VERSION=9.0.18
WORKDIR /src
COPY ["TaskManagerMediatR.API/TaskManagerMediatR.API.csproj", "TaskManagerMediatR.API/"]
COPY ["TaskManagerMediatR.Application/TaskManagerMediatR.Application.csproj", "TaskManagerMediatR.Application/"]
COPY ["TaskManagerMediatR.Contracts/TaskManagerMediatR.Contracts.csproj", "TaskManagerMediatR.Contracts/"]
COPY ["TaskManagerMediatR.Domain/TaskManagerMediatR.Domain.csproj", "TaskManagerMediatR.Domain/"]
COPY ["TaskManagerMediatR.Infrastructure/TaskManagerMediatR.Infrastructure.csproj", "TaskManagerMediatR.Infrastructure/"]
RUN dotnet restore "TaskManagerMediatR.API/TaskManagerMediatR.API.csproj"
COPY . .
WORKDIR "/src/TaskManagerMediatR.API"
RUN dotnet publish "TaskManagerMediatR.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false
RUN dotnet tool install --global dotnet-ef --version $EF_VERSION
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /src
RUN dotnet ef migrations bundle \
    --project TaskManagerMediatR.Infrastructure \
    --startup-project TaskManagerMediatR.API \
    --configuration $BUILD_CONFIGURATION \
    --self-contained \
    --runtime linux-x64 \
    --output /app/publish/efbundle \
    --force


FROM base AS final
WORKDIR /app
COPY --from=build --chown=$APP_UID:$APP_UID /app/publish .
COPY --chown=$APP_UID:$APP_UID entrypoint.sh /app/entrypoint.sh
USER root
RUN chmod +x /app/entrypoint.sh \
    && mkdir -p /tmp/.net \
    && chown -R $APP_UID:$APP_UID /tmp/.net /app
USER $APP_UID

ENTRYPOINT ["/app/entrypoint.sh"]
