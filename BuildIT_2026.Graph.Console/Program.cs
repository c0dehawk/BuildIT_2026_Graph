using BuildIT2026_Graph.Core.Models;
using MockingBird;
using BuildIT2026_Graph.Core.Services;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

while (true)
{
    Console.Clear();
    Console.WriteLine("=== BuildIT Graph Demo ===");
    Console.WriteLine("");
    Console.WriteLine("1. Seed Customer Nodes");
    Console.WriteLine("10. Exit");
    Console.Write("Select an option: ");

    string choice = Console.ReadLine() ?? string.Empty;

    switch (choice)
    {
        case "1":
            SeedCustomerNodes().Wait();
            break;
        case "10":
            Console.WriteLine("Exiting application. Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }

    static async Task SeedCustomerNodes()
    {
        Console.Write("How many records do you want to generate? ");
        var input = Console.ReadLine();

        var numberOfRecords = int.TryParse(input, out int n) ? n : 10;

        MBObject obj = new MBObject();
        obj.AddProperty(new MBNumberProperty("customerId"));
        obj.AddProperty(new MBStringProperty("FirstName").SetFormat(MBStringFormat.FirstName).SetIncludeSpecialCharacters(false));
        obj.AddProperty(new MBStringProperty("LastName").SetFormat(MBStringFormat.LastName).SetIncludeSpecialCharacters(false));
        obj.AddProperty(new MBDateTimeProperty("DateOfBirth"));
        obj.AddProperty(new MBStringProperty("Email").SetFormat(MBStringFormat.Email));

        var sampleRecords = obj.Mock(numberOfRecords); 


        //// Cast to Person
        List<Object> people = JsonConvert.DeserializeObject<List<Object>>(JsonConvert.SerializeObject(sampleRecords)) ?? new List<Object>();

        Console.Write(JsonConvert.SerializeObject(people, Newtonsoft.Json.Formatting.Indented));

    } 

    Console.WriteLine("\nPress Enter to return to the menu...");
    Console.ReadLine();
}
