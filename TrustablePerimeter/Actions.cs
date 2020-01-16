using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrustablePerimeter.Entities;

namespace TrustablePerimeter
{
    public abstract class AbstractAction : SimpleREST.Routing.Action
    {
        protected readonly DatabaseHandler _database;
     
        public AbstractAction(DatabaseHandler database)
        {
            _database = database;
        }

        protected IDictionary<string, object> ExtractBodyParameters(Stream requestDataStream)
        {
            string[] bodyParamsAffectations = ExtractParsedQueryParamsAffectations(requestDataStream);
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (string affectation in bodyParamsAffectations)
            {
                string[] operands = affectation.Split('=');
                if (operands.Length == 2)
                {
                    parameters.Add(operands[0], operands[1]);
                }
            }
            return parameters;
        }

        private string[] ExtractParsedQueryParamsAffectations(Stream requestDataStream)
        {
            byte[] rawData = new byte[256];
            int offset = 0;
            int bytesRead;
            bool finished = false;
            string decodedData = "";
            while(!finished)
            {
                bytesRead = requestDataStream.Read(rawData, offset, 32);
                offset = offset + bytesRead;
                decodedData = decodedData + Encoding.UTF8.GetString(rawData);
                finished = bytesRead == 0 || decodedData.Contains("\0");
            }
            decodedData = decodedData.Trim('\0');
            string[] queryParamsAffectations = decodedData.Split('&');
            return queryParamsAffectations;
        }
    }

    /// <summary>
    /// An action to withdraw money from an account
    /// Uniform Resource Locator: /account/withdraw
    /// Method: POST
    /// Parameters:
    ///     id - id of an account
    ///     amount - amount of money to withdraw
    /// Unless you specify all the arguments, calling this action will throw an exception
    /// </summary>
    public class WithdrawMoneyAction : AbstractAction
    {
        public WithdrawMoneyAction(DatabaseHandler database) : base(database)
        { }

        
        public override void DoAction(HttpListenerContext context)
        {
            IDictionary<string, object> parameters = ExtractBodyParameters(context.Request.InputStream);
            int accountId = -1;
            float amount = 0;
            if (parameters.ContainsKey("id"))
            {
                accountId = Convert.ToInt32(parameters["id"]);
            }
            if (parameters.ContainsKey("amount"))
            {
                amount = Convert.ToSingle(parameters["amount"]);
            }
            if (accountId == -1)
            {
                throw new SimpleREST.Exceptions.EndPointNotFoundException(HttpStatusCode.BadRequest, 
                                                                          "Bad request, please give the id of the account to withdraw");
            }
            Account fetchedAccount = _database.GetAccountById(accountId);
            fetchedAccount.Withdraw(amount);
        }
    }

    /// <summary>
    /// This action shows customer(s) data.
    /// Uniform Resource Locator: /customers
    /// Method: POST
    /// Parameters:
    ///     id - id of a customer
    /// If you don't specify the id parameter, this action will return every customer in the database
    /// </summary>
    public class ShowCustomerAction : AbstractAction
    {
        public ShowCustomerAction(DatabaseHandler database) : base(database)
        { }

        public override void DoAction(HttpListenerContext context)
        {
            Customer[] allCustomers;
            IDictionary<string, object> parameters = ExtractBodyParameters(context.Request.InputStream);
            if (parameters.ContainsKey("id"))
            {
                int fetchedCustomerId = Convert.ToInt32(parameters["id"]);
                Customer fetchedCustomer = _database.GetCustomerById(fetchedCustomerId);
                allCustomers = new Customer[1];
                allCustomers[0] = fetchedCustomer;
            }
            else
            {
                allCustomers = _database.GetAllCustomers();
            }

            foreach (Customer customer in allCustomers)
            {
                string response = "-----------------------------------------" + "\n" +
                            "Id:" + customer.Id + "\n" +
                            "Name:" + customer.Name + "\n" +
                            "Account:" + customer.Account.ToString() + "\n" +
                            "-----------------------------------------" + "\n";
                byte[] encodedResponse = Encoding.UTF8.GetBytes(response);
                context.Response.OutputStream.Write(encodedResponse, 0, encodedResponse.Length);
            }
        }
    }

    /// <summary>
    /// This action shows account(s) data.
    /// Uniform Resource Locator: /accounts
    /// Method: POST
    /// Parameters:
    ///     id - id of an account
    /// If you don't specify the id parameter, this action will return every account in the database
    /// </summary>
    public class ShowAccountAction : AbstractAction
    {
        public ShowAccountAction(DatabaseHandler database) : base(database)
        { }

        public override void DoAction(HttpListenerContext context)
        {
            IDictionary<string, object> parameters = ExtractBodyParameters(context.Request.InputStream);
            Account[] allAccounts;
            if (parameters.ContainsKey("id"))
            {
                Account fetchedAccount = _database.GetAccountById(Convert.ToInt32(parameters["id"]));
                allAccounts = new Account[1];
                allAccounts[0] = fetchedAccount;
            }
            else
            {
                allAccounts = _database.GetAllAccounts();
            }
            foreach (Account account in allAccounts)
            {
                string response = "-----------------------------------------" + "\n" +
                            "Id:" + account.Id + "\n" +
                            "Customer:" + account.Customer.Name + "\n" + 
                            "Money:" + account.Money + "\n" +
                            "-----------------------------------------" + "\n";
                byte[] encodedResponse = Encoding.UTF8.GetBytes(response);
                context.Response.OutputStream.Write(encodedResponse, 0, encodedResponse.Length);
            }
        }
    }

    /// <summary>
    /// This action creates or updates a customer.
    /// Uniform Resource Locator: /customer/create
    /// Method: POST
    /// Parameters:
    ///     id - id of a customer
    ///     name - name of the customer
    ///     account - account id that belong to the customer
    /// If a customer in the database already has the same id, data from the request will replace data in the database
    /// </summary>
    public class CreateCustomerAction : AbstractAction
    {
        public CreateCustomerAction(DatabaseHandler database) : base(database)
        { }

        public override void DoAction(HttpListenerContext context)
        {
            IDictionary<string, object> parameters = ExtractBodyParameters(context.Request.InputStream);
            Customer customer = new Customer();
            if (parameters.ContainsKey("account"))
            {
                Account account = _database.GetAccountById(Convert.ToInt32(parameters["account"]));
                if (account is null)
                {
                    account = new Account();
                    _database.UpdateAccount(account);
                }
                customer.Account = account;
            }
            parameters.Remove("account");
            customer.Update(parameters);
            byte[] customerId = Encoding.UTF8.GetBytes(Convert.ToString(customer.Id));
            context.Response.OutputStream.Write(customerId, 0, customerId.Length);
            byte[] customerName = Encoding.UTF8.GetBytes(Convert.ToString(customer.Name));
            context.Response.OutputStream.Write(customerName, 0, customerName.Length);
            _database.UpdateCustomer(customer);
        }
    }

    /// <summary>
    /// This action creates or updates an account.
    /// Uniform Resource Locator: /account/create
    /// Method: POST
    /// Parameters:
    ///     id - id of a customer
    ///     customer - id of the customer that the account belongs to
    /// If an account in the database already has the same id, data from the request will replace data in the database
    /// </summary>
    public class CreateAccountAction : AbstractAction
    {
        public CreateAccountAction(DatabaseHandler database) : base(database)
        { }

        public override void DoAction(HttpListenerContext context)
        {
            IDictionary<string, object> parameters = ExtractBodyParameters(context.Request.InputStream);
            Account account = new Account();
            account.Update(parameters);
            byte[] accountId = Encoding.UTF8.GetBytes(Convert.ToString(account.Id));
            context.Response.OutputStream.Write(accountId, 0, accountId.Length);
            byte[] accountMoney = Encoding.UTF8.GetBytes(Convert.ToString(account.Money));
            context.Response.OutputStream.Write(accountMoney, 0, accountMoney.Length);
            _database.UpdateAccount(account);
        }
    }
}
