using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessRep.Repositories
{
    public static class StringCryptoHelper
    {
        //3DES
        public static string Encrypt(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string choosenKey = "lmao-kcfu-edisni";
                byte[] byteFormOfInput = Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(choosenKey),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform crytopTransform = tripleDes.CreateEncryptor();
                byte[] encryptedInputInBytes = crytopTransform.TransformFinalBlock(byteFormOfInput, 0, byteFormOfInput.Length);
                tripleDes.Clear();
                return Convert.ToBase64String(encryptedInputInBytes, 0, encryptedInputInBytes.Length);
            }
            return input;
        }
        public static string Decrypt(this string input)
        {
            if (!string.IsNullOrEmpty(input) )
            {
                string choosenKey = "lmao-kcfu-edisni";
                byte[] byteFormOfInput = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(choosenKey),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform crytopTransform = tripleDes.CreateDecryptor();
                byte[] decryptedInputInBytes = crytopTransform.TransformFinalBlock(byteFormOfInput, 0, byteFormOfInput.Length);
                tripleDes.Clear();
                return Encoding.UTF8.GetString(decryptedInputInBytes);
            }
            return input;
        }

        public static bool IsAlphaNumeric(string input)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (r.IsMatch(input))
            {
                return true;
            }
            return false;
        }

        public static string CollapseAnswerText(this string input)
        {
            if (input.Length > 100)
            {
                return input.Substring(0, 98) + "...";
            }

            return input;

        }
    }
}
