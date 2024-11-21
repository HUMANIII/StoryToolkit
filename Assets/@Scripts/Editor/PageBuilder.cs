using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public class PageBuilder : EditorWindow
{
    public VisualElement ElementsContainer;
    
    private const string Conainter = "Conainter";
    private const string windowName = "PageBuilder";
    private const string Image = "Page Image";
    private const string TextKor = "TextKor";
    private const string BookName = "BookName";
    private const string PageNum = "PageNum";
    private const string TextEng = "TextEng";
    private const string BtnAdd = "Add";       
    private const string BtnSave = "Save";      
    private const string BuildedPage = "BuildedPage";
    private const string PageImage = "PageImage";
    private const string OutputLabel = "OutputLabel";
    private const string Options = "Options";
    private const string ChooseOptions = "추가할 컴포넌트를 선택하세요."; 

    private TextField kor;
    private TextField eng;
    private TextField book;
    private IntegerField page;
    private Button add;
    private Button save;
    private ListView buildedPage;
    private ImageElement image;
    private Label outputLabel;
    private DropdownField options;

    private Dictionary<string,Type> types = new();
    private List<PageBuilderElement> elements = new();
    private List<List<PageBuilderElement>> buildedElements = new();


    [MenuItem("Page Builder/Page Builder")]
    public static void ShowExample()
    {
        var wnd = GetWindow<PageBuilder>();
        wnd.titleContent = new GUIContent("Page Builder");
    }

    public void CreateGUI()
    {
        // UXML ���ø��� �ҷ���
        var vt = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(string.Format(ToolKitPath.UXML, windowName));
        vt.CloneTree(rootVisualElement);
        // USS ��Ÿ�� ��Ʈ ���� (���� ����)
        //var ss = AssetDatabase.LoadAssetAtPath<StyleSheet>(string.Format(ToolKitPath.USS, windowName));
        //rootVisualElement.styleSheets.Add(ss);

        page = rootVisualElement.Q<IntegerField>(PageNum);
        add = rootVisualElement.Q<Button>(BtnAdd);
        save = rootVisualElement.Q<Button>(BtnSave);
        buildedPage = rootVisualElement.Q<ListView>(BuildedPage);
        outputLabel = rootVisualElement.Q<Label>(OutputLabel);
        ElementsContainer = rootVisualElement.Q<VisualElement>(Conainter);
        
        options = rootVisualElement.Q<DropdownField>(Options);
        SetDropDown();




        add.clicked += () =>
        {
            // foreach(var element in elements)
            // {
            //     
            // }
            // buildedElements.Add(elements);
            // outputLabel.text = string.Format("Book {0} : {1}Page Added", book.value, page.value);
        };
    }

    public void ResetPage()
    {
        foreach (var element in elements)
        {
            element.ResetElement();
        }
        elements.Clear();
    }

    public void AddElement(string elementName)
    {
        if (elementName == ChooseOptions)
            return;
        
        if (!types.ContainsKey(elementName))
        {
            Debug.LogError("Type not found: " + elementName);
            return;
        }
        PageBuilderElement element = CreateInstance(types[elementName]) as PageBuilderElement;
        //PageBuilderElement element = Activator.CreateInstance(types[elementName]) as PageBuilderElement;
        if (element == null)
        {
            Debug.LogError(elementName + " could not be instantiated.");
            return;
        }
        elements.Add(element);
        element.AddElement(this);
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
        options.RegisterCallback<FocusEvent>(evt =>
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
}
