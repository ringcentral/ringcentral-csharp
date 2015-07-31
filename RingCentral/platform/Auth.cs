using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace RingCentral
{
    public class Auth
    {
        private string AccessToken { get; set; }
        private string RefreshToken { get; set; }
        private long AccessTokenExpiresIn { get; set; }
        private long AccessTokenExpireTime { get; set; }
        private long RefreshTokenExpiresIn { get; set; }
        private long RefreshTokenExpireTime { get; set; }
        private bool Remember { get; set; }
        private string TokenType { get; set; }
        private string OwnerId { get; set; }
        private string Scope { get; set; }


        public void SetData(JToken jToken)
        {
            if (!String.IsNullOrEmpty((string)jToken.SelectToken("remember")))
            {
                Remember = (bool)jToken.SelectToken("remember");
            }

            if (!String.IsNullOrEmpty((string)jToken.SelectToken("token_type")))
            {
                TokenType = (string)jToken.SelectToken("token_type");
            }

            if (!String.IsNullOrEmpty((string)jToken.SelectToken("owner_id")))
            {
                OwnerId = (string)jToken.SelectToken("owner_id");
            }

            if (!String.IsNullOrEmpty((string)jToken.SelectToken("scope")))
            {
                Scope = (string)jToken.SelectToken("scope");
            }

            if (!String.IsNullOrEmpty((string) jToken.SelectToken("access_token")))
            {
                AccessToken = (string) jToken.SelectToken("access_token");
            }

            if (!String.IsNullOrEmpty((string) jToken.SelectToken("refresh_token")))
            {
                RefreshToken = (string) jToken.SelectToken("refresh_token");
            }

            long currentTimeInMilliseconds = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;

            if (!String.IsNullOrEmpty((string) jToken.SelectToken("expires_in")))
            {
                AccessTokenExpiresIn = (long) jToken.SelectToken("expires_in");

                if (!String.IsNullOrEmpty((string) jToken.SelectToken("expire_time")))
                {
                    AccessTokenExpireTime = (long) jToken.SelectToken("expire_time");
                }
                else
                {
                    AccessTokenExpireTime = ((AccessTokenExpiresIn * 1000) + currentTimeInMilliseconds);
                } 
            }

            if (!String.IsNullOrEmpty((string) jToken.SelectToken("refresh_token_expires_in")))
            {
                RefreshTokenExpiresIn = (long) jToken.SelectToken("refresh_token_expires_in");

                if (!String.IsNullOrEmpty((string)jToken.SelectToken("expires_time")))
                {
                    RefreshTokenExpireTime = (long)jToken.SelectToken("refresh_token_expire_time");
                }
                else
                {
                    RefreshTokenExpireTime = ((RefreshTokenExpiresIn * 1000) + currentTimeInMilliseconds);
                }
                
            }
        }

        public Dictionary<string, string> GetAuthData()
        {
            var authData = new Dictionary<string, string>
                           {
                               {"remember", IsRemember().ToString()},
                               {"token_type", TokenType},
                               {"access_token", AccessToken},
                               {"expires_in", AccessTokenExpiresIn.ToString()},
                               {"expire_time", AccessTokenExpireTime.ToString()},
                               {"refresh_token", RefreshToken},
                               {"refresh_token_expires_in", RefreshTokenExpiresIn.ToString()},
                               {"refresh_token_expire_time", RefreshTokenExpireTime.ToString()},
                               {"scope",Scope},
                               {"owner_id", OwnerId}
                           };

            return authData;
        }

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
        /// <param name="accessToken"></param>
        /// <returns>bool value of token validity</returns>
        private bool IsTokenValid(long accessToken)
        {
            long currentTimeInMilliseconds = DateTime.Now.Ticks/TimeSpan.TicksPerMillisecond;

            return accessToken > currentTimeInMilliseconds;
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

        public string GetRefreshToken()
        {
            return RefreshToken;
        }

        public string GetAccessToken()
        {
            return AccessToken;
        }

        public bool IsRemember()
        {
            return Remember;
        }

        public void SetRemember(bool isRemember)
        {
            Remember = isRemember;
        }

    }
}