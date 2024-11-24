using System;
using System.Text;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public abstract class PageBuilderElement : EditorWindow
    {
        private const string DeleteBtn = "DeleteButton";
        //빌더 패턴을 사용해서 다시 구현해보기
        //내가 생각하는것 -> 특정 함수 호출시 매개변수로 들어간 클래스에 변화를 주어서 리턴하는것 ClassA.FucA().FucB().FucC() 이런식으로
        //위에서 FuncA 등등이 각 요소를 추가하는함수이며 각자의 세세한 것들은 각각의 클래스에서 구현하도록 하는것 -> 굳이 그렇게 하지 않고 Builder 자체에서 container를 가지게 하는 것으로 해결함 
        [FormerlySerializedAs("ElementName")] public string elementName;
        [FormerlySerializedAs("ElementPath")] public string elementPath;
        public PageBuilder pageBuilder;
        public bool isGameObject;
        protected VisualElement Element { get; set; }

        /// <summary>
        /// 저장 시 호출
        /// </summary>
        public abstract void SaveData(ref StringBuilder builder);
        /// <summary>
        /// 요소 추가시 호출 - 일반적으로 각 데이터를 저장하기 위해 사용
        /// </summary>
        public abstract void AddElement(PageBuilder pb);
        /// <summary>
        /// 요소 삭제시 호출
        /// </summary>
        protected abstract void RemoveElement();
    
        /// <summary>
        /// 내부 요소들 초기화 할 때 호출
        /// </summary>
        public abstract void ResetElement();
    
        /// <summary>
        /// 모든 요소 삭제할때 호출
        /// </summary>
        public abstract void ClearElements(); 

        protected T GetData<T>(string windowName) where T : VisualElement, new()
        {
            var vt = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(string.Format(ToolKitPath.UXML, windowName));
            vt.CloneTree(rootVisualElement);
            Element = rootVisualElement;
            pageBuilder.ElementsContainer.Add(Element);
            var rtn = Element.Q<T>(windowName);
            if (rtn == null)
            {
                rtn = new T();
                Element.Add(rtn);
            }
            Element.Q<Button>(DeleteBtn).clickable.clicked += RemoveElement;
            return rtn;
        }

        /// <summary>
        /// 드래그 드랍시 경로를 기반으로 한 특정 액션을 튀하게 만듬
        /// </summary>
        /// <param name="element">드랍 시 액션을 취할 요소</param>
        /// <param name="action">경로를 받은 이후에 실행 할 액션</param>
        /// <typeparam name="T">드래그된 요소의 자료형 필터링</typeparam>
        protected void RegisterDragDropOption<T>(VisualElement element, Action<string> action)
        {
            element.RegisterCallback<DragEnterEvent>(_ => DragAndDrop.AcceptDrag());
            element.RegisterCallback<DragUpdatedEvent>(_ =>
            {
                if (DragAndDrop.objectReferences.Length > 0 && DragAndDrop.objectReferences[0] is T)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                }
            });
            element.RegisterCallback<DragPerformEvent>(_ =>
            {
                if (DragAndDrop.objectReferences.Length > 0 && DragAndDrop.objectReferences[0] is T)
                {
                    var path = AssetDatabase.GetAssetPath(DragAndDrop.objectReferences[0]);
                    action(path);
                }
            });
        }
    }
}
