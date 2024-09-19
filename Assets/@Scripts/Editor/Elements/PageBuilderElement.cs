using UnityEngine;

public abstract class PageBuilderElement
{
    //���� ������ ����ؼ� �ٽ� �����غ���
    //���� �����ϴ°� -> Ư�� �Լ� ȣ��� �Ű������� �� Ŭ������ ��ȭ�� �־ �����ϴ°� ClassA.FucA().FucB().FucC() �̷�������
    //������ FuncA ����� �� ��ҷ� �߰��ϴ��Լ��̸� ������ ������ �͵��� ������ Ŭ�������� �����ϵ��� �ϴ°�
    public string ElementName;
    public string ElementPath;
    public bool isGameObject;
    public abstract void SaveData();
    public abstract void AddElement();
    public abstract void RemoveElement();
    public abstract void Reset();
}
