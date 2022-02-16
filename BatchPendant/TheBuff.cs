using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBuff : MonoBehaviour
{
    public enum Buffs { HEART, TIMER, SHOVEL, GRENADE, STAR, HAT }
    public Buffs buffs;
    private float timer;
    public float liveCycle = 8;
    void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        OldAndDie();
    }
    void OldAndDie()
    {
        if (Time.time - timer > liveCycle)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_GetBonus);
            switch (buffs)
            {
                case Buffs.HEART:
                    if (other.GetComponent<FightCore>().livesCount < other.GetComponent<FightCore>().livesMaxCount)
                    {
                        other.GetComponent<FightCore>().livesCount += 1;
                    }
                    else
                    {
                        other.GetComponent<FightCore>().livesCanGive += 1;
                    }
                    Destroy(this.gameObject);
                    break;
                case Buffs.TIMER:
                    // 加速器。
                    other.GetComponent<FightCore>().extraFlySpeed += 1;
                    Destroy(this.gameObject);
                    break;
                case Buffs.SHOVEL:
                    //移出  河流。
                    GameObject[] Rivers = GameObject.FindGameObjectsWithTag("River");
                    if (Rivers.Length != 0)
                    {
                        foreach (var item in Rivers)
                        {
                            if (Random.Range(0, 5) == 0)
                            {
                                Destroy(item.gameObject);
                            }
                        }
                    }
                    Destroy(this.gameObject);
                    break;
                case Buffs.GRENADE:
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < Enemies.Length; i++)
                    {
                        Enemies[i].GetComponent<FightCore>().health -= 1;
                    }
                    Destroy(this.gameObject);
                    break;
                case Buffs.STAR:
                    other.GetComponent<FightCore>().level += 1;
                    other.GetComponent<FightCore>().LevelUp();
                    Destroy(this.gameObject);
                    break;
                case Buffs.HAT:
                    other.GetComponent<FightCore>().health += 1;
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
