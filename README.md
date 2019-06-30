# Microservices based sample app using Docker, Kubernetes and .NET Core
Sample cloud native e-commerce application which uses microservices based architecture running on .NET Core and Docker, which can be orchestrated by either Docker Swarm, Kubernetes or Service Fabric. It can be configured to use only local resources (RabbitMQ, SQL Server, Redis) or it can use cloud resources. If using cloud resources, it is using Azure App Configuration and Key Vault to store sensitive information like connection strings. You can run it locally on Docker, Docker Swarm, or Kubernetes, or you can deploy it to Azure and run on either Kubernetes or Service Fabric.

## Assumptions
I assume you have basic understanding on Docker, orchestrations, and Azure. This small sample is only meant to introduce you to how a potential microservice based app might look like.

## What does it do?
The sample backend is composed of 7 microservices, 5 of which are used to achieve one simple scenario. They implement single workflow, user submits his purchase in an online store. Consider below architecture diagram:
![Blah](https://github.com/pwlodek/ECommerce.Microservices/blob/master/Presentation/Architecture.png)

The workflow starts by doing POST to http://localhost:8083/api/orders with the following JSON:

```javascript
{ customerId: 1, items: [ { productId: 1, quantity: 1 }, { productId: 2, quantity: 2 } ]}
```
This creates an order for customer 1, who wishes to purchase 2 items. After order is created, you can inspect/track it by issuing GET to the same microservice (http://localhost:8083/api/orders). The Sales.Api microservice goes to the Catalog.Api microservice to get the actual items for the order, and to Customers.Api microservice to get details about the customers.

Next, an event is sent on the service bus indicating that an order has been created and as such, we can start processing payment (via the Payment.Host microservice) and packing (via the Shipping.Host microservice). Since payment and packing operations are *long running* we have to maintain the state of order using shipping saga. Shipping saga listens for two events, payment completed and order packed. Only after both are received for a particular order, shipping saga sends the order, and notifies Sales.Api service that it has shipped the order. Then, Sales.Api service marks the order as shipped, which concludes the workflow.

## How to run it?
You can launch the sample locally on Docker, on a local Docker Swarm, on local Kubernetes, or on Azure AKS.

### Local Docker environment
Since the backend is composed of multiple microservices, we have to archestrate the entire app. The easies is to use docker compose. First build it, and then run it.

```
docker-compose build
docker-compose up
```

### Azure
First you need to set up Azure infrastructure. I recommend IaaS solution as it give you a lot of insights into the what and how. Unfortunately, at this point ACS does not support Swarm Mode, so I highly recommend community driven ACS-Engine found at https://azure.microsoft.com/en-us/resources/templates/101-acsengine-swarmmode/. Clik on the button "Deploy to Azure" and you will end up with fully configured Docker Swarm cluster. Once you have the cluster up and running, you have to create Azure Container Registry, which will hold all the images. The clusters' nodes will grab the app from there. Assuming your container registry is named *ecommercesampleregistry*, execute the following steps:

* login to the remote registry on your local developer instance: 
```
docker login ecommercesampleregistry.azurecr.io -u ecommercesampleregistry -p <password>
```
* build local images: 
```
docker-compose -f docker-compose.azure.yml build
```
* push local images to the repository in Azure: 
```
docker-compose -f docker-compose.azure.yml push
```
* download yaml onto Docker swarm manager node: 
```
curl -o docker-compose.yml https://raw.githubusercontent.com/pwlodek/ECommerce.Microservices/master/docker-compose.azure.yml
```
* login to the remote registry on your manager and worker nodes: 
```
docker login ecommercesampleregistry.azurecr.io -u ecommercesampleregistry -p <password>
```
* deploy stack on manager node: 
```
docker stack deploy -c docker-compose.yml app1 --with-registry-auth
```
* remember to create load balancing probe and rule for port 8083 (Sales service) so that you can connect from external hosts

At this point you should be able to reach Sales.Api microservice to execute the workflow discussed above. Congratulations! Your first microservice based backend is deployed!

## Wrapping up
At this point you have the sample backend up and running but maybe you asked a question, why do I have to connect to my services via a port? The answer is simple. Each service wants to expose port 80, and there can be multiple services  running on the same node. Hence, each service has to map it's internal port 80 to a well-known port on the node. Which in turn means each service type needs its unique port. If you don't like that, you can introduce a layer of *gateway* microservices that mape their internal port 80 to external 80, and they act as the frontline for accepting requests, and relying them to proper microservices. This way, your internal microservice implementation is hidden from the world.
