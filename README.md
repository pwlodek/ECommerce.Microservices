# Microservices based sample app using Docker and .NET Core
Sample microservices based e-commerce backend running on .NET Core and Docker. You can run it locally on Docker, Docker Swarm, or you can deploy Docker Swarm Cluster to Azure and run it in the cloud.

## What does it do?
The sample backend is composed of 7 microservices, 5 of which are used to achieve one simple scenario. They implement single workflow, user submits his purchase in a online store. Consider below architecture diagram:
![Blah](https://github.com/pwlodek/ECommerce.Microservices/blob/master/Presentation/Architecture.png)

The workflow starts by doing POST to http://localhost:8083/api/orders with the following JSON:

```javascript
{ customerId: 1, items: [ { productId: 1, quantity: 1 }, { productId: 2, quantity: 2 } ]}
```
This creates an order for customer 1, who wishes to purchase 2 items. Now you can inspect/track the order by issuing GET to the same microservice (http://localhost:8083/api/orders). The Sales.Api microservice goes to the Catalog.Api microservice to get the actual items for the order, and to Customers.Api microservice to get details about the customers.

Next, an event is sent on the service bus indicating that an order has been created and as such, we can start processing payment (via the Payment.Host microservice) and packing (via the Shipping.Host microservice). Since payment and packing operations are *long running* we have to maintain the state of order using shipping saga. Shipping saga listens for two events, payment completed and order packed. Only after both are received for a particular order, shipping saga sends the order, and notifies Sales.Api service that it has shipped the order. Then, Sales.Api service marks the order as shipped, which concludes the workflow.

## How to run it?
Since it is a backend composed of multiple microservices, we have to archestrate the entire app. The easies is to use docker compose. First build it, and then run it.

```
docker-compose build
docker-compose up
```
