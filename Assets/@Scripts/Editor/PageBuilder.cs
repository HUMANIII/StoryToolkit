using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PageBuilder :EditorWindow
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


        kor = rootVisualElement.Q<TextField>(TextKor);
        eng = rootVisualElement.Q<TextField>(TextEng);
        book = rootVisualElement.Q<TextField>(BookName);
        page = rootVisualElement.Q<IntegerField>(PageNum);
        add = rootVisualElement.Q<Button>(BtnAdd);
        save = rootVisualElement.Q<Button>(BtnSave);
        buildedPage = rootVisualElement.Q<ListView>(BuildedPage);
        image = rootVisualElement.Q<Image>(PageImage);
        outputLabel = rootVisualElement.Q<Label>(OutputLabel);

        add.clicked += () =>
        {
            outputLabel.text = string.Format("Book {0} : {1}Page Added", book.value, page.value);
        };
    }
}
