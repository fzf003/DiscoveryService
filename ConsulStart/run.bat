consul.exe agent -server -bootstrap-expect 3  -data-dir .\consul\tmp -node=10.0.84.56 -bind=10.0.84.56 -advertise 10.0.84.56 -ui -client=0.0.0.0 -retry-join=10.6.104.80

