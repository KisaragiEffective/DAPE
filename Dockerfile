# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
# csproj だけコピーして先に restore
COPY ./KisaragiTech.Dape/*.csproj ./KisaragiTech.Dape/
RUN dotnet restore ./KisaragiTech.Dape/KisaragiTech.Dape.csproj

# StyleCop をサイレンス
COPY ./.editorconfig ./.editorconfig
# 残りのソースをコピー
COPY ./KisaragiTech.Dape ./KisaragiTech.Dape

# ビルド & パック
RUN dotnet publish ./KisaragiTech.Dape/KisaragiTech.Dape.csproj \
    -c Release -o /app/publish \
    -p:UseAppHost=false

# ===== Prepare necessary prebuilt binaries ===
FROM debian:bookworm-slim AS prebuilt

RUN apt-get update && \
    apt-get install -y --no-install-recommends tini && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
    
# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra

WORKDIR /app
COPY --from=build /app/publish .
COPY --from=prebuilt /usr/bin/tini /usr/bin/tini

# 環境変数（必要に応じて書き換え）
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# ArcadeDB / Redis は外部でリンクする前提

ENTRYPOINT ["/usr/bin/tini", "--", "dotnet", "KisaragiTech.Dape.dll"]
