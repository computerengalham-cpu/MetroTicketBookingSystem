using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MetroTicketSystem.Data;
using MetroTicketSystem.Models;

namespace MetroTicketSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Reset database to ensure a clean slate during testing
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                // 1. Seed Sample Data
                SeedData(context);

                // 2. Execute Required LINQ Queries
                ExecuteRequiredQueries(context);

                // 3. Execute Bonus Tasks
                ExecuteBonusQueries(context);
            }
        }

        private static void SeedData(AppDbContext context)
        {
            Console.WriteLine("--- Seeding Sample Data ---");

            // Add 2 Stations
            var s1 = new Station { Name = "Ramses", Location = "Cairo Downtown" };
            var s2 = new Station { Name = "Maadi", Location = "South Cairo" };
            context.Stations.AddRange(s1, s2);

            // Add 2 Trains
            var t1 = new Train { Number = "T1", Capacity = 300 };
            var t2 = new Train { Number = "T2", Capacity = 450 };
            context.Trains.AddRange(t1, t2);

            // Save changes to generate database IDs for relationship mapping
            context.SaveChanges();

            // Add 5 Tickets (Explicitly linked to Train and Station objects)
            var tickets = new[]
            {
                new Ticket { PassengerName = "Ahmed", Price = 25.00m, TravelDate = DateTime.Now, Train = t1, Station = s1 },
                new Ticket { PassengerName = "Sara", Price = 15.50m, TravelDate = DateTime.Now, Train = t1, Station = s2 },
                new Ticket { PassengerName = "John", Price = 30.00m, TravelDate = DateTime.Now, Train = t2, Station = s1 },
                new Ticket { PassengerName = "Mona", Price = 10.00m, TravelDate = DateTime.Now, Train = t2, Station = s2 },
                new Ticket { PassengerName = "Ahmed", Price = 22.00m, TravelDate = DateTime.Now, Train = t2, Station = s1 }
            };

            context.Tickets.AddRange(tickets);
            context.SaveChanges(); // Commit tickets to the database

            Console.WriteLine("Database seeded successfully!\n");
        }

        private static void ExecuteRequiredQueries(AppDbContext context)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("        REQUIRED LINQ QUERIES           ");
            Console.WriteLine("========================================");

            // 1. Get all tickets & Load Related Data using Include()
            Console.WriteLine("\n[1] All Tickets (With Eager Loading):");
            var allTickets = context.Tickets
                                    .Include(t => t.Train)
                                    .Include(t => t.Station)
                                    .ToList();

            foreach (var t in allTickets)
            {
                Console.WriteLine($"Passenger: {t.PassengerName} | Train: {t.Train.Number} | Station: {t.Station.Name} | Price: {t.Price}");
            }

            // 2. Get tickets with price greater than 20
            Console.WriteLine("\n[2] Tickets with Price > 20:");
            var expensiveTickets = context.Tickets.Where(t => t.Price > 20).ToList();
            foreach (var t in expensiveTickets)
            {
                Console.WriteLine($"Passenger: {t.PassengerName} - Price: {t.Price}");
            }

            // 3. Get the first ticket for a specific passenger ("Ahmed")
            Console.WriteLine("\n[3] First ticket for passenger 'Ahmed':");
            var firstAhmedTicket = context.Tickets.FirstOrDefault(t => t.PassengerName == "Ahmed");
            if (firstAhmedTicket != null)
            {
                Console.WriteLine($"Found: {firstAhmedTicket.PassengerName} - Price: {firstAhmedTicket.Price}");
            }

            // 4. Count all tickets
            Console.WriteLine("\n[4] Total Ticket Count:");
            int totalTickets = context.Tickets.Count();
            Console.WriteLine($"Total: {totalTickets}");

            // 5. Order tickets by price descending
            Console.WriteLine("\n[5] Tickets ordered by price descending:");
            var orderedTickets = context.Tickets.OrderByDescending(t => t.Price).ToList();
            foreach (var t in orderedTickets)
            {
                Console.WriteLine($"Passenger: {t.PassengerName} - Price: {t.Price}");
            }
        }

        private static void ExecuteBonusQueries(AppDbContext context)
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine("            BONUS TASKS                 ");
            Console.WriteLine("========================================");

            // 1. Get the number of tickets for each train
            Console.WriteLine("\n[Bonus 1] Number of tickets per train:");
            var trainTicketCounts = context.Trains
                                           .Select(t => new { t.Number, TicketCount = t.Tickets.Count })
                                           .ToList();

            foreach (var item in trainTicketCounts)
            {
                Console.WriteLine($"Train: {item.Number} has {item.TicketCount} ticket(s).");
            }

            // 2. Get the cheapest ticket
            Console.WriteLine("\n[Bonus 2] Cheapest Ticket:");
            var cheapestTicket = context.Tickets.OrderBy(t => t.Price).FirstOrDefault();
            if (cheapestTicket != null)
            {
                Console.WriteLine($"Passenger: {cheapestTicket.PassengerName} - Price: {cheapestTicket.Price}");
            }

            // 3. Get the most expensive ticket
            Console.WriteLine("\n[Bonus 3] Most Expensive Ticket:");
            var mostExpensiveTicket = context.Tickets.OrderByDescending(t => t.Price).FirstOrDefault();
            if (mostExpensiveTicket != null)
            {
                Console.WriteLine($"Passenger: {mostExpensiveTicket.PassengerName} - Price: {mostExpensiveTicket.Price}");
            }
        }
    }
}