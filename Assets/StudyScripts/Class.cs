using UnityEngine;

public class Class:MonoBehaviour
{
    [SerializeField] private int First;
    [SerializeField] private int Second;
    private void Start()
    {
        Debug.Log(First + Second);
    }
}