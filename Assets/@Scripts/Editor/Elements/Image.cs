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
        private const string ElementNameFormat = "Image_{0}";
        private static readonly List<ImageElement> ImgElements = new();
        private VisualElement img;

        public override void AddElement(PageBuilder builder)
        {
            //기본적인 정보 세팅
            pageBuilder = builder;
            img = GetData<VisualElement>(Tag);
            elementName = string.Format(ElementNameFormat, ImgElements.Count);
            img.name = elementName;
            ImgElements.Add(this);
        
            //드래그  드랍 옵션 추가
            RegisterDragDropOption<Texture2D>(img, ChangeImage);
        }

        protected override void RemoveElement()
        {
            Element.RemoveFromHierarchy();
            ImgElements.Remove(this);
            UpdateElements();
        }

        public override void ResetElement()
        {
            img.style.backgroundImage = null;
        }

        public override void ClearElements()
        {
            Element.RemoveFromHierarchy();
            ImgElements.Remove(this);
        }

        public override void SaveData(ref StringBuilder builder)
        {
        }

        private void UpdateElements()
        {
            for (var i = 0; i < ImgElements.Count; i++)
            {
                ImgElements[i].elementName = string.Format(ElementNameFormat, i);
                ImgElements[i].img.name = ImgElements[i].elementName;
            }
        }

        private void ChangeImage(string path)
        {
            elementPath = path;
            img.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(elementPath);
        }
    }
}
