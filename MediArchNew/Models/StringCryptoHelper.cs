using System;
using System.Security.Cryptography;
using System.Text;

namespace MediArch.Models
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
            else
            {
                return input;
            }
        }
        public static string Decrypt(this string input)
        {
            if (!string.IsNullOrEmpty(input) && (input.Contains("=") || input.Contains("+") || input.Contains("/") || input.Contains("\\")))
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

        public static string CollapseAnswerText(this string input)
        {
            if (input.Length > 60)
            {
                return input.Substring(0, 57)+"...";
            }
                return input;

        }

        public static string CollapseMail(this string input)
        {
            if (input.Length > 25)
            {
                return input.Substring(0, input.IndexOf("@", StringComparison.Ordinal)+1) + "...";
            }
            return input;
        }

        public static string Collapse(this string input)
        {
            if (input.Length > 40)
            {
                return input.Substring(0, 37) + "...";
            }
            return input;
        }
    }
}
