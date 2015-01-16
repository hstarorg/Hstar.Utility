using System;
using System.Globalization;
namespace Hstar.Utility.Helpers
{
    public class RmbHelper
    {
        /// <summary>
        /// 将指定的金额转换为人民币大写形式
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns>大写形式</returns>
        public string ConvertToAmout(int money)
        {
            return this.ConvertToAmout(Convert.ToDecimal(money));
        }

        /// <summary>
        /// 将指定的金额转换为人民币大写形式
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns>大写形式</returns>
        public string ConvertToAmout(double money)
        {
            return this.ConvertToAmout(Convert.ToDecimal(money));
        }

        /// <summary>
        /// 将指定的金额转换为人民币大写形式
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns>大写形式</returns>
        public string ConvertToAmout(decimal money)
        {
            string text = "零壹贰叁肆伍陆柒捌玖";
            string text2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
            string text3 = "";
            string str = "";
            string str2 = "";
            int num2 = 0;
            money = Math.Round(Math.Abs(money), 2);
            string text4 = ((long)(money * 100m)).ToString(CultureInfo.InvariantCulture);
            int length = text4.Length;
            if (length > 15)
            {
                throw new ArgumentException("该金额超出所能转换的最大值。");
            }
            text2 = text2.Substring(15 - length);
            for (int i = 0; i < length; i++)
            {
                string text5 = text4.Substring(i, 1);
                int startIndex = Convert.ToInt32(text5);
                if (i != length - 3 && i != length - 7 && i != length - 11 && i != length - 15)
                {
                    if (text5 == "0")
                    {
                        str = "";
                        str2 = "";
                        num2++;
                    }
                    else
                    {
                        if (text5 != "0" && num2 != 0)
                        {
                            str = "零" + text.Substring(startIndex, 1);
                            str2 = text2.Substring(i, 1);
                            num2 = 0;
                        }
                        else
                        {
                            str = text.Substring(startIndex, 1);
                            str2 = text2.Substring(i, 1);
                            num2 = 0;
                        }
                    }
                }
                else
                {
                    if (text5 != "0" && num2 != 0)
                    {
                        str = "零" + text.Substring(startIndex, 1);
                        str2 = text2.Substring(i, 1);
                        num2 = 0;
                    }
                    else
                    {
                        if (text5 != "0" && num2 == 0)
                        {
                            str = text.Substring(startIndex, 1);
                            str2 = text2.Substring(i, 1);
                            num2 = 0;
                        }
                        else
                        {
                            if (text5 == "0" && num2 >= 3)
                            {
                                str = "";
                                str2 = "";
                                num2++;
                            }
                            else
                            {
                                if (length >= 11)
                                {
                                    str = "";
                                    num2++;
                                }
                                else
                                {
                                    str = "";
                                    str2 = text2.Substring(i, 1);
                                    num2++;
                                }
                            }
                        }
                    }
                }
                if (i == length - 11 || i == length - 3)
                {
                    str2 = text2.Substring(i, 1);
                }
                text3 = text3 + str + str2;
                if (i == length - 1 && text5 == "0")
                {
                    text3 += '整';
                }
            }
            if (money == 0m)
            {
                text3 = "零元整";
            }
            return text3;
        }
    }
}
