using _007_OptimisticConcurrencyControl.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace _007_OptimisticConcurrencyControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");
            InitialBalance();

            //Creating Threads
            Thread t1 = new Thread(UpdateBalance)
            {
                Name = "Thread1"
            };
            Thread t2 = new Thread(UpdateBalance)
            {
                Name = "Thread2"
            };
            Thread t3 = new Thread(UpdateBalance)
            {
                Name = "Thread3"
            };
            Thread t4 = new Thread(UpdateBalance)
            {
                Name = "Thread4"
            };
            Thread t5 = new Thread(UpdateBalance)
            {
                Name = "Thread5"
            };
            //Executing the methods
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();

            ShowBalance();

            Console.WriteLine("Main Thread Ended");
            Console.Read();
        }

        static void InitialBalance()
        {
            using (var dbContext = new BankDbContext())
            {
                var account = dbContext.Accounts
                    .Where(x => x.Id == 1)
                    .FirstOrDefault();

                if (account == null)
                {
                    dbContext.Database.OpenConnection();
                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Accounts ON;");

                    dbContext.Add(new Account { Id = 1, Balance = 0 });
                    dbContext.SaveChanges();

                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Accounts OFF");
                    dbContext.Database.CloseConnection();
                }
                else
                {
                    account.Balance = 0;
                    dbContext.SaveChanges();
                }
            }
        }

        static void UpdateBalance()
        {
            Console.WriteLine("UpdateBalance Started using " + Thread.CurrentThread.Name);
            using (var dbContext = new BankDbContext())
            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var account = dbContext.Accounts
                       .Where(x => x.Id == 1)
                       .FirstOrDefault();

                    account.Balance += 10000;
                    dbContext.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Set [ConcurrencyCheck] attribute to the property in the model first
                    // EF will check if the property dirty or not
                    Console.WriteLine("UpdateBalance Failed using " + Thread.CurrentThread.Name);
                }
            }
            Console.WriteLine("UpdateBalance Ended using " + Thread.CurrentThread.Name);
        }

        static void ShowBalance()
        {
            using (var dbContext = new BankDbContext())
            {
                var account = dbContext.Accounts
                    .Where(x => x.Id == 1)
                    .FirstOrDefault();

                Console.WriteLine("Balance : " + account.Balance);
            }
        }
    }
}

