using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RingCentral
{
    public class Auth
    {
        private long accessTokenExpiresIn;
        private long accessTokenExpireTime;
        private long refreshTokenExpiresIn;
        private long refreshTokenExpireTime;
        private string tokenType;
        private string ownerId;
        private string scope;
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public bool Remember { get; set; }

        /// <summary>
        ///     After authorization data is set via this method so it may be retrieved and persisted later if a user wishes to
        ///     resume their active session.
        ///     Data stored:
        ///     remember - if a user wishes to be remembered for the max duration of the refresh token
        ///     token_type - the type of token
        ///     owner_id - the owner id
        ///     scope - the scope
        ///     access_token - value of the access token obtained on authorization
        ///     refresh_token - value of the refresh token obtained on authorization
        ///     Also stores when access token and refresh token expire
        /// </summary>
        /// <param name="jToken"></param>
        public void SetData(JToken jToken)
        {
            #region data
            if (!string.IsNullOrEmpty((string)jToken.SelectToken("remember")))
            {
                Remember = (bool)jToken.SelectToken("remember");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("token_type")))
            {
                tokenType = (string)jToken.SelectToken("token_type");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("owner_id")))
            {
                ownerId = (string)jToken.SelectToken("owner_id");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("scope")))
            {
                scope = (string)jToken.SelectToken("scope");
            }
            #endregion
            var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            #region Access Token
            if (!string.IsNullOrEmpty((string)jToken.SelectToken("access_token")))
            {
                AccessToken = (string)jToken.SelectToken("access_token");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("expires_in")))
            {
                accessTokenExpiresIn = (long)jToken.SelectToken("expires_in");
            }

            if (string.IsNullOrEmpty((string)jToken.SelectToken("expire_time")) &&
                !string.IsNullOrEmpty((string)jToken.SelectToken("expires_in")))
            {
                accessTokenExpireTime = ((Convert.ToInt64((string)jToken.SelectToken("expires_in")) * 1000) + currentTimeInMilliseconds);
            }
            else if (!string.IsNullOrEmpty((string)jToken.SelectToken("expire_time")))
            {
                accessTokenExpireTime = (long)jToken.SelectToken("expire_time");
            }
            #endregion

            #region Refresh Token
            if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token")))
            {
                RefreshToken = (string)jToken.SelectToken("refresh_token");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expires_in")))
            {
                refreshTokenExpiresIn = (long)jToken.SelectToken("refresh_token_expires_in");
            }

            if (string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expire_time")) &&
                !string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expires_in")))
            {
                refreshTokenExpireTime = ((Convert.ToInt64((string)jToken.SelectToken("refresh_token_expires_in")) * 1000) + currentTimeInMilliseconds);
            }
            else if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expire_time")))
            {
                refreshTokenExpireTime = (long)jToken.SelectToken("refresh_token_expire_time");
            }

            #endregion
        }

        public void SetData(Dictionary<string, string> data)
        {
            #region data
            if (data.ContainsKey("remember") && !String.IsNullOrEmpty(data["remember"]))
            {
                Remember = Convert.ToBoolean(data["remember"]);
            }

            if (data.ContainsKey("token_type") && !String.IsNullOrEmpty(data["token_type"]))
            {
                tokenType = data["token_type"];
            }

            if (data.ContainsKey("owner_id") && !String.IsNullOrEmpty(data["owner_id"]))
            {
                ownerId = data["owner_id"];
            }

            if (data.ContainsKey("scope") && !String.IsNullOrEmpty(data["scope"]))
            {
                scope = data["scope"];
            }
            #endregion
            var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            #region Access Token
            if (data.ContainsKey("access_token") && !String.IsNullOrEmpty(data["access_token"]))
            {
                AccessToken = data["access_token"];
            }

            if (data.ContainsKey("expires_in") && !string.IsNullOrEmpty(data["expires_in"]))
            {
                accessTokenExpiresIn = Convert.ToInt64(data["expires_in"]);
            }

            if ((data.ContainsKey("expire_time") && string.IsNullOrEmpty(data["expire_time"])) &&
                (data.ContainsKey("expires_in") && !string.IsNullOrEmpty(data["expires_in"])))
            {
                accessTokenExpireTime = ((Convert.ToInt64(data["expires_in"]) * 1000) + currentTimeInMilliseconds);
            }
            else if (data.ContainsKey("expires_time") && !string.IsNullOrEmpty(data["expire_time"]))
            {
                accessTokenExpireTime = Convert.ToInt64(data["expire_time"]);
            }
            #endregion

            #region Refresh Token
            if (data.ContainsKey("refresh_token") && !String.IsNullOrEmpty(data["refresh_token"]))
            {
                RefreshToken = data["refresh_token"];
            }

            if (data.ContainsKey("refresh_token_expires_in") && !string.IsNullOrEmpty(data["refresh_token_expires_in"]))
            {
                refreshTokenExpiresIn = Convert.ToInt64(data["refresh_token_expires_in"]);
            }

            if ((data.ContainsKey("refresh_token_expire_time") && string.IsNullOrEmpty(data["refresh_token_expire_time"])) &&
                (data.ContainsKey("refresh_token_expires_in") && !string.IsNullOrEmpty(data["refresh_token_expires_in"])))
            {
                refreshTokenExpireTime = ((Convert.ToInt64(data["refresh_token_expires_in"]) * 1000) + currentTimeInMilliseconds);
            }
            else if (data.ContainsKey("refresh_token_expire_time") && !string.IsNullOrEmpty(data["refresh_token_expire_time"]))
            {
                refreshTokenExpireTime = Convert.ToInt64(data["refresh_token_expire_time"]);
            }
            #endregion
        }

        /// <summary>
        ///     Gets the auth data set on authorization
        /// </summary>
        /// <returns>Dictionary of auth data</returns>
        public Dictionary<string, string> GetData()
        {
            var authData = new Dictionary<string, string>
                           {
                               {"remember", Remember.ToString()},
                               {"token_type", tokenType},
                               {"access_token", AccessToken},
                               {"expires_in", accessTokenExpiresIn.ToString()},
                               {"expire_time", accessTokenExpireTime.ToString()},
                               {"refresh_token", RefreshToken},
                               {"refresh_token_expires_in", refreshTokenExpiresIn.ToString()},
                               {"refresh_token_expire_time", refreshTokenExpireTime.ToString()},
                               {"scope", scope},
                               {"owner_id", ownerId}
                           };

            return authData;
        }

        /// <summary>
        ///     Resets the Auth Data
        /// </summary>
        public void Reset()
        {
            Remember = false;
            tokenType = "";

            AccessToken = null;
            accessTokenExpiresIn = 0;
            accessTokenExpireTime = 0;

            RefreshToken = null;
            refreshTokenExpiresIn = 0;
            refreshTokenExpireTime = 0;

            scope = "";
            ownerId = "";
        }

        /// <summary>
        ///     Private method to determine if the Access Token or Refresh Token is valid
        /// </summary>
        /// <param name="tokenExpireTime">Token expire time</param>
        /// <returns>bool value of token validity</returns>
        private bool IsTokenValid(long tokenExpireTime)
        {
            var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return tokenExpireTime > currentTimeInMilliseconds;
        }

        /// <summary>
        ///     Determines if the Access Token is still valid
        /// </summary>
        /// <returns>The bool value of Refresh token validity</returns>
        public bool IsAccessTokenValid()
        {
            return IsTokenValid(accessTokenExpireTime);
        }

        /// <summary>
        ///     Determines if the Refresh Token is still valid
        /// </summary>
        /// <returns>The bool value of Refresh token validity</returns>
        public bool IsRefreshTokenValid()
        {
            return IsTokenValid(refreshTokenExpireTime);
        }
    }
}