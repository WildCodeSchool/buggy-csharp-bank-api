using System;
using SimpleREST;
using SimpleREST.Routing;
namespace TrustablePerimeter
{
    public class Server : SimpleREST.HTTP.HttpServer
    {
        DatabaseHandler database;
        public Server(string[] prefixes) : base(prefixes)
        {
            database = new DatabaseHandler();

            Router = new Router();
            Route createCustomer = new Route("/customer/create");
            Router.AddRouteMethod(createCustomer, Method.Post);
            Router.SetActionForUriMethod(new CreateCustomerAction(database),
                                         "/customer/create",                        
                                         Method.Post);
            Route customersRoute = new Route("/customers");
            Router.AddRouteMethod(customersRoute, Method.Post);
            Router.SetActionForUriMethod(new ShowCustomerAction(database),
                                         "/customers",
                                         Method.Post);
            Route createAccount= new Route("/account/create");
            Router.AddRouteMethod(createAccount, Method.Post);
            Router.SetActionForUriMethod(new CreateAccountAction(database),
                                         "/account/create",
                                         Method.Post);
            Route showAccounts = new Route("/accounts");
            Router.AddRouteMethod(showAccounts, Method.Post);
            Router.SetActionForUriMethod(new ShowAccountAction(database),
                                         "/accounts",
                                         Method.Post);
            Route withdrawalRoute = new Route("/account/withdraw");
            Router.AddRouteMethod(withdrawalRoute, Method.Post);
            Router.SetActionForUriMethod(new WithdrawMoneyAction(database),
                                         "/account/withdraw",
                                         Method.Post);
            /*Route creditmentRoute = new Route("/account/credit");
            Router.AddRouteMethod(creditmentRoute, Method.Post);
            Router.SetActionForUriMethod(new CreditMoneyAction(database),
                                         "/account/credit",
                                         Method.Post);
            Route accountRoute = new Route("/account");
            Router.AddRouteMethod(accountRoute, Method.Get);
            Router.SetActionForUriMethod(new ShowAccountsAction(database),
                                         "/account",
                                         Method.Get);
            */

        }
    }
}
