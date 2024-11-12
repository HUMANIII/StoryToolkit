using UnityEngine;

public abstract class PageBuilderElement
{
    //빌더 패턴을 사용해서 다시 구현해보기
    //내가 생각하는것 -> 특정 함수 호출시 매개변수로 들어간 클래스에 변화를 주어서 리턴하는것 ClassA.FucA().FucB().FucC() 이런식으로
    //위에서 FuncA 등등이 각 요소룰 추가하는함수이며 각자의 세세한 것들은 각각의 클래스에서 구현하도록 하는것
    public string ElementName;
    public string ElementPath;
    public PageBuilder pageBuilder;
    public bool isGameObject;
    /// <summary>
    /// 저장 시 호출
    /// </summary>
    public abstract void SaveData();
    /// <summary>
    /// 요소 추가시 호출 - 일반적으로 각 데이터를 저장하기 위해 사용
    /// </summary>
    public abstract void AddElement(PageBuilder pb);
    /// <summary>
    /// 요소 삭제시 호출
    /// </summary>
    public abstract void RemoveElement();
    /// <summary>
    /// 모든 요소 초기화 버튼 눌렀을때 호출
    /// </summary>
    public abstract void Reset();
}
