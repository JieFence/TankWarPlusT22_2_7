using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D theRigidbody2D;
    public bool bulletFromA = false;
    public bool bulletFromB = false;
    [Header("数值")]
    public int BulletLevel = 1;
    public float AT = 1;
    public float flySpeed = 6;
    public float health = 1;
    void Start()
    {
        theRigidbody2D = GetComponent<Rigidbody2D>();
        theRigidbody2D.velocity = transform.up * flySpeed;
    }
    void Update()
    {
        AutoDestory();
    }
    void AutoDestory()
    {
        if (health < 1)
        {
            theRigidbody2D.velocity = Vector2.zero;
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);
            transform.GetChild(0).gameObject.SetActive(true);//启动炸弹
        }
    }
    private void OnTriggerEnter2D(Collider2D other)//子弹检测障碍物。
    {

        switch (other.tag)
        {
            case "GrassBush":
                if (BulletLevel > 3)
                {
                    Destroy(other.gameObject);
                    health -= 1;
                }
                break;
            case "RockWall":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);

                Destroy(other.gameObject);
                health -= 2;
                break;
            case "IronWall":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);
                if (BulletLevel > 2)
                {
                    Destroy(other.gameObject);
                    health -= 3;
                }
                else
                {
                    health = 0;
                }
                break;
            case "Home":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_HeartDamage);
                Destroy(other.gameObject);
                health = 0;
                break;
            case "Player":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);

                if (!DatePlayerCanControll.Instance.isGameOver_Win)
                {
                    if (bulletFromA || bulletFromB)
                    { }
                    else
                    {
                        if (!other.GetComponent<FightCore>().isShield)
                        {
                            other.GetComponent<PlayerController>().Attacked(AT);
                            health = 0;
                        }
                    }
                }
                else
                {
                    if (!other.GetComponent<FightCore>().isShield)
                    {
                        if ((other.GetComponent<FightCore>().playerIndex == 0) && bulletFromB)
                        {
                            other.GetComponent<PlayerController>().Attacked(AT);
                            health = 0;
                        }
                        if ((other.GetComponent<FightCore>().playerIndex == 1) && bulletFromA)
                        {
                            other.GetComponent<PlayerController>().Attacked(AT);
                            health = 0;
                        }

                    }

                }
                break;
            case "Enemy":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);
                if (bulletFromA || bulletFromB)
                {
                    if (!other.GetComponent<FightCore>().isShield)
                    {
                        other.GetComponent<EnemyController>().Attacked(AT);
                    }

                    health = 0;
                }
                break;
            case "Barrier":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Hit);
                health = 0;
                break;
            case "River":
                flySpeed *= 1.5f;
                theRigidbody2D.velocity = transform.up * flySpeed;
                break;
            case "Bullet":
                SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Explosion);
                other.GetComponent<BulletController>().health = 0;
                health = 0;
                break;
            default:
                break;
        }
    }
}
