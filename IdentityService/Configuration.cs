using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityService
{
    public class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("CatalogService", "Catalog")
            };
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("CatalogService", "Catalog", new [] { JwtClaimTypes.Name})
                {
                    Scopes = {"CatalogService"}
                }
            };
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                 new Client
                 {
                    ClientId = "catalog-service-web-api",
                    ClientName = "Catalog Service",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = true,
                    // RedirectUris =
                    // {
                    //     "htts://.../signin-oidc"
                    // },
                    // AllowedCorsOrigins =
                    // {
                    //     "http://..."
                    // },
                    // PostLogoutRedirectUris =
                    // {
                    //     "htts://.../signout-oidc"
                    // },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "CatalogService"
                    },
                    AllowAccessTokensViaBrowser = true
                 }
            };
    }
}