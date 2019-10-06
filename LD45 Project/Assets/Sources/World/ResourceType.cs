using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

public enum RESOURCE_TYPE
{
    [Description("Ore")]
    [BasePrice(5f)]
    ORE = 0,
    [Description("Metal")]
    [BasePrice(15f)]
    METAL = 1,
    [BasePrice(2.5f)]
    [Description("Ice")]
    ICE = 2
}

public static class ResourceTypeExtension
{
    public static string GetDescription(this Enum GenericEnum)
    {
        Type genericEnumType = GenericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
        if ((memberInfo != null && memberInfo.Length > 0))
        {
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((_Attribs != null && _Attribs.Count() > 0))
            {
                return ((DescriptionAttribute)_Attribs.ElementAt(0)).Description;
            }
        }
        return GenericEnum.ToString();
    }

    public static float GetBasePrice(this Enum GenericEnum)
    {
        Type genericEnumType = GenericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
        if ((memberInfo != null && memberInfo.Length > 0))
        {
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(BasePriceAttribute), false);
            if ((_Attribs != null && _Attribs.Count() > 0))
            {
                return ((BasePriceAttribute)_Attribs.ElementAt(0)).Price;
            }
        }
        return 1f;
    }
}

public class BasePriceAttribute : Attribute
{
    public BasePriceAttribute(float price)
    {
        Price = price;
    }

    public float Price { get; private set; }
}