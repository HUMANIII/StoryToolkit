using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Scripts.Editor.Elements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Editor
{
    public class PageBuilder : EditorWindow
    {
        public VisualElement ElementsContainer;
    
        private const string Container = "Conainter";
        private const string WindowName = "PageBuilder";
        private const string PackageName = "PackageName";
        private const string BtnAdd = "Add";       
        private const string BtnSave = "Save";     
        private const string BtnClear = "Clear";     
        private const string BuiltPage = "BuildedPage";
        private const string OutputLabel = "OutputLabel";
        private const string Options = "Options";
        private const string ChooseOptions = "추가할 컴포넌트를 선택하세요.";
    
        private const string DirLastPageSetting = "./PageBuilder/LastPageSetting.txt";
        private const string DirPagesInfo = "./PageBuilder/PagesInfo.txt";
        private const string ElementEndFormat = "§§§"; 
        private const string PageEndFormat = "§§"; 
        private const string PackageEndFormat = "§"; 
        
        private StringBuilder sb = new StringBuilder();
    
    
        private IntegerField packageNum;
        private Button add;
        private Button save;
        private Button clear;
        private ListView builtPage;
        private ImageElement image;
        private Label outputLabel;
        private DropdownField options;
        private TextField packageName;

        private readonly Dictionary<string,Type> types = new();
        private readonly List<PageBuilderElement> elements = new();
        private readonly List<List<PageBuilderElement>> builtElements = new();
        private int curPage;
        
        private LinkedList<string> pageElements = new();


        [MenuItem("Page Builder/Page Builder")]
        public static void ShowExample()
        {
            var wnd = GetWindow<PageBuilder>();
            wnd.titleContent = new GUIContent("Page Builder");
        }

        public void CreateGUI()
        {
            var vt = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(string.Format(ToolKitPath.UXML, WindowName));
            vt.CloneTree(rootVisualElement);
            //var ss = AssetDatabase.LoadAssetAtPath<StyleSheet>(string.Format(ToolKitPath.USS, windowName));
            //rootVisualElement.styleSheets.Add(ss);

            add = rootVisualElement.Q<Button>(BtnAdd);
            save = rootVisualElement.Q<Button>(BtnSave);
            clear = rootVisualElement.Q<Button>(BtnClear);
            builtPage = rootVisualElement.Q<ListView>(BuiltPage);
            outputLabel = rootVisualElement.Q<Label>(OutputLabel);
            ElementsContainer = rootVisualElement.Q<VisualElement>(Container);
            packageName = rootVisualElement.Q<TextField>(PackageName);
        
            options = rootVisualElement.Q<DropdownField>(Options);
            SetDropDown();
            LoadCurPackageSetting();
            LoadAllData();
            
            clear.clicked += ClearPage;
            add.clicked += AddData;
            save.clicked += SaveAllData;
        }

        public void OnDestroy()
        {
            SaveCurPackageSetting();
            ClearPage();
        }

        public void ResetPage()
        {
            foreach (var element in elements)
            {
                element.ResetElement();
            }
        }

        private void ClearPage()
        {
            foreach (var element in elements)
            {
                element.ClearElements();
            }
            elements.Clear();
        }

        private void AddElement(string elementName)
        {
            if (elementName == ChooseOptions)
                return;
        
            if (!types.TryGetValue(elementName, out var type))
            {
                Debug.LogError("Type not found: " + elementName);
                return;
            }
            PageBuilderElement element = CreateInstance(type) as PageBuilderElement;
            if (element == null)
            {
                Debug.LogError(elementName + " could not be instantiated.");
                return;
            }
            elements.Add(element);
            element.AddElement(this);
        }

        private void AddData()
        {
            sb.Clear();
            sb.AppendLine(packageName.value);
            
            foreach(var element in elements)
            {
                element.GetData(ref sb);
                sb.AppendLine(ElementEndFormat);
            }
            pageElements.AddLast(sb.ToString());
            Debug.Log("data added");
            ResetPage();
        }

        private void SetDropDown()
        {
            //페이지 빌더 엘리먼트들의 상속을 받은 것들을 찾음
            List<Type> allTypes = EditorUtils.CheckDataWithReflection(typeof(PageBuilderElement));
        
            //초기화 후 각 타입과 이름을 할당함
            List<string> typeNames = new();
            types.Clear();
            foreach (Type type in allTypes)
            {
                types.Add(type.Name, type);
                typeNames.Add(type.Name);
            }
        
            //찾은 모든 요소들의 이름을 드롭다운에 추가
            options.choices = typeNames;
            //드롭다운에 안내로 쓸 드롭다운 요소 추가
            options.choices.Add(ChooseOptions);
            options.value = ChooseOptions;
        
            //드롭다운 클릭시 안내요소 삭제
            options.RegisterCallback<FocusEvent>(_ =>
            {
                options.choices.Remove(ChooseOptions);
            });
        
            //드롭다운 클릭시 요소 생성하는 이벤트 추가
            options.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue != ChooseOptions)
                {
                    AddElement(evt.newValue);
                
                    //드롭다운 클릭이 끝난 뒤 안내 요소 생성 이후 다시 안내하도록 만들기
                    options.choices.Add(ChooseOptions);
                    options.value = ChooseOptions;
                }
                else
                {
                    //FocusEvent를 다시 처리하기 위한 블러처리
                    options.Blur();
                }
            });
        }

        private void AddPackageSetting()
        {
            builtElements.Add(elements);
            curPage = builtElements.Count;
        }

        private void SelectPackageSetting(int pageNum)
        {
            curPage = pageNum;
            ClearPage();
            foreach (var item in builtElements[curPage])
            {
                elements.Add(item);
            }
        }

        public void RemoveElement(PageBuilderElement element)
        {
            elements.Remove(element);
            element.Element.RemoveFromHierarchy();
        }

        private void SaveCurPackageSetting()
        {
            sb.Clear();
            try
            {
                foreach (var element in elements)
                {
                    sb.Append("\t" + element.GetType().Name);
                }
                Debug.unityLogger.Log(sb + " is Saved");
                string dirName = Path.GetDirectoryName(DirLastPageSetting);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName ?? string.Empty);
                }
                File.WriteAllText(DirLastPageSetting, sb.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private void LoadCurPackageSetting()
        {
            string dirName = Path.GetDirectoryName(DirLastPageSetting);
            try
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName ?? string.Empty);
                }
                if (!File.Exists(DirLastPageSetting))
                {
                    File.WriteAllText(DirLastPageSetting, string.Empty);
                }

                foreach (string line in File.ReadAllLines(DirLastPageSetting))
                {
                    foreach (string elementsName in EditorUtils.GetStringBeforeSeparator(line))
                    {
                        if(elementsName == string.Empty)
                            continue;
                        AddElement(elementsName);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SaveAllData()
        {
            sb.Clear();
            try
            {
                foreach (var element in pageElements)
                {
                    sb.AppendLine(element);
                    sb.AppendLine(PageEndFormat);
                }
                Debug.unityLogger.Log(sb + " is Saved");
                string dirName = Path.GetDirectoryName(DirPagesInfo);
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName ?? string.Empty);
                }
                File.WriteAllText(DirPagesInfo, sb.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void LoadAllData()
        {
            string dirName = Path.GetDirectoryName(DirPagesInfo);
            try
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName ?? string.Empty);
                }
                if (!File.Exists(DirPagesInfo))
                {
                    File.WriteAllText(DirPagesInfo, string.Empty);
                    return;
                }
                sb.Clear();
                foreach (string line in File.ReadLines(DirPagesInfo))
                {
                    sb.AppendLine(line);
                    if (line != PageEndFormat) 
                        continue;
                    
                    pageElements.AddFirst(sb.ToString());
                    sb.Clear();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
