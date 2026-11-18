FROM flyway/flyway:13.12.2-alpine

WORKDIR /workspace

# Build context is the repository root.
COPY schema/fxfer-db schema/fxfer-db

ENTRYPOINT ["flyway"]
CMD ["-configFiles=schema/fxfer-db/flyway.conf", "validate"]
