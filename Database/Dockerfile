FROM microsoft/mssql-server-linux:2017-latest
ENV SA_PASSWORD=Zaq123#!
ENV ACCEPT_EULA=Y
COPY Entrypoint.sh Entrypoint.sh
COPY SqlCmdStartup.sh SqlCmdStartup.sh
COPY SqlCmdScript.sql SqlCmdScript.sql
RUN chmod +x ./SqlCmdStartup.sh
CMD /bin/bash ./Entrypoint.sh