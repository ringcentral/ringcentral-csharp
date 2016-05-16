using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace RingCentral
{
    public class Auth
    {
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        private long AccessTokenExpiresIn { get; set; }
        private long AccessTokenExpireTime { get; set; }
        private long RefreshTokenExpiresIn { get; set; }
        private long RefreshTokenExpireTime { get; set; }
        public bool Remember { get; set; }
        private string TokenType { get; set; }
        private string OwnerId { get; set; }
        private string Scope { get; set; }

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
                TokenType = (string)jToken.SelectToken("token_type");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("owner_id")))
            {
                OwnerId = (string)jToken.SelectToken("owner_id");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("scope")))
            {
                Scope = (string)jToken.SelectToken("scope");
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
                AccessTokenExpiresIn = (long)jToken.SelectToken("expires_in");
            }

            if (string.IsNullOrEmpty((string)jToken.SelectToken("expire_time")) &&
                !string.IsNullOrEmpty((string)jToken.SelectToken("expires_in")))
            {
                AccessTokenExpireTime = ((Convert.ToInt64((string)jToken.SelectToken("expires_in")) * 1000) + currentTimeInMilliseconds);
            }
            else if (!string.IsNullOrEmpty((string)jToken.SelectToken("expire_time")))
            {
                AccessTokenExpireTime = (long)jToken.SelectToken("expire_time");
            }
            #endregion

            #region Refresh Token
            if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token")))
            {
                RefreshToken = (string)jToken.SelectToken("refresh_token");
            }

            if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expires_in")))
            {
                RefreshTokenExpiresIn = (long)jToken.SelectToken("refresh_token_expires_in");
            }

            if (string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expire_time")) &&
                !string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expires_in")))
            {
                RefreshTokenExpireTime = ((Convert.ToInt64((string)jToken.SelectToken("refresh_token_expires_in")) * 1000) + currentTimeInMilliseconds);
            }
            else if (!string.IsNullOrEmpty((string)jToken.SelectToken("refresh_token_expire_time")))
            {
                RefreshTokenExpireTime = (long)jToken.SelectToken("refresh_token_expire_time");
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
                TokenType = data["token_type"];
            }

            if (data.ContainsKey("owner_id") && !String.IsNullOrEmpty(data["owner_id"]))
            {
                OwnerId = data["owner_id"];
            }

            if (data.ContainsKey("scope") && !String.IsNullOrEmpty(data["scope"]))
            {
                Scope = data["scope"];
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
                AccessTokenExpiresIn = Convert.ToInt64(data["expires_in"]);
            }

            if ((data.ContainsKey("expire_time") && string.IsNullOrEmpty(data["expire_time"])) &&
                (data.ContainsKey("expires_in") && !string.IsNullOrEmpty(data["expires_in"])))
            {
                AccessTokenExpireTime = ((Convert.ToInt64(data["expires_in"]) * 1000) + currentTimeInMilliseconds);
            }
            else if (data.ContainsKey("expires_time") && !string.IsNullOrEmpty(data["expire_time"]))
            {
                AccessTokenExpireTime = Convert.ToInt64(data["expire_time"]);
            }
            #endregion

            #region Refresh Token
            if (data.ContainsKey("refresh_token") && !String.IsNullOrEmpty(data["refresh_token"]))
            {
                RefreshToken = data["refresh_token"];
            }

            if (data.ContainsKey("refresh_token_expires_in") && !string.IsNullOrEmpty(data["refresh_token_expires_in"]))
            {
                RefreshTokenExpiresIn = Convert.ToInt64(data["refresh_token_expires_in"]);
            }

            if ((data.ContainsKey("refresh_token_expire_time") && string.IsNullOrEmpty(data["refresh_token_expire_time"])) &&
                (data.ContainsKey("refresh_token_expires_in") && !string.IsNullOrEmpty(data["refresh_token_expires_in"])))
            {
                RefreshTokenExpireTime = ((Convert.ToInt64(data["refresh_token_expires_in"]) * 1000) + currentTimeInMilliseconds);
            }
            else if (data.ContainsKey("refresh_token_expire_time") && !string.IsNullOrEmpty(data["refresh_token_expire_time"]))
            {
                RefreshTokenExpireTime = Convert.ToInt64(data["refresh_token_expire_time"]);
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
                               {"token_type", TokenType},
                               {"access_token", AccessToken},
                               {"expires_in", AccessTokenExpiresIn.ToString()},
                               {"expire_time", AccessTokenExpireTime.ToString()},
                               {"refresh_token", RefreshToken},
                               {"refresh_token_expires_in", RefreshTokenExpiresIn.ToString()},
                               {"refresh_token_expire_time", RefreshTokenExpireTime.ToString()},
                               {"scope", Scope},
                               {"owner_id", OwnerId}
                           };

            return authData;
        }

        /// <summary>
        ///     Resets the Auth Data
        /// </summary>
        public void Reset()
        {
            Remember = false;
            TokenType = "";

            AccessToken = null;
            AccessTokenExpiresIn = 0;
            AccessTokenExpireTime = 0;

            RefreshToken = null;
            RefreshTokenExpiresIn = 0;
            RefreshTokenExpireTime = 0;

            Scope = "";
            OwnerId = "";
        }

        /// <summary>
        ///     Private method to determine if the Access Token or Refresh Token is valid
        /// </summary>
        /// <param name="accessTokenExpireTime">Access token expire time</param>
        /// <returns>bool value of token validity</returns>
        private bool IsTokenValid(long accessTokenExpireTime)
        {
            var currentTimeInMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return accessTokenExpireTime > currentTimeInMilliseconds;
        }

        /// <summary>
        ///     Determines if the Access Token is still valid
        /// </summary>
        /// <returns>The bool value of Refresh token validity</returns>
        public bool IsAccessTokenValid()
        {
            return IsTokenValid(AccessTokenExpireTime);
        }

        /// <summary>
        ///     Determines if the Refresh Token is still valid
        /// </summary>
        /// <returns>The bool value of Refresh token validity</returns>
        public bool IsRefreshTokenValid()
        {
            return IsTokenValid(RefreshTokenExpireTime);
        }
    }
}