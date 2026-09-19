using UnityEngine;

public class Methods : MonoBehaviour
{

    //void Start()
    //{
    //    Debug.Log("Jump");
    //    Jump();
    //    Debug.Log("Crouch");
    //}

    //void Jump()
    //{
    //    Debug.Log("Jump1");
    //    Debug.Log("Jump2");
    //    Debug.Log("Jump3");
    //}
    //----------------------------------------------------
    //private int hp = 100;
    //private void Start()
    //{

    //    while (hp > 0)
    //    {
    //        Debug.Log("Хп составляет" + CurrentHP());
    //        CurrentHP();
    //    if (hp <= 0)
    //        {
    //            Debug.Log("Вы двестик((");
    //            Debug.Log("Попробуй сначала!");
    //        }             
    //    }
    //}

    //int CurrentHP()
    //{
    //    int damage = Random.Range(1, 100);
    //    Debug.Log("Выстрел");
    //    Debug.Log("Нанесено " + damage + " урона");

    //    return hp -= damage;
    //}
    //---------------------------------------------------

    private void Start()
    {
        int currentplayers = 299;
        int newplayers = 78;
        Debug.Log(NumberOfPlayers(currentplayers, newplayers));
    }
    int NumberOfPlayers(int currentP, int newP)
    {
        return currentP + newP;
    }
    Methods MethodsScript()
    {
        return this;
    }
}
