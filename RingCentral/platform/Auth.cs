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


        public void SetData(JToken jToken)
        {
            if (!String.IsNullOrEmpty((string)jToken.SelectToken("remember")))
            {
                Remember = (bool)jToken.SelectToken("remember");
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
                AccessTokenExpireTime = ((AccessTokenExpiresIn*1000) + currentTimeInMilliseconds);
            }

            if (!String.IsNullOrEmpty((string) jToken.SelectToken("refresh_token_expires_in")))
            {
                RefreshTokenExpiresIn = (long) jToken.SelectToken("refresh_token_expires_in");
                RefreshTokenExpireTime = ((RefreshTokenExpiresIn*1000) + currentTimeInMilliseconds);
            }
        }

        public Dictionary<string, string> GetAuthData()
        {
            var authData = new Dictionary<string, string>
                           {
                               {"access_token", GetAccessToken()},
                               {"refresh_token", GetRefreshToken()},
                               {"expires_in", GetAccessTokenExpiresIn()},
                               {"refresh_token_expires_in", GetRefreshTokenExpiresIn()},
                               {"remember", IsRemember().ToString()}
                           };

            return authData;
        }

        public void Reset()
        {
            AccessToken = null;
            RefreshToken = null;
            AccessTokenExpiresIn = 0;
            AccessTokenExpireTime = 0;
            RefreshTokenExpiresIn = 0;
            RefreshTokenExpireTime = 0;
            Remember = false;
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

        public string GetAccessTokenExpiresIn()
        {
            return AccessTokenExpiresIn.ToString();
        }

        public string GetRefreshTokenExpiresIn()
        {
            return RefreshTokenExpiresIn.ToString();
        }
    }
}