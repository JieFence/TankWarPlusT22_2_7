using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheHome : MonoBehaviour
{
    public float health = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        JudgeGameOver();
    }
    void JudgeGameOver()
    {
        if (health < 1)
        {
            //暂停游戏，播放Game Over动画。
            Debug.Log("GameOver");
            Destroy(gameObject);
        }
    }
    public void HomeAttacked(float AT)
    {
        health -= AT;
    }
}
