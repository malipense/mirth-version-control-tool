# mirth-version-control-tool

Console application developed to pull all channels and libraries from a Mirth Connect instance using
Mirth Connect Web API.

Current commands availables:

PULLCHANNELS - pulls data from remote NextGen/Mirth server. Saves it to the provided path as xml.
	
	PARAMETERS: [
		--server = remote server name or IP
		--username = valid mirth user
		--password = user's password
		--path = C:/backup/channels
	]	

	EX: pullchannels --server https://mirthserver:8443/api --username user --password password --path C:/backup/channels

PULLLIB - pulls libraries and code templates.

	PARAMETERS: [
		--server = remote server name or IP
		--username = valid mirth user
		--password = user's password
		--path = C:/backup/codetemplates
	]	

	EX: pulllib --server https://mirthserver:8443/api --username user --password password --path C:/backup/channels

HELP - list available commands.
EXIT - exit application.

Author: Eduardo Malipense
