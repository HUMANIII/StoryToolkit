using UnityEngine;

public class Image : PageBuilderElement
{
    public const string ElementNameFormat = "Image_{0}";
    public static int count;

    public override void AddElement()
    {
        ElementName = string.Format(ElementNameFormat, count);
    }

    public override void RemoveElement()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        count = 0;
    }

    public override void SaveData()
    {
        throw new System.NotImplementedException();
    }
}
