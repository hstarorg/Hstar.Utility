using System;

namespace Hstar.Utility.Extensions
{
    public static class ObjectExtention
    {
        public static T To<T>(this object obj) 
        {
            Type conversionType = typeof(T);
            if (obj == null)
            {
                return default(T);
            }
//            if (conversionType.IsNullableType())
//            {
//                conversionType = conversionType.GetUnNullableType();
//            }
            if (conversionType.IsEnum)
            {
                return (T)Enum.Parse(conversionType, obj.ToString());
            }
            if (conversionType == typeof(Guid))
            {
                return (T)(object)Guid.Parse(obj.ToString());
            }
            return (T)Convert.ChangeType(obj, conversionType);
        }
    }
}
