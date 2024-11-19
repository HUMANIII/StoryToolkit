using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;

public class TextBoxElement : PageBuilderElement
{
    private const string TAG = "TextBox";
    private const string ElementNameFormat = "TextBox_{0}";
    private static List<TextBoxElement> TextBoxElements = new();
    private TextField text;
    
    public override void SaveData()
    {
    }

    public override void AddElement(PageBuilder pb)
    {
        //기본적인 정보 세팅
        pageBuilder = pb;
        text = GetData<TextField>(TAG);
        ElementName = string.Format(ElementNameFormat, TextBoxElements.Count);
        text.name = ElementName;
        TextBoxElements.Add(this);
    }

    public override void RemoveElement()
    {
        element.RemoveFromHierarchy();
        TextBoxElements.Remove(this);
        UpdateElements();
    }

    public override void ResetElement()
    {
        element.RemoveFromHierarchy();
        TextBoxElements.Remove(this);
    }
    
    private void UpdateElements()
    {
        for (var i = 0; i < TextBoxElements.Count; i++)
        {
            TextBoxElements[i].ElementName = string.Format(ElementNameFormat, i);
            TextBoxElements[i].text.name = TextBoxElements[i].ElementName;
        }
    }
}
