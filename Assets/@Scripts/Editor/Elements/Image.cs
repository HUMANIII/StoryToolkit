using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

public class ImageElement : PageBuilderElement
{
    public const string ElementNameFormat = "Image_{0}";
    private static List<ImageElement> imgElements = new();
    private Image img;

    public override void AddElement(PageBuilder builder)
    {
        ElementName = string.Format(ElementNameFormat, imgElements.Count);
        img = new()
        {
            name = ElementName,
            style =
            {
                backgroundColor = Color.white,
                width = 100,
                height = 100
            }
        };
        pageBuilder = builder;
        pageBuilder.RootElement.Add(img);
        imgElements.Add(this);
    }

    public override void RemoveElement()
    {
        img.RemoveFromHierarchy();
        imgElements.Remove(this);
        pageBuilder.RootElement.Remove(img);
        
        UpdateElements();
    }

    public override void Reset()
    {
        img.RemoveFromHierarchy();
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
