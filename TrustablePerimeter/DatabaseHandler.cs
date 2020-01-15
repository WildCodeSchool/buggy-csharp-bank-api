using System;
using TrustablePerimeter.Entities;
using System.Collections.Generic;

namespace TrustablePerimeter
{
    public class DatabaseHandler
    {
        private List<Account> Accounts = new List<Account>();
        private List<Customer> Customers = new List<Customer>();
        public DatabaseHandler()
        {
            Random randomGenerator = new Random();
            int currentId = 1;
            string[] customersName = { "Roland", "Jean-Baptiste", "Alfred", "Sidonie", "Bernadette", "Ingrid" };
            foreach (string name in customersName)
            {
                Account account = new Account();
                account.Money = randomGenerator.Next(0, 2000);
                account.Id = currentId;
                Customer customer = new Customer();
                account.Customer = customer;
                Accounts.Add(account);
                customer.Id = currentId;
                customer.Name = name;
                customer.Account = account;
                Customers.Add(customer);
                currentId = currentId + 1;
            }
        }

        public void UpdateAccount(Account newAccount)
        {
            Account currentAccount = GetAccountById(newAccount.Id);
            if (currentAccount is null)
            {
                Accounts.Add(newAccount);
            } else {
                currentAccount.Update(newAccount);
            }
        }

        public void UpdateCustomer(Customer newCustomer)
        {
            Customer currentCustomer = GetCustomerById(newCustomer.Id);
            if (currentCustomer is null)
            {
                Customers.Add(newCustomer);
            } else
            {
                currentCustomer.Update(newCustomer);
            }
        }

        public Customer[] GetAllCustomers()
        {
            Customer[] returnedCustomers = new Customer[Customers.Count];
            Customers.CopyTo(returnedCustomers);
            return returnedCustomers;
        }

        public Account[] GetAllAccounts()
        {
            Account[] returnedAccounts = new Account[Accounts.Count];
            Accounts.CopyTo(returnedAccounts);
            return returnedAccounts;
        }

        public Account GetAccountById(int id)
        {
            Account fetchedAccount = Accounts.Find((Account account) => account.Id == id);
            return fetchedAccount;
        }

        public Customer GetCustomerById(int id)
        {
            Customer fetchedCustomer = Customers.Find((Customer customer) => customer.Id == id);
            return fetchedCustomer;
        }

    }
}
