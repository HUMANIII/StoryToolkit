using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomEditorWindow : EditorWindow
{
    [MenuItem("Window/Custom UI Toolkit Editor")]
    public static void ShowExample()
    {
        CustomEditorWindow wnd = GetWindow<CustomEditorWindow>();
        wnd.titleContent = new GUIContent("UI Toolkit Editor");
    }

    public void CreateGUI()
    {
        // UXML 템플릿을 불러옴
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/EditorWindow.uxml");
        visualTree.CloneTree(rootVisualElement);

        // USS 스타일 시트 적용 (선택 사항)
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/EditorWindow.uss");
        //rootVisualElement.styleSheets.Add(styleSheet);

        // UI 요소 가져오기
        var inputField = rootVisualElement.Q<TextField>("inputField");
        var submitButton = rootVisualElement.Q<Button>("submitButton");
        var outputLabel = rootVisualElement.Q<Label>("outputLabel");

        // 버튼 클릭 이벤트 처리
        submitButton.clicked += () =>
        {
            outputLabel.text = "You entered: " + inputField.value;
        };
    }
}
