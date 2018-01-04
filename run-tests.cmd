python fetch-wiremock-and-run.py start 41375
python fetch-wiremock-and-run.py start 8222
python broker/activemq-wrapper.py start

nunit3-console test/specs/bin/Release/TDL.Test.Specs.dll

python fetch-wiremock-and-run.py stop 41375
python fetch-wiremock-and-run.py stop 8222
python broker/activemq-wrapper.py stop
