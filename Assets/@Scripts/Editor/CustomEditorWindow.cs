using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Window/Custom UI Toolkit Editor")]
    public static void ShowExample()
    {
        var wnd = GetWindow<CustomEditorWindow>();
        wnd.titleContent = new GUIContent("UI Toolkit Editor");
    }

    public void CreateGUI()
    {
        // UXML ���ø��� �ҷ���
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/EditorWindow.uxml");
        visualTree.CloneTree(rootVisualElement);

        // USS ��Ÿ�� ��Ʈ ���� (���� ����)
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/EditorWindow.uss");
        //rootVisualElement.styleSheets.Add(styleSheet);

        // UI ��� ��������
        var inputField = rootVisualElement.Q<TextField>("inputField");
        var submitButton = rootVisualElement.Q<Button>("submitButton");
        var outputLabel = rootVisualElement.Q<Label>("outputLabel");

        // ��ư Ŭ�� �̺�Ʈ ó��
        submitButton.clicked += () =>
        {
            outputLabel.text = "You entered: " + inputField.value;
        };
    }
}
