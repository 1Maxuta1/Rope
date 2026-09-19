using UnityEngine;


//void Start()
//{
//int[] numbers = new[] { 5, 12, 45, 34, 90 };
//FindNumber(numbers, 34);
//}
//    void FindNumber(int[] numbers, int target)
//    {
//        for (int i = 0; i < numbers.Length; i++)
//    {
//            if (numbers[i] == target)
//            {
//            Debug.Log("Число " + 34 + " найдено, завершаем поиск");
//            break;
//        }

//    }
//}
//---------------------------------------------
//public class Array : MonoBehaviour
//{
//    private int i = 0;
//    private void Start()
//    {
//        do
//        {
//            i++;
//            Debug.Log(i);

//        }
//        while (i < 5);
//        Debug.Log("Число больше 5");

//    }
//}
//-----------------------------------
public class Array : MonoBehaviour
{
    private int i = 0;
    private void Start()
    {
        while (i < 5)
        {
            i++;
            Debug.Log(i);

        }      
        Debug.Log("Число больше 5");       
    }
}
