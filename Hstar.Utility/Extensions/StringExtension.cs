using System.Text;
using System.Text.RegularExpressions;
using Hstar.Utility.Cryptography;

namespace Hstar.Utility.Extensions
{
    public static class StringExtension
    {
        #region RSA 加解密扩展

        public static string RSAEncryptWithPubKey(this string str, string publicKey, Encoding encoding = null)
        {
            var rsa = new RSAEncryption();
            return rsa.EncryptWithPubKey(str, publicKey, encoding);
        }

        public static string RSAEncryptWithPriKey(this string str, string privateKey, Encoding encoding = null)
        {
            var rsa = new RSAEncryption();
            return rsa.EncryptWithPriKey(str, privateKey, encoding);
        }

        public static string RSADecryptWithPubKey(this string str, string publicKey, Encoding encoding = null)
        {
            var rsa = new RSAEncryption();
            return rsa.DecryptWithPubKey(str, publicKey, encoding);
        }

        public static string RSADecryptWithPriKey(this string str, string privateKey, Encoding encoding = null)
        {
            var rsa = new RSAEncryption();
            return rsa.DecryptWithPriKey(str, privateKey, encoding);
        }

        #endregion

        #region 对称加解密扩展

        public static string SymmetricEncrypt(this string str, string key, string keyVector = null, SymmetricAlgorithmType saType = SymmetricAlgorithmType.AES, Encoding encoding = null)
        {
            var sa = new SymmetricEncryption(key, keyVector ?? key, saType);
            return sa.Encrypt(str, encoding);
        }

        public static string SymmetricDecrypt(this string str, string key, string keyVector = null, SymmetricAlgorithmType saType = SymmetricAlgorithmType.AES, Encoding encoding = null)
        {
            var sa = new SymmetricEncryption(key, keyVector ?? key, saType);
            return sa.Decrypt(str, encoding);
        }
        #endregion

        #region Hash加密扩展

        public static string HashEncrypt(this string str, HashAlgorithmType haType = HashAlgorithmType.MD5, Encoding encoding = null)
        {
            var he = new HashEncryption(haType);
            return he.Encrypt(str, encoding);
        }

        #endregion

        #region 数据格式验证

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern = @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern = @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否身份证号，验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string value)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            string pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }
        #endregion
    }
}
