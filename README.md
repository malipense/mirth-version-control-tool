# mirth-version-controll-tool

Console application developed to pull all channels and libraries from a Mirth Connect instance using
Mirth Connect Web API.

A "remote" folder contaning all the channels separated by group and Libraries w/ CodeTemplates will be created on the project folder when you execute the tool.

TODO: add option to upload/send xml files to CREATE a new channel on the server
TODO: add option to upload/send xml files to UPDATE an existing channel on the server


*HOW TO USE*

1 - Open the solution on Visual Studio, go to -> Build -> Build Solution
2 - Open bin folder on the project folder
3 - Open the command line
4 - run mirth-control-tool.exe -server "https://url:8443/api" -username "user" -password "pass" pull


Author: Eduardo Malipense