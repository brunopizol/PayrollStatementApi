# Imagem de compilação
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copiar projeto e restaurar pacotes
COPY FolhaDePagamento.sln .
COPY FolhaDePagamento/*.csproj ./FolhaDePagamento/
COPY FolhaDePagamento.Application/*.csproj ./FolhaDePagamento.Application/
COPY FolhaDePagamento.Domain/*.csproj ./FolhaDePagamento.Domain/
COPY FolhaDePagamento.Infra/*.csproj ./FolhaDePagamento.Infra/
COPY FolhaDePagamento.Presentation/*.csproj ./FolhaDePagamento.Presentation/
RUN dotnet restore FolhaDePagamento.sln

# Copiar todo o código
COPY . .

# Compilar e publicar a aplicação
RUN dotnet publish FolhaDePagamento.sln -c Release -o out

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out ./

# Expor porta
EXPOSE 80

# Comando para iniciar o container
ENTRYPOINT ["dotnet", "FolhaDePagamento.Presentation.dll"]
