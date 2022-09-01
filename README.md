# mirth-version-controll-tool

Console application developed to pull all channels and libraries from a Mirth Connect instance using
Mirth Connect Web API.

Current commands availables:

MIRTHPULL - pulls data from remote NextGen/Mirth server. Saves it to the provided path as xml.
	
	PARAMETERS: [
		--server = remote server address or IP
		--username = valid mirth user
		--password = user's password
		--resource = which resource you want to retrieve - currently supports: channels
		--saveto = path to save the files
	]	

	EX: mirthpull --server https://mirthserver:8443/api --username user --password password --resource channels --saveto C:/backup/channels

COMMIT - commits changes to a github repository.
	
	PARAMETERS: [
		--repo = github destination repo, for private repositories the user needs to be authenticated
		--username = github username
		--sourcefilepath = file fullname/path
		--token = required for private repositories
	]

	EX: commit --repo githubrepo --user malipense --sourcefilepath C:/dev/test/file.xml	--token xWEs1

OUTPUTFILE - writes file to path

HELP - list all the available commands in the current version.

*****************************************************************************************
**TODO: add option to upload/send xml files to CREATE a new channel on the server	   **
**TODO: add option to upload/send xml files to UPDATE an existing channel on the server**
*****************************************************************************************

*HOW TO USE*

Author: Eduardo Malipense
