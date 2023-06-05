# EstimateLiabilitiesLife.API

## Dockerize API

```bash
docker build -t "mats-api" .  
docker run -it --rm -p 5000:80 --name aspnetcore_sample mats-api
```

```bash
curl -X 'POST' 'http://localhost:5000/reserve' -H 'accept: */*' -H 'Content-Type: application/json' -d '{"contractNo":42341,"valueDate":"2023-04-30","birthDate":"1973-04-30","sex":"F","z":65,"guarantee":1000,"payPeriod":5,"table":"APG"}'  
{"contractNo":42341,"valueDate":"2023-04-30T00:00:00","pvTechnicalProvision":520.2712805436308}
```

## Deploy to Azure Container Instances

```bash
RESOURCE_GROUP="actuarial-apps-rg"
LOCATION="northeurope"
API_NAME="estimate-liabilities-api"
GITHUB_USERNAME="holmen1"
ACI_NAME="acilife"$GITHUB_USERNAME
```

```bash
docker build --tag $GITHUB_USERNAME/$API_NAME .  
docker push $GITHUB_USERNAME/$API_NAME
```

Create a resource group
```bash
az group create --name $RESOURCE_GROUP --location $LOCATION
```

Create a container
```bash
az container create \
  --name $ACI_NAME \
  --resource-group $RESOURCE_GROUP \
  --image $GITHUB_USERNAME/$API_NAME \
  --ports 80 \
  --dns-name-label $ACI_NAME
```

```bash
az container show --resource-group $RESOURCE_GROUP --name $ACI_NAME --query "{FQDN:ipAddress.fqdn,ProvisioningState:provisioningState}" --out table
````
FQDN  ProvisioningState  
aciholmen1.northeurope.azurecontainer.io  Succeeded

```bash
curl -X 'POST' 'acilifeholmen1.northeurope.azurecontainer.io/reserve' -H 'accept: */*' -H 'Content-Type: application/json' -d '{"contractNo":42341,"valueDate":"2023-04-30","birthDate":"1973-04-30","sex":"F","z":65,"guarantee":1000,"payPeriod":5,"table":"APG"}' 
```

Clean up resources
```bash
az group delete --name $RESOURCE_GROUP
```
