using System;
using SimpleREST;
using SimpleREST.Routing;
namespace TrustablePerimeter
{
    public class Server : SimpleREST.HTTP.HttpServer
    {
        public Server(string[] prefixes) : base(prefixes)
        {
            Router = new Router();
            Route withdrawalRoute = new Route("/account/withdraw");
            Router.AddRouteMethod(withdrawalRoute, Method.Post);
            Router.SetActionForUriMethod(new WithdrawMoneyAction(),
                                         "/account/withdraw",
                                         Method.Post);
            Route creditmentRoute = new Route("/account/credit");
            Router.AddRouteMethod(creditmentRoute, Method.Post);
            Router.SetActionForUriMethod(new CreditMoneyAction(),
                                         "/account/credit",
                                         Method.Post);
            Route accountRoute = new Route("/account");
            Router.AddRouteMethod(accountRoute, Method.Get);
            Router.SetActionForUriMethod(new ShowAccountsAction(),
                                         "/account",
                                         Method.Get);
            Route customersRoute = new Route("/customers");
            Router.AddRouteMethod(customersRoute, Method.Post);
            Router.SetActionForUriMethod(new ShowAccountsAction(),
                                         "/customers",
                                         Method.Post);

        }
    }
}
