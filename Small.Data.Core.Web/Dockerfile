FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

#RUN dotnet ef migrations remove
#RUN dotnet ef migrations add InitialCreate
#RUN dotnet ef database update

# Copy everything else and build
COPY . ./
RUN dotnet ef migrations remove
RUN dotnet ef migrations add InitialCreate
RUN dotnet ef database update

RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Small.Data.Core.Web.dll"]
