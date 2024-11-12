using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PageBuilder : EditorWindow
{
    public VisualElement RootElement;
    
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

    private TextField kor;
    private TextField eng;
    private TextField book;
    private IntegerField page;
    private Button add;
    private Button save;
    private ListView buildedPage;
    private ImageElement image;
    private Label outputLabel;

    private List<PageBuilderElement> elements = new();
    private List<List<PageBuilderElement>> buildedElements = new();


    [MenuItem("Window/Page Builder")]
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
        RootElement = rootVisualElement;
        // USS ��Ÿ�� ��Ʈ ���� (���� ����)
        //var ss = AssetDatabase.LoadAssetAtPath<StyleSheet>(string.Format(ToolKitPath.USS, windowName));
        //rootVisualElement.styleSheets.Add(ss);

        page = RootElement.Q<IntegerField>(PageNum);
        add = RootElement.Q<Button>(BtnAdd);
        save = RootElement.Q<Button>(BtnSave);
        buildedPage = RootElement.Q<ListView>(BuildedPage);
        outputLabel = RootElement.Q<Label>(OutputLabel);

        add.clicked += () =>
        {
            AddElement(new ImageElement());
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
            element.Reset();
        }
        elements.Clear();
    }

    public void AddElement(PageBuilderElement element)
    {
        elements.Add(element);
        element.AddElement(this);
    }
}
