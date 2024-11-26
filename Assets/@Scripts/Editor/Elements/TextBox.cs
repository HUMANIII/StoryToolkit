using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public class TextBoxElement : PageBuilderElement
    {
        private const string Tag = "TextBox";
        public const string ElementNameFormat = "@TextBoxElement: ";
        private TextField text;
    
        public override void SaveData(ref StringBuilder builder)
        {
            builder.Append(ElementNameFormat);
            builder.Append(text.value);
        }

        public override void AddElement(PageBuilder pb)
        {
            //기본적인 정보 세팅
            pageBuilder = pb;
            text = GetData<TextField>(Tag);
        }

        protected override void RemoveElement()
        {
            Element.RemoveFromHierarchy();
            UpdateElements();
        }

        public override void ResetElement()
        {
            text.value = string.Empty;
        }

        public override void ClearElements()
        {
            Element.RemoveFromHierarchy();
        }

        private void UpdateElements()
        {
        }
    }
}
