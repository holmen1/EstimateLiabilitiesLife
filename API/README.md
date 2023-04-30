# PensionAPI

* Test the API on Azure

```bash
curl 'https://pensionapi.azurewebsites.net'
```

```bash
curl -X 'POST' 'https://pensionapi.azurewebsites.net/reserves' -H 'accept: */*' -H 'Content-Type: application/json' -d '{"contractNo":42341,"valueDate":"2023-04-30","birthDate":"1973-04-30","sex":"F","z":65,"guarantee":1000,"payPeriod":5,"table":"APG"}'
{"contractNo":42341,"valueDate":"2023-04-30T00:00:00","pvTechnicalProvision":520.2712805436308}
```

* localhost, without SSL

```bash
curl --insecure 'https://localhost:7178'
```