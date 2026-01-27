# ===== Build stage =====
FROM eclipse-temurin:25.0.1_8-jdk AS build

WORKDIR /app

# Install sbt
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl gnupg && \
    curl -sL "https://keyserver.ubuntu.com/pks/lookup?op=get&search=0x2EE0EA64E40A89B84B2DF73499E82A75642AC823" | \
    gpg --dearmor | tee /usr/share/keyrings/sbt-archive-keyring.gpg > /dev/null && \
    echo "deb [signed-by=/usr/share/keyrings/sbt-archive-keyring.gpg] https://repo.scala-sbt.org/scalasbt/debian all main" | \
    tee /etc/apt/sources.list.d/sbt.list \
    apt-get update && \
    apt-get install -y --no-install-recommends sbt && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Cache dependencies
COPY build.sbt .
COPY project/ project/
RUN sbt update

# Build
COPY src/ src/
COPY .scalafmt.conf .
RUN sbt assembly

# ===== Prepare necessary prebuilt binaries =====
FROM debian:trixie-slim AS prebuilt

RUN apt-get update && \
    apt-get install -y --no-install-recommends tini && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# ===== Runtime stage =====
FROM gcr.io/distroless/java25-debian13:nonroot

WORKDIR /app

COPY --from=build /app/target/scala-3.7.4/dape.jar /app/dape.jar
COPY --from=prebuilt /usr/bin/tini /usr/bin/tini

ENV JAVA_OPTS="-Xmx512m"

ENTRYPOINT ["/usr/bin/tini", "--", "java", "-jar", "/app/dape.jar"]
