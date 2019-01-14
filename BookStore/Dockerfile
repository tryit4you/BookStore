FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 63774
EXPOSE 44382

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY BookStore/BookStore.csproj BookStore/
RUN dotnet restore BookStore/BookStore.csproj
COPY . .
WORKDIR /src/BookStore
RUN dotnet build BookStore.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BookStore.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookStore.dll"]
