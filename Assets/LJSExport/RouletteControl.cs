using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteControl : MonoBehaviour
{
    Animator RandomAni;
    void Start()
    {
        RandomAni = GetComponent<Animator>();
    }
    public float timer = 0f;
    public static bool timerFlag = false;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 7f) return;
        if (!timerFlag)
        {
            timerFlag = true;
            RandomSelect();
        }

    }

    public static int randomNum = -1;

    public void RandomSelect()
    {
        print("A"); ;
        randomNum = Random.Range(0, 2);
        if (randomNum == 0)
        {
            RandomAni.SetTrigger("SelectA");
        }
        if (randomNum == 1)
        {
            RandomAni.SetTrigger("SelectB");
        }
    }
}
