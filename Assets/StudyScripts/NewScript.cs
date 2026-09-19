using UnityEngine;

public class NewScript : MonoBehaviour
{
    private int _number = 5 + 5 - 3;
    private float _floatnumber = 4.4f;
    string _myAge = "Мне двадцять четыре";
    void Start()
    {
        Debug.Log(_number);
        _number++;
        _floatnumber = _number;
        Debug.Log(_floatnumber - _number);
        Debug.Log(_number);
        Debug.Log(_myAge);
        Da();
    }
    void Da()
    {
        var Variable = "lol " + true;
        Debug.Log(Variable);
    }
}
