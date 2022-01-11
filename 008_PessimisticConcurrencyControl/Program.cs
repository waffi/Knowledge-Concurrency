using _008_PessimisticConcurrencyControl.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;

namespace _008_PessimisticConcurrencyControl
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pessimistic Concurrency Control assumes that something will and so locks it

            // Take an exclusive lock so that no one else can start modifying the record
            // Others have to wait until the lock is released
            // Can caused deadlock

            // Pessimistic locking is useful if there are a lot of updates and relatively high chances of users trying to update data at the same time

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
                dbContext.Database.OpenConnection();
                // Lock Database Row
                dbContext.Database.ExecuteSqlRaw("SELECT * FROM dbo.Accounts WITH(XLOCK, ROWLOCK) WHERE Id = 1");

                var account = dbContext.Accounts
                    .Where(x => x.Id == 1)
                    .FirstOrDefault();

                account.Balance += 10000;
                dbContext.SaveChanges();

                dbContextTransaction.Commit();

                dbContext.Database.CloseConnection();
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
