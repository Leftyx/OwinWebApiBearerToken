using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ClientHttp
{
    class Program
    {
        public const string ClientId = "MyApp";
        public const string ClientSecret = "MySecret";
        public const string UserName = "John";
        public const string Password = "Smith";

        // // public const string baseUrl = "http://localhost.fiddler:8686";
        public const string baseUrl = "http://localhost:8686";

        static void Main(string[] args)
        {
            RunnerAsync();
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }

        public static async void RunnerAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var accessTokenResponse = await GetAccessTokenAsync(client);

                var values = await GetValuesAsync(client, accessTokenResponse);
                Console.WriteLine(values);

                // AccessTokenResponse accessTokenRefreshResponse = await GetRefreshTokenAsync(client, accessTokenResponse.RefreshToken);
                // values = await GetValuesAsync(client, accessTokenRefreshResponse);
                //Console.WriteLine(values);
            }
        }

        public static async Task<object> GetValuesAsync(HttpClient client, AccessTokenResponse accessTokenResponse)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/api/customer/1"))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken);

                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsAsync<object>();

                    return result;
                }
            }
        }

        public static async Task<AccessTokenResponse> GetAccessTokenAsync(HttpClient client)
        {
            /*
             *** If success, returns something as follows:
             {
                 "access_token": "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAL0Tx4aDDzEy32ulnOq3LdwAAAAACAAAAAAADZgAAwAAAABAAAAANbKP3adQpekCa-oSGJn8ZAAAAAASAAACgAAAAEAAAAH3Rzt0q4gfIsP4XhjB7cScIAQAAmj0CKW5XuJx2nadjiZcFRYLZPbF3NRUZnuc5C0-BQS4iJ04LQgErgv-bAYpO8WuDLW9RRUjFWAEpvWSY8ohmAPojWqNWrHcwe3bMeXLK7lBT12YJo0EgbeWKRgKer-yhctn3HybHuf43fGrej7RgzTca_0aKXQRV38uIv6LN7t9_ebx1Ov7QYZmoTZPqbbXhFPvjr0hRxEljUrJbABU_lS3FOz5hMTs0k8pz5WfH4BBJr2oqTMwxRrstQDUKUs0gcFAJKSOpjxbuKyqVbD7cFHJEKhbRQZO_9T_oQY29BxRXFysxCDWrWkJj0Crn1RB3Tw4v-ytZ1jDDTeOOBtJnXHIzBNvC-1vuFAAAAM3K3bYpIoId8XWs6JNc3O7vQD2I",
                 "token_type": "bearer",
                 "expires_in": 1209600,
                 "refresh_token": "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAL0Tx4aDDzEy32ulnOq3LdwAAAAACAAAAAAADZgAAwAAAABAAAAAVRydlGVhaZNrxt4-oNdyxAAAAAASAAACgAAAAEAAAAEqR-EIUM99O9sMHbJkeJPoIAQAAeddehchEbWjQiLCH8DiPRbZ_KLCBbhwPufIH7sGgBSAeZI9RDPwYTt0ThzOt6Ea9XCmPgv63WNY86UdlwUDy4fIY2g4sRxL9MrB0TEYt90tOJQojxVWFAvDOkxn_E5Mz5IuAJ3cGuZYLaxCxGJW4G_eBOrFsDPlOCQll1Pd6VEI_DYGFzfmVR3KsW41OyAyKnO-388zylouaM31chOC0aj6JiOW2YwJMhfGxKBZgq6gSZba2oMW327LT0vGLm4c3rHVvU5TkeyfgnRIO7s4mUh4FyEgmLFR32a9NyRDQj5ir3i86U2SMPPuJiR8F8trT_6E07_ASYQdtEnQr-pHQUgM46IWWJOD4FAAAAECwQpRIKa_rKdOtYawVhw8H3Fs9"
             }
             */

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/oauth/token"))
            {
                var values = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>(Constants.Parameters.GrantType, Constants.GrantTypes.Password),
                    new KeyValuePair<string, string>(Constants.Parameters.Username, UserName),
                    new KeyValuePair<string, string>(Constants.Parameters.Password, Password),
                    // new KeyValuePair<string, string>(Constants.Parameters.Scope, "Read Write")
                };

                // client_id and client_secret: http://tools.ietf.org/html/rfc6749#section-2.3.1
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(string.Format("{0}:{1}", ClientId, ClientSecret)));
                request.Content = new FormUrlEncodedContent(values);
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsAsync<AccessTokenResponse>();
                    return result;
                }
            }
        }

        public static async Task<AccessTokenResponse> GetRefreshTokenAsync(HttpClient client, string refreshToken)
        {
            /*
             *** If success, returns something as follows:
             {
                 "access_token": "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAL0Tx4aDDzEy32ulnOq3LdwAAAAACAAAAAAADZgAAwAAAABAAAAANbKP3adQpekCa-oSGJn8ZAAAAAASAAACgAAAAEAAAAH3Rzt0q4gfIsP4XhjB7cScIAQAAmj0CKW5XuJx2nadjiZcFRYLZPbF3NRUZnuc5C0-BQS4iJ04LQgErgv-bAYpO8WuDLW9RRUjFWAEpvWSY8ohmAPojWqNWrHcwe3bMeXLK7lBT12YJo0EgbeWKRgKer-yhctn3HybHuf43fGrej7RgzTca_0aKXQRV38uIv6LN7t9_ebx1Ov7QYZmoTZPqbbXhFPvjr0hRxEljUrJbABU_lS3FOz5hMTs0k8pz5WfH4BBJr2oqTMwxRrstQDUKUs0gcFAJKSOpjxbuKyqVbD7cFHJEKhbRQZO_9T_oQY29BxRXFysxCDWrWkJj0Crn1RB3Tw4v-ytZ1jDDTeOOBtJnXHIzBNvC-1vuFAAAAM3K3bYpIoId8XWs6JNc3O7vQD2I",
                 "token_type": "bearer",
                 "expires_in": 1209600,
                 "refresh_token": "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAL0Tx4aDDzEy32ulnOq3LdwAAAAACAAAAAAADZgAAwAAAABAAAAAVRydlGVhaZNrxt4-oNdyxAAAAAASAAACgAAAAEAAAAEqR-EIUM99O9sMHbJkeJPoIAQAAeddehchEbWjQiLCH8DiPRbZ_KLCBbhwPufIH7sGgBSAeZI9RDPwYTt0ThzOt6Ea9XCmPgv63WNY86UdlwUDy4fIY2g4sRxL9MrB0TEYt90tOJQojxVWFAvDOkxn_E5Mz5IuAJ3cGuZYLaxCxGJW4G_eBOrFsDPlOCQll1Pd6VEI_DYGFzfmVR3KsW41OyAyKnO-388zylouaM31chOC0aj6JiOW2YwJMhfGxKBZgq6gSZba2oMW327LT0vGLm4c3rHVvU5TkeyfgnRIO7s4mUh4FyEgmLFR32a9NyRDQj5ir3i86U2SMPPuJiR8F8trT_6E07_ASYQdtEnQr-pHQUgM46IWWJOD4FAAAAECwQpRIKa_rKdOtYawVhw8H3Fs9"
             }
             */

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/oauth/token"))
            {
                var values = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>(Constants.Parameters.GrantType, Constants.GrantTypes.RefreshToken),
                    new KeyValuePair<string, string>(Constants.Parameters.RefreshToken, refreshToken),
                };

                // client_id and client_secret: http://tools.ietf.org/html/rfc6749#section-2.3.1
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", EncodeToBase64(string.Format("{0}:{1}", ClientId, ClientSecret)));
                request.Content = new FormUrlEncodedContent(values);
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsAsync<AccessTokenResponse>();
                    return result;
                }
            }
        }

        private static string EncodeToBase64(string value)
        {
            var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

    }
}
