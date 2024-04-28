using System.ComponentModel;
using System.Reflection;

namespace CharacterAPI.Utils
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {

        /// <summary>
        /// 从枚举中获取Description
        /// </summary>
        /// <param name="enumName">需要获取枚举描述的枚举</param>
        /// <returns>描述内容</returns>
        public static string GetDescription(this Enum enumName)
        {
            var description = string.Empty;
            var fieldInfo = enumName.GetType().GetField(enumName.ToString());
            var attributes = GetDescriptAttr(fieldInfo);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            else
            {
                description = enumName.ToString();
            }
            return description;
        }

        /// <summary>
        /// 根据 value 值获取Description
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Type enumType, int? value)
        {
            var key = GetNameAndValue(enumType)?.FirstOrDefault(p => p.Value.Equals(value)).Key;
            return key?.ToString();
        }

        /// <summary>
        /// 根据 枚举字符串名 值获取Description
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDescription(this Type enumType, string name)
        {
            var value = GetNameAndDescriptions(enumType)?.FirstOrDefault(p => p.Key.ToString().Equals(name)).Value;
            return value?.ToString();
        }


        /// <summary>
        /// 获取字段Description
        /// </summary>
        /// <param name="fieldInfo">FieldInfo</param>
        /// <returns>DescriptionAttribute[] </returns>
        private static DescriptionAttribute[] GetDescriptAttr(FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        }

        /// <summary>
        /// 获取枚举所有名称
        /// </summary>
        /// <param name="enumType">枚举类型typeof(T)</param>
        /// <returns>枚举名称列表</returns>
        public static List<string> GetEnumNamesList(this Type enumType)
        {

            return Enum.GetNames(enumType).ToList();
        }

        /// <summary>
        /// 获取所有枚举对应的值
        /// </summary>
        /// <param name="enumType">枚举类型typeof(T)</param>
        /// <returns>枚举值列表</returns>
        public static List<int> GetEnumValuesList(this Type enumType)
        {
            var list = new List<int>();
            foreach (var value in Enum.GetValues(enumType))
            {
                list.Add(Convert.ToInt32(value));
            }
            return list;
        }

        /// <summary>
        /// 获取枚举名以及对应的Description
        /// </summary>
        /// <param name="type">枚举类型typeof(T)</param>
        /// <returns>返回Dictionary  ,Key为枚举名，  Value为枚举对应的Description</returns>
        public static Dictionary<object, object> GetNameAndDescriptions(this Type type)
        {
            if (type.IsEnum)
            {
                var dic = new Dictionary<object, object>();
                var enumValues = Enum.GetValues(type);
                foreach (Enum value in enumValues)
                {
                    dic.Add(value, GetDescription(value));
                }
                return dic;
            }
            return null;
        }

        /// <summary>
        /// 获取枚举名以及对应的Value
        /// </summary>
        /// <param name="type">枚举类型typeof(T)</param>
        /// <returns>返回Dictionary  ,Key为描述名，  Value为枚举对应的值</returns>
        public static Dictionary<object, object> GetNameAndValue(this Type type)
        {
            if (type.IsEnum)
            {
                var dic = new Dictionary<object, object>();
                var enumValues = Enum.GetValues(type);
                foreach (Enum value in enumValues)
                {
                    dic.Add(GetDescription(value), value.GetHashCode());
                }
                return dic;
            }
            return null;
        }
        /// <summary>
        /// 获取枚举名以及对应的Value
        /// </summary>
        /// <param name="type">枚举类型typeof(T)</param>
        /// <returns>返回Dictionary  ,Key为枚举对应的值，  Value为描述名</returns>
        public static Dictionary<object, object> GetValueAndName(this Type type)
        {
            if (type.IsEnum)
            {
                var dic = new Dictionary<object, object>();
                var enumValues = Enum.GetValues(type);
                foreach (Enum value in enumValues)
                {
                    dic.Add(value.GetHashCode(), GetDescription(value));
                }
                return dic;
            }
            return null;
        }
    }
}
