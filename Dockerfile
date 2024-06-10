ARG REPO=mcr.microsoft.com/dotnet
ARG VERSION=8.0-alpine3.18

FROM $REPO/sdk:$VERSION AS build
WORKDIR /sln
COPY . .
RUN dotnet publish src/TaskManagement.Bootstrap/TaskManagement.Bootstrap.csproj -c Release --os linux -o /app

FROM $REPO/aspnet:$VERSION
LABEL maintainer kamileo24219@gmail.com
RUN apk add tzdata \
 && cp /usr/share/zoneinfo/Europe/Warsaw /etc/localtime \
 && echo "Europe/Warsaw" > /etc/timezone \
 && apk del tzdata \
 && apk add --no-cache icu-libs icu-data-full \
 && mkdir /app \
 && chown -R app /app
USER app
WORKDIR /app
EXPOSE 8000
ENV ASPNETCORE_URLS="http://*:8000" DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TaskManagement.Bootstrap.dll"]