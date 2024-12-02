using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public class ImageElement : PageBuilderElement
    {
        private const string Tag = "Image";
        private VisualElement img;

        public override void AddElement(PageBuilder builder)
        {
            //기본적인 정보 세팅
            pageBuilder = builder;
            img = GetData<VisualElement>(Tag);
        
            //드래그  드랍 옵션 추가
            RegisterDragDropOption<Texture2D>(img, ChangeImage);
        }

        public override void ResetElement()
        {
            img.style.backgroundImage = null;
        }

        public override void ClearElements()
        {
            Element.RemoveFromHierarchy();
        }

        public override void SetData(string data)
        {
        }

        public override void GetData(ref StringBuilder builder)
        {
            builder.AppendLine(nameof(ImageElement));
            string data = elementPath.GetAddressableInfo(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)).ToString();
            builder.AppendLine(data);
        }

        private void UpdateElements()
        {
        }

        private void ChangeImage(string path)
        {
            elementPath = path;
            Debug.Log(elementPath);
            img.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(elementPath);
        }
    }
}
