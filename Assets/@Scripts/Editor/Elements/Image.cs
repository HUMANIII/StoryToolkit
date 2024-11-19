using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

public class ImageElement : PageBuilderElement
{
    private const string TAG = "Image";
    private const string ElementNameFormat = "Image_{0}";
    private static List<ImageElement> imgElements = new();
    private VisualElement img;

    public override void AddElement(PageBuilder builder)
    {
        //기본적인 정보 세팅
        pageBuilder = builder;
        img = GetData<VisualElement>(TAG);
        ElementName = string.Format(ElementNameFormat, imgElements.Count);
        img.name = ElementName;
        imgElements.Add(this);
        
        //드래그  드랍 옵션 추가
        RegisterDragDropOption<Texture2D>(img, ChangeImage);
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

    private void ChangeImage(string path)
    {
        ElementPath = path;
        img.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(ElementPath);
    }
}
