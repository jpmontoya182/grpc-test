using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");

            // ====
            var greeterClient = new Greeter.GreeterClient(channel);
            var client = await greeterClient.SayHelloAsync(new HelloRequest { Name = "Juanes" });
            Console.WriteLine($"Name : {client.Name}, Menssage : {client.Message}");
            Console.WriteLine("======================================");
            // ====
            var customerClient = new Customers.CustomersClient(channel);
            // ====
                var userId = 3;            
                var customer = await customerClient.GetCustomerInfoAsync(new CustomerLookupModel { UserId = userId });
                Console.WriteLine($"It was recover with the UserId {userId}:  {customer.FirstName}, {customer.LastName}");
            // ====
            Console.WriteLine("======================================");
            using (var call = customerClient.GetNewCustomers(new NewCustomerRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentCustomer = call.ResponseStream.Current;
                    Console.WriteLine($"It was recover the user:  {currentCustomer.FirstName}, {currentCustomer.LastName}");
                }
            }
            // ====
            Console.ReadLine();
        }
    }
}
