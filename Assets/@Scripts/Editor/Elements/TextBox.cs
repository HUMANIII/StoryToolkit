using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements;

namespace Scripts.Editor.Elements
{
    public class TextBoxElement : PageBuilderElement
    {
        private const string Tag = "TextBox";
        private TextField text;

        public override void SetData(string data)
        {
        }

        public override void GetData(ref StringBuilder builder)
        {
            builder.AppendLine(nameof(TextBoxElement));
            builder.AppendLine(ElementName.value);
            builder.AppendLine(text.value);
        }

        public override void AddElement(PageBuilder pb)
        {
            //기본적인 정보 세팅
            pageBuilder = pb;
            text = GetData<TextField>(Tag);
        }

        public override void ResetElement()
        {
            base.ResetElement();
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
