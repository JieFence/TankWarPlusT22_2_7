using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : FightCore
{
    private float attackTimer;
    private Rigidbody2D theRigidbody2D;

    public KeyCode keyCode1;
    public string Xaxis;
    public string Yaxis;
    public GameObject bullet;

    public enum TowardDirection { up, down, left, right };
    public TowardDirection towardDirection;
    public bool isIdle = false;
    [Header("UI数据显示")]
    public Text Text_LV;
    public Text Text_AT;
    public Text Text_Health;
    public Text Text_LiveCount;
    public Text Text_LiveCanGive;
    public Text Text_MoveSpeed;


    void Start()
    {
        attackTimer = Time.time;
        theRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        Move();
        Attack(keyCode1);
        DateVisual();
    }

    void DateVisual()
    {
        Text_LV.text = level.ToString();
        Text_AT.text = (extraAT + 1).ToString();
        Text_Health.text = health.ToString();
        Text_LiveCount.text = livesCount.ToString() + "/" + livesMaxCount.ToString();
        Text_LiveCanGive.text = livesCanGive.ToString();
        Text_MoveSpeed.text = moveSpeed.ToString();
    }
    void Move()
    {
        float moveDirX = Input.GetAxis(Xaxis);
        float moveDirY = Input.GetAxis(Yaxis);
        if (Mathf.Abs(moveDirX) > Mathf.Abs(moveDirY))
        {
            theRigidbody2D.velocity = new Vector2(moveDirX, 0) * moveSpeed;
            if (theRigidbody2D.velocity.x > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                towardDirection = TowardDirection.right;
            }
            if (theRigidbody2D.velocity.x < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                towardDirection = TowardDirection.left;
            }
        }
        if (Mathf.Abs(moveDirX) < Mathf.Abs(moveDirY))
        {
            theRigidbody2D.velocity = new Vector2(0, moveDirY) * moveSpeed;
            if (theRigidbody2D.velocity.y > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                towardDirection = TowardDirection.up;
            }
            if (theRigidbody2D.velocity.y < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                towardDirection = TowardDirection.down;
            }
        }
        if (theRigidbody2D.velocity.magnitude < moveSpeed * 0.1f && isIdle)
        {
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_EngineIdle);
            isIdle = false;
        }
        if (theRigidbody2D.velocity.magnitude == moveSpeed && !isIdle)
        {
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_EngineDriving);
            isIdle = true;
        }
    }
    void Attack(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode) && Time.time - attackTimer >= attackTimeGap)
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

            //  此处为作者额外增加的猛料！


            if (gameObject.GetComponent<FightCore>().playerIndex == 0)
            {
                bulletController.bulletFromA = true;
            }
            else if (gameObject.GetComponent<FightCore>().playerIndex == 1)
            {
                bulletController.bulletFromB = true;
            }


            //

            bulletController.AT += extraAT;
            bulletController.health += extraBulletHealth;
            bulletController.flySpeed += extraFlySpeed;
            bulletController.BulletLevel = level;
            attackTimer = Time.time;
        }
    }
    public void Attacked(float AT)
    {
        health -= AT;
    }
}
