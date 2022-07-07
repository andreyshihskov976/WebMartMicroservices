using IdentityServer4;
using IdentityServer4.Models;

namespace WebMart.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                //Service-2-Service Scopes
                new ApiScope("catalog", "Catalog API"),
                new ApiScope("basket", "Basket API"),

                //Client app Scopes
                new ApiScope("admin_permissions", "Administrator permissions"),
                new ApiScope("user_permissions", "User permissions"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "ClientCredentials.Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes =
                    {
                        "catalog",
                        "basket"
                    }
                },

                // interactive client using code flow
                new Client
                {
                    ClientId = "application.client",
                    ClientName = "ResourceOwnerPassword.Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowOfflineAccess = true,

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "admin_permissions",
                        "user_permissions"
                    }
                },
            };
    }
}