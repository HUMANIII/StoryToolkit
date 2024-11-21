using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class EditorUtils
{
    public static List<Type> CheckDataWithReflection(Type baseType)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        // 모든 자식 클래스
        var allDerivedClasses = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
            .ToList();

        return allDerivedClasses;
    }
}
