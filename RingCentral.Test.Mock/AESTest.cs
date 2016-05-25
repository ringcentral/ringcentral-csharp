using NUnit.Framework;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class AESTest
    {
        [Test]
        public void AESDecryption()
        {
            var dataString = "YSholLwd6hNXsLPHW+DZQsdhFutxTGa04L6E4ySBQ4ihyjZ/7iH9VdqAiEsmFLrNNaHT+RfGAJfqmwoAfS2E0uJI/dY3UkuiBq152iKnoWkEhphdkAeerQwBbCCl82F6/2ezoyBbTomwkalyV32djQF7Xh5jxJGw1xorldxX6WUJ/a3g4EPU7zAIVBElx+kOJm4TwfiznJoNAArVFp96MxjDe4aHrVNpPSFzQrVKdgTVsXuqTaX5j0Dq1bHfZZ539L7C/yTT1mSCO9BCWdyclquDdKwf40V+Up0eMYgV8tGMqI5r0I8OaU7djD66Jv/HICikqFOLwPoHj7uQby/SXMKPKxA/7UVfDFeNHrC7ZXdIj2LDv7NC2WHG/c92qDoORbuO20+dLWUGqyKnwYx4Xh7EqBfVM2uQErbH9pk2DZ1WL93I6zdoGtRwzQBI7GwRY0KtPchJICdHkjA42s3U0g==";
            var keyString = "1q3kw4BhAAtT0SJkZ0a8EA==";
            var expected = "{\"uuid\":\"3622482f-465b-4f6b-9beb-44f508b59016\",\"event\":\"/restapi/v1.0/account/~/extension/850957020/message-store\",\"timestamp\":\"2016-05-25T00:54:36.834Z\",\"subscriptionId\":\"57b5d366-ab22-490b-9754-b60fa6aab373\",\"body\":{\"extensionId\":850957020,\"lastUpdated\":\"2016-05-25T00:54:22.055+0000\",\"changes\":[{\"type\":\"SMS\",\"newCount\":0,\"updatedCount\":1}]}}";
            
            var key = Convert.FromBase64String(keyString);            
            var keyParameter = ParameterUtilities.CreateKeyParameter("AES", key);
            var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");
            cipher.Init(false, keyParameter);

            var data = Convert.FromBase64String(dataString);
            var memoryStream = new MemoryStream(data, false);
            var cipherStream = new CipherStream(memoryStream, cipher, null);

            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var length = 0;
            var resultStream = new MemoryStream();
            while ((length = cipherStream.Read(buffer, 0, bufferSize)) > 0)
            {
                resultStream.Write(buffer, 0, length);
            }
            var result = Encoding.UTF8.GetString(resultStream.ToArray());

            Assert.AreEqual(expected, result);
        }
    }
}
