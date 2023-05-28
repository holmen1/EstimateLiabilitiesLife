# EstimateLiabilitiesLife.API

* Dockerize API

```bash
docker build -t "mats-api" .  
docker run -it --rm -p 5000:80 --name aspnetcore_sample mats-api
```

```bash
curl -X 'POST' 'http://localhost:5000/reserves' -H 'accept: */*' -H 'Content-Type: application/json' -d '{"contractNo":42341,"valueDate":"2023-04-30","birthDate":"1973-04-30","sex":"F","z":65,"guarantee":1000,"payPeriod":5,"table":"APG"}'  
{"contractNo":42341,"valueDate":"2023-04-30T00:00:00","pvTechnicalProvision":520.2712805436308}
```
