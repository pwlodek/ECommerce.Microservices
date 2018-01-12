#wait for the SQL Server to start up
sleep 15
#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Zaq123#! -d master -i SqlCmdScript.sql