using PayPal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.UI.Util
{
    public static class PayPalConfiguration
    {
        //these variables will store the clientID and clientSecret
        //by reading them from the web.config
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Manager.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // getting accesstocken from paypal                
            string accessToken = new OAuthTokenCredential
        (ClientId, ClientSecret, GetConfig()).GetAccessToken();

            return accessToken;
        }

        public static PayPal.Api.APIContext GetAPIContext()
        {
            // return apicontext object by invoking it with the accesstoken
            PayPal.Api.APIContext apiContext = new PayPal.Api.APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}