using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : FightCore
{

    private Rigidbody2D theRigidbody2D;
    public enum TowardDirection { up, down, left, right };
    public enum MoveMode { WanderMode, AttackHomeMode }
    public MoveMode moveMode;

    public TowardDirection towardDirection;
    public GameObject bullet;


    private float timer;
    public float calmDownTime;


    //敌人AI：
    public Vector2 basePos;
    public Vector2 tarPos;
    [Space]
    public LayerMask layerMaskWall;
    public LayerMask layerMaskTank;
    [Space]

    public int[] randomDirNumber = new int[] { 0, 1, 2, 3 }; //不可变

    public float stepLength = 0.5f;//不可变
    public float lineLength = 0.1f;//不可变


    [SerializeField]
    public bool thenCanBlock = false;//不可变
    public bool isSurrounded = false;//不可变

    void Start()
    {
        theRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        timer = Time.time;

        tarPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        AutoMove();
        AutoAttack();
        Die();


    }
    void AutoMove()
    {
        //自动移动。
        switch (moveMode)
        {
            case MoveMode.WanderMode:

                if ((isSurrounded == false) && (Vector2.Distance(transform.position, tarPos) < 0.01f || theRigidbody2D.velocity.magnitude < 0.01f))
                {
                    basePos = transform.position;
                    Shuffle(randomDirNumber);
                    foreach (var dirNumber in randomDirNumber)
                    {
                        isSurrounded = true;
                        switch (dirNumber)
                        {
                            //第一层是墙体识别
                            //第二层是正方向坦克识别
                            //第三层是测方向坦克识别
                            case 0:
                                if (ObstacleRecognition(transform.position, new Vector2(-0.25f, 0.8f), Vector2.up, lineLength, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(0.25f, 0.8f), Vector2.up, lineLength, layerMaskWall))
                                {
                                    thenCanBlock = true;
                                }
                                else
                                {
                                    if (ObstacleRecognition(transform.position, Vector2.up * 0.8f, Vector2.up, lineLength, layerMaskTank))
                                    {
                                        thenCanBlock = true;
                                    }
                                    else
                                    {
                                        //这里只有layermask可以管理。
                                        if (ObstacleRecognition(transform.position, new Vector2(-0.2f, 0.8f), Vector2.left * 0.2f, 0.2f, layerMaskTank) || ObstacleRecognition(transform.position, new Vector2(0.2f, 0.8f), Vector2.right * 0.2f, 0.2f, layerMaskTank))
                                        {
                                            thenCanBlock = true;
                                        }
                                        else
                                        {
                                            thenCanBlock = false;
                                        }
                                    }
                                }
                                break;
                            case 1:
                                if (ObstacleRecognition(transform.position, new Vector2(0.25f, -0.8f), Vector2.down, lineLength, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(-0.25f, -0.8f), Vector2.down, lineLength, layerMaskWall))
                                {
                                    thenCanBlock = true;
                                }
                                else
                                {
                                    if (ObstacleRecognition(transform.position, Vector2.down * 0.8f, Vector2.down, lineLength, layerMaskTank))
                                    {
                                        thenCanBlock = true;
                                    }
                                    else
                                    {
                                        if (ObstacleRecognition(transform.position, new Vector2(-0.2f, -0.8f), Vector2.left * 0.2f, 0.2f, layerMaskTank) || ObstacleRecognition(transform.position, new Vector2(0.2f, -0.8f), Vector2.right * 0.2f, 0.2f, layerMaskTank))
                                        {
                                            thenCanBlock = true;
                                        }
                                        else
                                        {
                                            thenCanBlock = false;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (ObstacleRecognition(transform.position, new Vector2(-0.8f, -0.25f), Vector2.left, lineLength, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(-0.8f, 0.25f), Vector2.left, lineLength, layerMaskWall))
                                {
                                    thenCanBlock = true;
                                }
                                else
                                {
                                    if (ObstacleRecognition(transform.position, Vector2.left * 0.8f, Vector2.left, lineLength, layerMaskTank))
                                    {
                                        thenCanBlock = true;
                                    }
                                    else
                                    {
                                        if (ObstacleRecognition(transform.position, new Vector2(-0.8f, -0.2f), Vector2.down * 0.2f, 0.2f, layerMaskTank) || ObstacleRecognition(transform.position, new Vector2(-0.8f, 0.2f), Vector2.up * 0.2f, 0.2f, layerMaskTank))
                                        {
                                            thenCanBlock = true;
                                        }
                                        else
                                        {
                                            thenCanBlock = false;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                if (ObstacleRecognition(transform.position, new Vector2(0.8f, -0.25f), Vector2.right, lineLength, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(0.8f, 0.25f), Vector2.right, lineLength, layerMaskWall))
                                {
                                    thenCanBlock = true;
                                }
                                else
                                {
                                    if (ObstacleRecognition(transform.position, Vector2.right * 0.8f, Vector2.right, lineLength, layerMaskTank))
                                    {
                                        thenCanBlock = true;
                                    }
                                    else
                                    {
                                        if (ObstacleRecognition(transform.position, new Vector2(0.8f, -0.2f), Vector2.down * 0.2f, 0.2f, layerMaskTank) || ObstacleRecognition(transform.position, new Vector2(0.8f, 0.2f), Vector2.up * 0.2f, 0.2f, layerMaskTank))
                                        {
                                            thenCanBlock = true;
                                        }
                                        else
                                        {
                                            thenCanBlock = false;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        if (thenCanBlock == false) //如果找到了可行方向：
                        {
                            isSurrounded = false;
                            switch (dirNumber)//给速器 和 重定器。
                            {
                                case 0:
                                    tarPos = new Vector2(basePos.x, basePos.y + stepLength);
                                    theRigidbody2D.velocity = Vector2.up * moveSpeed;
                                    towardDirection = TowardDirection.up;
                                    break;
                                case 1:
                                    tarPos = new Vector2(basePos.x, basePos.y - stepLength);
                                    theRigidbody2D.velocity = Vector2.down * moveSpeed;
                                    towardDirection = TowardDirection.down;
                                    break;
                                case 2:
                                    tarPos = new Vector2(basePos.x - stepLength, basePos.y);
                                    theRigidbody2D.velocity = Vector2.left * moveSpeed;
                                    towardDirection = TowardDirection.left;
                                    break;
                                case 3:
                                    tarPos = new Vector2(basePos.x + stepLength, basePos.y);
                                    theRigidbody2D.velocity = Vector2.right * moveSpeed;
                                    towardDirection = TowardDirection.right;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    }
                }
                break;
            case MoveMode.AttackHomeMode:
                break;
            default:
                break;
        }
        //四面楚歌检测s器：
        if (isSurrounded == true)
        {
            if (false == (ObstacleRecognition(transform.position, new Vector2(-0.4f, 0.75f), Vector2.right * 0.8f, 0.8f, layerMaskWall) && ObstacleRecognition(transform.position, new Vector2(-0.4f, -0.75f), Vector2.right * 0.8f, 0.8f, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(0.75f, -0.4f), Vector2.up * 0.8f, 0.8f, layerMaskWall) || ObstacleRecognition(transform.position, new Vector2(-0.75f, -0.4f), Vector2.up * 0.8f, 0.8f, layerMaskWall)))
            {
                isSurrounded = false;
            }
        }
        //自动转向：
        switch (towardDirection)
        {
            case TowardDirection.up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case TowardDirection.down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case TowardDirection.left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case TowardDirection.right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            default:
                break;
        }
    }
    void AutoAttack()
    {
        if (Time.time - timer > calmDownTime)
        {
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Fire);
            GameObject BirthBullet;
            switch (towardDirection)
            {
                case TowardDirection.up:
                    BirthBullet = Instantiate(bullet, transform.position + Vector3.up * 0.5f, Quaternion.Euler(0, 0, 0));
                    break;
                case TowardDirection.down:
                    BirthBullet = Instantiate(bullet, transform.position + Vector3.down * 0.5f, Quaternion.Euler(0, 0, 180));
                    break;
                case TowardDirection.left:
                    BirthBullet = Instantiate(bullet, transform.position + Vector3.left * 0.5f, Quaternion.Euler(0, 0, 90));
                    break;
                case TowardDirection.right:
                    BirthBullet = Instantiate(bullet, transform.position + Vector3.right * 0.5f, Quaternion.Euler(0, 0, -90));
                    break;
                default:
                    BirthBullet = new GameObject();
                    break;
            }
            BulletController bulletController = BirthBullet.GetComponent<BulletController>();
            // bulletController.AT += extraAT;
            // bulletController.health += extraBulletHealth;
            extraFlySpeed = Random.Range(4, 9);
            bulletController.flySpeed += extraFlySpeed;
            // bulletController.BulletLevel = level;

            calmDownTime = Random.Range(0.6f, 3f);
            timer = Time.time;

        }
    }
    void Die()
    {
        if (health < 1)
        {
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Die);
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Explosion);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);

        }
    }
    void Shuffle(int[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            int temp = deck[i];
            int randomIndex = Random.Range(0, deck.Length);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    RaycastHit2D ObstacleRecognition(Vector2 pos, Vector2 offset, Vector2 dir, float lineLength, LayerMask layerMask)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dir, lineLength, layerMask);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, dir, color, 0.5f);

        return hit;
    }
    public void Attacked(float AT)
    {
        health -= AT;
    }
}
