using LatchTalk.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using OtpSharp;
using Base32;
using System.IO;

namespace LatchTalk
{
    public class TotpProvider : IUserTokenProvider<ApplicationUser, string>
    {
        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult((string)null);
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult(user.TotpAuthenticatorEnabled);
        }

        public Task NotifyAsync(string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser, string> manager, ApplicationUser user)
        {
            return ValidateAsync(user.TotpSecretKey, token);
        }

        public static Task<bool> ValidateAsync(string key, string token)
        {
            long timeStepMatched;
            Totp otp = new Totp(Base32Encoder.Decode(key));
            bool valid = otp.VerifyTotp(token, out timeStepMatched, new VerificationWindow(2, 2));
            return Task.FromResult(valid);
        }

        public static TotpAuthenticatorViewModel GenerateNew(string username)
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            TotpAuthenticatorViewModel model = new TotpAuthenticatorViewModel()
            {
                SecretKey = Base32Encoder.Encode(secretKey),
                BarcodeUrl = KeyUrl.GetTotpUrl(secretKey, username) + "&issuer=LatchTalk"
            };

            using (MemoryStream imageStream = new MemoryStream())
            {
                new MessagingToolkit.QRCode.Codec.QRCodeEncoder().Encode(model.BarcodeUrl).Save(imageStream, System.Drawing.Imaging.ImageFormat.Bmp);
                model.BarcodeBase64Image = Convert.ToBase64String(imageStream.ToArray());
            }
            return model;
        }


    }
}