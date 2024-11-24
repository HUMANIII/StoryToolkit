using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public class TextBoxElement : PageBuilderElement
    {
        private const string Tag = "TextBox";
        private const string ElementNameFormat = "TextBox_{0}";
        private static readonly List<TextBoxElement> TextBoxElements = new();
        private TextField text;
    
        public override void SaveData(ref StringBuilder builder)
        {
        }

        public override void AddElement(PageBuilder pb)
        {
            //기본적인 정보 세팅
            pageBuilder = pb;
            text = GetData<TextField>(Tag);
            elementName = string.Format(ElementNameFormat, TextBoxElements.Count);
            text.name = elementName;
            TextBoxElements.Add(this);
        }

        protected override void RemoveElement()
        {
            Element.RemoveFromHierarchy();
            TextBoxElements.Remove(this);
            UpdateElements();
        }

        public override void ResetElement()
        {
            text.value = string.Empty;
        }

        public override void ClearElements()
        {
            Element.RemoveFromHierarchy();
            TextBoxElements.Remove(this);
        }

        private void UpdateElements()
        {
            for (var i = 0; i < TextBoxElements.Count; i++)
            {
                TextBoxElements[i].elementName = string.Format(ElementNameFormat, i);
                TextBoxElements[i].text.name = TextBoxElements[i].elementName;
            }
        }
    }
}
