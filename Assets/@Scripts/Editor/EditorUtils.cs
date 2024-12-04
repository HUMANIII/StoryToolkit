using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Scripts.Editor
{
    [Serializable]
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
        /// <summary>
        /// 리플렉션으로 모든 자식 클래스를 불러오는 함수
        /// </summary>
        /// <param name="baseType">이 함수의 자식 클래스를 불러옴</param>
        /// <returns>자식클래스들의 List형</returns>
        public static List<Type> CheckDataWithReflection(Type baseType)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // 모든 자식 클래스
            var allDerivedClasses = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                .ToList();

            return allDerivedClasses;
        }
        
        /// <summary>
        /// 입력 값을 특정 문자열을 기준으로 분리해서 List에 담는 함수
        /// </summary>
        /// <param name="input">잘라낼 문자열</param>
        /// <param name="separator">잘라내는 기준이 되는 문자열</param>
        /// <returns></returns>
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

        //아래 함수의 최적화를 위해 캐싱해두는 함수 - 프로그램 시작 이후 해당 주소가 삭제 되는 것은 감지 할 수 없음
        private static readonly HashSet<string> IsBuilt = new();
        //최초 캐싱이 되었는지 확인하는 함수
        private static bool _isBuiltSet = false;
        /// <summary>
        /// 해당 문자열이 어드레서블로 등록 되어 있는지 검사하는 문자열
        /// </summary>
        /// <param name="input">검사할 문자열</param>
        /// <returns>해당 문자열의 주소를 가진 어드레서블 에셋의 존재여부</returns>
        public static bool CheckIsBuilt(this string input)
        {
            //최초 검사 시도시 캐싱 시작
            if (!_isBuiltSet)
            {
                SetIsBuilt();
            }
            
            //해당 문자열이 캐싱되어있는지 확인
            if (IsBuilt.Contains(input))
                return true;
            
            //없다면 해당 문자열이 어드레서블로 등록 되어 있는지 확인 
            if (!Addressables.ResourceLocators.Any(locator => locator.Locate(input, typeof(object), out _)))
                return false;
            
            IsBuilt.Add(input);
            return true;
        }

        /// <summary>
        /// 어드레서블에 등록된 string값을 모두 캐싱함 - 첫 탐색이 너무 느리다면 public으로 수정 이후 첫 로딩때 해버리는것도 좋다고 생각함
        /// </summary>
        private static void SetIsBuilt()
        {
            if(_isBuiltSet)
                return;
            
            _isBuiltSet = true;
            //어드레서블을 순회돌면서 키값을 추가함
            foreach (IResourceLocator locator in Addressables.ResourceLocators)
            {
                foreach (IResourceLocation location in locator.AllLocations)
                {
                    IsBuilt.Add(location.PrimaryKey);
                }
            }
        }
        
        /// <summary>
        /// 해당 경로의 에셋의 어드레서블 주소이름과 원래 탑입을 리턴해주는 함수 
        /// </summary>
        /// <param name="path">확인할 경로</param>
        /// <param name="newName">어드레서블에 등록되어있지 않다면 해당 이름으로 등록시킴</param>
        /// <returns>어드레서블 정보 리턴</returns>
        public static AddressAbleInfo GetAddressableInfo(this string path, string newName = default)
        {
            //리턴용 객체 생성
            AddressAbleInfo rtn = default;
            
            //해당 경로에 있는 에셋이 어드레서블에 등록 되어 있는지 확인 후 이름(주소)를 가져온다.
            AddressableAssetSettings setting = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(path);
            //만약 경로에 에셋이 없다면 빠른 리턴
            if (guid == "")
                return rtn;
            
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
            
            //해당 경로에 있는 에셋의 자료형 을 가져온다
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            rtn.Type = asset.GetType();
            return rtn;
        }
    }
}
