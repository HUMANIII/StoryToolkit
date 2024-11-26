using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Scripts.Editor
{
    public struct AddressAbleInfo
    {
        //실제 값의 타입
        public Type Type;
        //주소의 이름
        public string Name;

        public override string ToString()
        {
            return $"@type : {Type} @name : {Name}";
        }
    }   
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
                string substring = input[..separatorIndex];
                resultList.Add(substring);

                // separator를 삭제하고 나머지 문자열로 갱신
                input = input[(separatorIndex + separator.Length)..];
            }

            // 마지막 남은 문자열도 리스트에 추가 (separator 뒤에 남은 내용)
            if (input.Length > 0)
            {
                resultList.Add(input);
            }

            return resultList;
        }

        public static AddressAbleInfo GetAddressableInfo(this string path, string newName = default)
        {
            //리턴용 객체 생성
            AddressAbleInfo rtn = new();
            
            //해당 경로에 있는 에셋의 자료형 을 가져온다
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            rtn.Type = asset.GetType();
            
            //해당 경로에 있는 에셋이 어드레서블에 등록 되어 있는지 확인 후 이름(주소)를 가져온다.
            AddressableAssetSettings setting = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(path);
            AddressableAssetEntry entry = setting.FindAssetEntry(guid);
            if (entry != null)
            {
                rtn.Name = entry.address;
            }
            //등록되지 않은 에셋이라면 기본 그룹에 등록시킴
            else if(newName != default)
            {
                AddressableAssetGroup group = setting.DefaultGroup;
                AddressableAssetEntry newEntry = setting.CreateOrMoveEntry(guid, group);
                newEntry.address = newName;
                rtn.Name = newEntry.address;
            }
            return rtn;
        }
    }
}
