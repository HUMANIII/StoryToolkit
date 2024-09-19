using System;
using UnityEngine;
using static PageBuilderElement;

public class ToolKitPath
{
    public const string UXML = "Assets/UIToolKit/UXML/{0}.uxml";
    public const string USS = "Assets/UIToolKit/USS/{0}.uss";
    public const string SavePath = "Assets/Data/{0}/{1}.asset";
}

public static class Extensions
{
    public static PageBuilderElement getBuilderElement(this string ClassName)
    {
        var type = Type.GetType(ClassName);
        if(type == null)
            return null;        
        return (PageBuilderElement)Activator.CreateInstance(type);
    }
}