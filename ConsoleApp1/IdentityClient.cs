using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleApp1
{
    public class IdentityClient
    {
        public static async Task MainAsync()
        {
            // discover endpoints from metadata
            var svcUri = "http://localhost:8000";
            var disco = await DiscoveryClient.GetAsync(svcUri);

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            //13701407113
            // request token
            tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            //var cc = await tokenClient.RequestCustomGrantAsync("", "", new {token = ""});
            tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("bob", "password", "api1");
            var refreshToken = await tokenClient.RequestRefreshTokenAsync("1");
            refreshToken = await tokenClient.RequestRefreshTokenAsync("2");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var apiUri = "http://localhost:8001/identity";
            var response = await client.GetAsync(apiUri);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
