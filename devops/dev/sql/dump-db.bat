mysqldump -u main -h 104.225.208.163 -p ^
--databases Deadit_Dev ^
--column-statistics=FALSE ^
--routines ^
--events ^
--triggers ^
--add-drop-table ^
--allow-keywords ^
--no-create-db ^
--no-data ^
--result-file "C:\xampp\htdocs\files\deadit\sql\ddl\.schemas.sql"


mysqldump -u main -h 104.225.208.163 -p ^
--column-statistics=FALSE ^
--allow-keywords ^
--no-create-db ^
--no-create-info ^
--replace ^
--order-by-primary ^
--result-file "C:\xampp\htdocs\files\deadit\sql\ddl\.data.sql" ^
Deadit_Dev Vote_Type Post_Type Error_Message Banned_Community_Name Error_Message_Group


@echo off
cd "C:\xampp\htdocs\files\deadit\sql\ddl"

type ".schemas.sql" ".data.sql" > "dump.sql"

del ".schemas.sql" /Q
del ".data.sql" /Q

pause
