using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

public class ImageElement : PageBuilderElement
{
    private const string TAG = "Image";
    private const string ElementNameFormat = "Image_{0}";
    private static List<ImageElement> imgElements = new();
    private Image img;

    public override void AddElement(PageBuilder builder)
    {
        pageBuilder = builder;
        img = GetData<Image>(TAG);
        ElementName = string.Format(ElementNameFormat, imgElements.Count);
        img.name = ElementName;
        img.style.backgroundColor = Color.white;
        img.style.width = 100;
        img.style.height = 100;
        imgElements.Add(this);
    }

    public override void RemoveElement()
    {
        element.RemoveFromHierarchy();
        imgElements.Remove(this);
        UpdateElements();
    }

    public override void ResetElement()
    {
        element.RemoveFromHierarchy();
        imgElements.Remove(this);
    }

    public override void SaveData()
    {
        throw new System.NotImplementedException();
    }

    private void UpdateElements()
    {
        for (var i = 0; i < imgElements.Count; i++)
        {
            imgElements[i].ElementName = string.Format(ElementNameFormat, i);
            imgElements[i].img.name = imgElements[i].ElementName;
        }
    }

    public void ChangeImage(string path)
    {
        ElementPath = path;
        img.image = AssetDatabase.LoadAssetAtPath<Texture2D>(ElementPath);
    }
}
