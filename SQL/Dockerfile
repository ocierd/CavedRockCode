FROM mcr.microsoft.com/mssql/server

ARG PROJECT_DIR=/tmp/devdatabase
RUN mkdir -p $PROJECT_DIR
WORKDIR $PROJECT_DIR
COPY SQL/InitializeDatabase.sql ./
COPY SQL/wait-for-it.sh ./
COPY SQL/entrypoint.sh ./
COPY SQL/setup.sh ./

CMD ["/bin/bash","entrypoint.sh"]