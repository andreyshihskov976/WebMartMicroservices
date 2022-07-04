using IdentityServer4.Models;

namespace WebMart.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("CatalogService", "Catalog API"),
                new ApiScope("BasketService", "Basket API"),
                new ApiScope("OrderService", "Order API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "BasketService.Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("BasketService.Secret".Sha256())
                    },
                    AllowedScopes = { "CatalogService" }
                },
                new Client
                {
                    ClientId = "OrderService.Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("OrderService.Secret".Sha256())
                    },
                    AllowedScopes = { "BasketService" }
                },
            };
    }
}