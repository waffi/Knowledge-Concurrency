using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _006_Deadlock_Solution
{
    public class AccountManager
    {
        private Account FromAccount;
        private Account ToAccount;
        private double TransferAmount;

        public AccountManager(Account AccountFrom, Account AccountTo, double AmountTransfer)
        {
            FromAccount = AccountFrom;
            ToAccount = AccountTo;
            TransferAmount = AmountTransfer;
        }

        public void FundTransfer()
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {FromAccount.ID}");
            lock (FromAccount)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {FromAccount.ID}");
                Console.WriteLine($"{Thread.CurrentThread.Name} Doing Some work");
                Thread.Sleep(1000);
                Console.WriteLine($"{Thread.CurrentThread.Name} trying to acquire lock on {ToAccount.ID}");


                // before

                //lock (ToAccount)
                //{
                //    FromAccount.WithdrawMoney(TransferAmount);
                //    ToAccount.DepositMoney(TransferAmount);
                //}

                // after ( use try attempt to acquired lock )

                if (Monitor.TryEnter(ToAccount, 3000))
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} acquired lock on {ToAccount.ID}");
                    try
                    {
                        FromAccount.WithdrawMoney(TransferAmount);
                        ToAccount.DepositMoney(TransferAmount);
                    }
                    finally
                    {
                        Monitor.Exit(ToAccount);
                    }
                }
                else
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} Unable to acquire lock on {ToAccount.ID}, So existing.");
                }
            }
        }
    }
}
