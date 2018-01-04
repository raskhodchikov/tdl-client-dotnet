echo Starting WireMock process #1
python wiremock/fetch-wiremock-and-run.py start 41375
echo Started WireMock process #1

echo Starting WireMock process #2
python wiremock/fetch-wiremock-and-run.py start 8222
echo Started WireMock process #2

echo Starting broker process
python broker/activemq-wrapper.py start
echo Started broker process

nunit3-console test/specs/bin/Release/TDL.Test.Specs.dll

python wiremock/fetch-wiremock-and-run.py stop 41375
python wiremock/fetch-wiremock-and-run.py stop 8222
python broker/activemq-wrapper.py stop
