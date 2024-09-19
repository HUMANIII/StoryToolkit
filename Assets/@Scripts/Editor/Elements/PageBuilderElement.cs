using UnityEngine;

public abstract class PageBuilderElement
{
    //빌더 패턴을 사용해서 다시 구현해보기
    //내가 생각하는것 -> 특정 함수 호출시 매개변수로 들어간 클래스에 변화를 주어서 리턴하는것 ClassA.FucA().FucB().FucC() 이런식으로
    //위에서 FuncA 등등이 각 요소룰 추가하는함수이며 각자의 세세한 것들은 각각의 클래스에서 구현하도록 하는것
    public string ElementName;
    public string ElementPath;
    public bool isGameObject;
    public abstract void SaveData();
    public abstract void AddElement();
    public abstract void RemoveElement();
    public abstract void Reset();
}
