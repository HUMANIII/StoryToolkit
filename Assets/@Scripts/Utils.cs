using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// 널검사 및 각 요소 추가용 - 에디터에서만 동작함
    /// </summary>
    /// <param name="field">검사할 요소</param>
    /// <typeparam name="T">해당 요소의 타입</typeparam>
    public static void CheckComp<T>(this Component field) where T : Component
    {
        if (field == null)
        {
            Debug.LogError("field is null");
        }
        if (!field.gameObject.TryGetComponent(out T comp))
        {
            Debug.LogError(typeof(T) + " does not have a component of type " + typeof(T));
        }
    }
}
