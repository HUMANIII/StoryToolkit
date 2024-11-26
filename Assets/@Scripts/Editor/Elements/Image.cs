using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public class ImageElement : PageBuilderElement
    {
        private const string Tag = "Image";
        public const string ElementNameFormat = "@Image: ";
        private VisualElement img;

        public override void AddElement(PageBuilder builder)
        {
            //기본적인 정보 세팅
            pageBuilder = builder;
            img = GetData<VisualElement>(Tag);
        
            //드래그  드랍 옵션 추가
            RegisterDragDropOption<Texture2D>(img, ChangeImage);
        }

        protected override void RemoveElement()
        {
            Element.RemoveFromHierarchy();
            UpdateElements();
        }

        public override void ResetElement()
        {
            img.style.backgroundImage = null;
        }

        public override void ClearElements()
        {
            Element.RemoveFromHierarchy();
        }

        public override void SaveData(ref StringBuilder builder)
        {
            builder.Append(ElementNameFormat);
            string data = elementPath.GetAddressableInfo().ToString();
            builder.Append(data);
        }

        private void UpdateElements()
        {
        }

        private void ChangeImage(string path)
        {
            elementPath = path;
            img.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(elementPath);
        }
    }
}
