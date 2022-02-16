using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCore : MonoBehaviour
{
    [Header("PlayerProp")]
    public List<Sprite> TanksTypes = new List<Sprite>();
    public int extraAT = 0;
    public int extraBulletHealth = 0;
    public int extraFlySpeed = 0;

    public int playerIndex;//0 代表 玩家A； 1 代表 玩家 B
    public int level = 1;
    public float health = 1;
    public bool isShield = false;
    public bool isDie = false;
    public int livesCount = 1;
    public int livesMaxCount = 1;
    public int livesCanGive = 0;
    public float attackTimeGap = 1;
    public float moveSpeed = 1;//敌人的固定速度是1，玩家的起始速度是3。


    public void LevelUp()
    {
        switch (level)
        {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = TanksTypes[0];

                attackTimeGap = 1f;
                health = 1;
                moveSpeed = 3;
                livesMaxCount = 1;

                extraAT = 0;
                extraBulletHealth = 0;
                extraFlySpeed = 0;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = TanksTypes[1];

                attackTimeGap = 0.8f;
                health = 2;
                moveSpeed = 4;
                livesMaxCount = 2;

                extraAT = 1;
                extraBulletHealth = 0;
                extraFlySpeed = 1;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = TanksTypes[2];

                attackTimeGap = 0.6f;
                health = 2;
                moveSpeed = 5;
                livesMaxCount = 3;

                extraAT = 2;
                extraBulletHealth = 1;
                extraFlySpeed = 2;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().sprite = TanksTypes[3];

                attackTimeGap = 0.4f;
                health = 3;
                moveSpeed = 6;
                livesMaxCount = 4;

                extraAT = 3;
                extraBulletHealth = 3;
                extraFlySpeed = 4;
                break;
            default:
                attackTimeGap = 0.2f;
                health = 3;
                moveSpeed += 1;
                livesMaxCount = 4;

                extraAT += 1;
                extraBulletHealth += 1;
                extraFlySpeed += 1;






                break;
        }
    }
}
