# gRPC Stock Price Example
This is a sample project that demonstrates how to use gRPC services in .NET Core. It consists of two projects: GrpcStockExample and GrpcStockExample.Client. The GrpcStockExample project is the gRPC service, which provides stock prices for a given stock symbol. The GrpcStockExample.Client project is the gRPC client, which allows users to request stock prices by providing a stock symbol.

## Prerequisites
To run this project, you must have the following software installed on your machine:

.NET Core SDK

## Getting Started
To get started with the gRPC Stock Price Example project, follow these steps:

Open a command prompt or terminal window.
Navigate to the GrpcStockExample project directory.
Type dotnet run and press Enter. This will start the gRPC service on http://localhost:5078.
Open a new command prompt or terminal window.
Navigate to the GrpcStockExample.Client project directory.
Type dotnet run and press Enter. This will start the gRPC client and request stock prices for several stock symbols.

## Using the gRPC Client
Once the gRPC client is running, it will request prices for predefined list of stock. There is no input on client side since this is just a demo app.
Note: If the gRPC service is unable to provide a price for a given stock symbol, the client will display an error message indicating that the price could not be found.

## Conclusion
The gRPC Stock Price Example project demonstrates how to use gRPC services in .NET Core to provide stock prices for a given stock symbol. By following the steps outlined in this documentation, you can run the project on your machine and use the gRPC client to request stock prices for various stock symbols.
