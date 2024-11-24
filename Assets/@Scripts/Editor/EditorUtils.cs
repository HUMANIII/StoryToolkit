using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scripts.Editor
{
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
        
        public static List<string> GetStringBeforeSeparator(string input, string separator = "\t")
        {
            List<string> resultList = new List<string>();

            while (input.Contains(separator))
            {
                // separator가 나오는 위치까지 잘라서 리스트에 추가
                int separatorIndex = input.IndexOf(separator, StringComparison.Ordinal);
                string substring = input.Substring(0, separatorIndex);
                resultList.Add(substring);

                // separator를 삭제하고 나머지 문자열로 갱신
                input = input.Substring(separatorIndex + separator.Length);
            }

            // 마지막 남은 문자열도 리스트에 추가 (separator 뒤에 남은 내용)
            if (input.Length > 0)
            {
                resultList.Add(input);
            }

            return resultList;
        }
    }
    

}
