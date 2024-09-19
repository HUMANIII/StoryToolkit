using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PageBuilder : EditorWindow
{
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
    private Image image;
    private Label outputLabel;

    private List<PageBuilderElement> elements = new();
    private List<List<PageBuilderElement>> buildedElements = new();


    [MenuItem("Window/Page Builder")]
    public static void ShowExample()
    {
        PageBuilder wnd = GetWindow<PageBuilder>();
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

        add.clicked += () =>
        {
            foreach(var element in elements)
            {
                
            }
            buildedElements.Add(elements);
            outputLabel.text = string.Format("Book {0} : {1}Page Added", book.value, page.value);
        };
    }
}
