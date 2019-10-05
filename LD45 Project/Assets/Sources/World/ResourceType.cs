﻿using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

public enum RESOURCE_TYPE
{
    [Description("Iron Ore")]
    IRON_ORE = 0,
    [Description("Steel")]
    STEEL = 1,
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
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if ((_Attribs != null && _Attribs.Count() > 0))
            {
                return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
            }
        }
        return GenericEnum.ToString();
    }
}