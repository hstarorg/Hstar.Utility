using System;
using System.Text;

namespace Hstar.Utility.Helpers
{
    public class RmbHelper
    {
        #region  常量定义

        /// <summary>
        /// 亿，万万
        /// </summary>
        private const int HUNDRED_MILLION = 100000000;

        /// <summary>
        /// 万，十千
        /// </summary>
        private const int TEN_THOUSAND = 10000;

        private const int THOUSAND = 1000;

        private const int HUNDRED = 100;

        private const int TEN = 10;

        /// <summary>
        /// 万亿(不包含)
        /// </summary>
        private const double MAX_MONEY = 1000000000000;

        /// <summary>
        /// 金额不能为负，最小值为0.
        /// </summary>
        private const double MIN_MONEY = 0;

        private const string AMOUT_STRING = "零壹贰叁肆伍陆柒捌玖";

        #endregion

        #region 构造函数
        /// <summary>
        /// 将指定的金额转换为人民币大写形式
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns>大写形式</returns>
        public string ConvertToAmout(int money)
        {
            return this.ConvertToAmout(Convert.ToDouble(money));
        }

        public string ConvertToAmout(float money)
        {
            return this.ConvertToAmout(Convert.ToDouble(money));
        }

        public string ConvertToAmout(decimal money)
        {
            return this.ConvertToAmout(Convert.ToDouble(money));
        }

        public string ConvertToAmout(double money)
        {
            return this.GetMoneyAmout(money);
        }

        #endregion

        #region 工具库

        /// <summary>
        /// 金额转换为中文大写
        /// </summary>
        /// <param name="money">金额</param>
        /// <returns></returns>
        private string GetMoneyAmout(double money)
        {
            var rightMoney = GetMoneyFormat(money);
            //整数部分
            var longPart = (long)rightMoney;
            //小数部分
            var decimalPart =Math.Round(rightMoney - longPart,2);

            var longPartAmout = this.ConvertLongPartToAmout(longPart);
            var decimalPartAmout = this.ConvertDecimalPartToAmout(decimalPart);

            return longPartAmout + (string.IsNullOrEmpty(decimalPartAmout) ? "整" : decimalPartAmout);
        }

        /// <summary>
        /// 验证金额格式，同时输出保留2位小数的金额
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        private double GetMoneyFormat(double money)
        {
            var d = double.Parse(money.ToString("F2"));
            if (d > MAX_MONEY || d < MIN_MONEY)
            {
                throw new ArgumentOutOfRangeException("Money must greater than or equal 0 and less than 1000,000,000,000");
            }
            return d;
        }

        /// <summary>
        /// 转换整数部分到中文大写
        /// </summary>
        /// <param name="longPart"></param>
        /// <returns></returns>
        private string ConvertLongPartToAmout(long longPart)
        {
            if (longPart == 0)
            {
                return "零元";
            }
            var amoutBuilder = new StringBuilder();
            while (longPart > 0)
            {
                if (longPart > HUNDRED_MILLION)
                {
                    amoutBuilder.AppendFormat("{0}亿", this.NumToChinese((int)longPart / HUNDRED_MILLION));
                    longPart %= HUNDRED_MILLION;
                }
                else if (longPart > TEN_THOUSAND)
                {
                    amoutBuilder.AppendFormat("{0}万", this.NumToChinese((int)longPart / TEN_THOUSAND));
                    longPart %= TEN_THOUSAND;
                }
                else
                {
                    amoutBuilder.AppendFormat("{0}", this.NumToChinese((int)longPart));
                    longPart = 0;
                }
            }
            amoutBuilder.Append("元");
            return amoutBuilder.ToString();
        }

        /// <summary>
        /// 将10000以内的金额转换为中文大写
        /// </summary>
        /// <param name="num">10000以内的数字</param>
        /// <returns></returns>
        private string NumToChinese(int num)
        {
            var builder = new StringBuilder();
            bool useZero = false;
            while (num > 0)
            {
                if (num >= THOUSAND)
                {
                    builder.AppendFormat("{0}仟", AMOUT_STRING[num / THOUSAND]);
                    num %= THOUSAND;
                    useZero = true;
                }
                else if (num >= HUNDRED)
                {
                    builder.AppendFormat("{0}佰", AMOUT_STRING[num / HUNDRED]);
                    num %= HUNDRED;
                    useZero = false;
                }
                else if (num >= TEN)
                {
                    if (useZero)
                    {
                        builder.Append(AMOUT_STRING[0]);
                    }
                    builder.AppendFormat("{0}拾", AMOUT_STRING[num / TEN]);
                    num %= TEN;
                }
                else
                {
                    builder.Append(AMOUT_STRING[num]);
                    num = 0;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 转换小数部分
        /// </summary>
        /// <param name="decimalPart">小数部分的值</param>
        /// <returns></returns>
        private string ConvertDecimalPartToAmout(double decimalPart)
        {
            var builder = new StringBuilder();
            //先优先处理为整数
            decimalPart *= 100;
            if (decimalPart >= 10)
            {
                builder.AppendFormat("{0}角", AMOUT_STRING[(int)decimalPart / 10]);
                decimalPart %= 10;
            }

            if (decimalPart > 0)
            {
                builder.AppendFormat("{0}分", AMOUT_STRING[(int)decimalPart]);
            }
            return builder.ToString();
        }

        #endregion
    }
}
