using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBirther : MonoBehaviour
{
    public GameObject fenceWall;
    public List<GameObject> Walls = new List<GameObject>();
    public enum MapType { AbsolutelyRandom, BigRock, BigSeel, BigGrassBush, BigRiver };
    public MapType mapType;
    [Range(19, 38)] public int XCount;
    [Range(21, 42)] public int YCount;
    [Range(0, 100)] public int WallDensity;//后面用拉杆代替。满值设为100
    public List<Vector2> spawnPoints = new List<Vector2>();
    public Vector2[] choosePoints;
    public List<Vector2> fencePoints = new List<Vector2>();
    [Header("BuffManager")]
    private float buffTimer;
    public float randomTimeGap = 15;
    public Vector2 randomPos;
    public List<GameObject> Buffs = new List<GameObject>();
    [Space]
    [Space]
    [Header("玩家管理器")]
    public GameObject playerA;
    public GameObject BornStarForplayerA;
    public GameObject playerB;
    public GameObject BornStarForplayerB;

    private FightCore fightCore_A;
    private FightCore fightCore_B;
    private Vector2 rebrithPoint_A;
    private Vector2 rebrithPoint_B;
    [Space]
    [Space]

    [Header(" 敌人生成器")]
    public LayerMask layerMaskTank;

    private bool canBirthOnAPoint = false;
    private bool canBirthOnBPoint = false;
    private bool canBirthOnCPoint = false;

    public float enemyBirthTimer;
    public float enemyBirthTimerLeftTime;

    public int enemyCountOnMap;
    public int enemyBirthTimeGap = 8;//随意
    [Range(3, 100)] public int enemyTotalCount = 10;//随意
    [Range(3, 20)] public int enemyTotalWaves = 3;//随意

    public int hasBirthEnemiesCountOfTheWave = 0;//必须是零！
    public int nowWave = 1;//必须是一！


    public int[] theChooseInt;
    public int[] theEnemyWaveCountArray;
    public int[] qwe;

    public Vector2[] enemyBirthPoints = new Vector2[3];
    public List<GameObject> Enemys = new List<GameObject>();
    public List<Sprite> enemysSprite = new List<Sprite>();
    [Header("游戏管理器")]
    public GameObject PauseMenu;


    public GameObject gameOver_Lose;
    public bool isDoubleDie = false;
    public GameObject gameOver;
    public bool hasGameOver = false;//不可更改！
    [Header("UI数据显示")]
    public GameObject PublicTell;
    public Text text_nowWave;
    public Text text_theEnemiesCountOfTheWave;

    // Start is called before the first frame update
    //每隔一定时间，在一定位置，生成一定数量的buff
    void Start()
    {
        //数据复位。1
        enemyTotalCount = DatePlayerCanControll.Instance.enemyTotalCount;
        enemyTotalWaves = DatePlayerCanControll.Instance.enemyTotalWaves;
        WallDensity = DatePlayerCanControll.Instance.WallDensity;
        PointsBirth();

        buffTimer = Time.time;
        enemyBirthTimer = Time.time;
        enemyBirthTimeGap = Random.Range(2, 17);
        //
        fightCore_A = playerA.GetComponent<FightCore>();
        fightCore_B = playerB.GetComponent<FightCore>();
        rebrithPoint_A = playerA.transform.position;
        rebrithPoint_B = playerB.transform.position;

        BornStarForplayerA.SetActive(true);//召唤玩家
        BornStarForplayerB.SetActive(true);//召唤玩家
        //

        choosePoints = ChooseSet((int)(WallDensity * 0.01f * spawnPoints.Count), spawnPoints);
        MapFill();
        FenceWallFill();
        EnemyWavesBirther();

        if (DatePlayerCanControll.Instance.playerCount == 1)
        {
            fightCore_B.health = 0;
        }
        UIMapDateManager();

    }
    private void Update()
    {
        enemyCountOnMap = GameObject.FindGameObjectsWithTag("Enemy").Length;
        enemyBirthTimerLeftTime = enemyBirthTimeGap - Time.time + enemyBirthTimer;
        EnemyBirther();
        bfManager();



        Die_PlayerA();
        Die_PlayerB();
        PauseMenuCTRL();
        GameLose();
    }
    void GameLose()
    {
        if (fightCore_A.isDie && fightCore_B.isDie && !isDoubleDie)
        {
            isDoubleDie = true;

            gameOver_Lose.SetActive(true);
        }
    }
    void UIMapDateManager()//没必要一直刷新，只需要到达下一波的时候进行活动就可以了。
    {
        text_nowWave.text = nowWave.ToString(); ;
        text_theEnemiesCountOfTheWave.text = theEnemyWaveCountArray[nowWave - 1].ToString();
    }
    void PauseMenuCTRL()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            if (PauseMenu.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = canBirthOnAPoint ? Color.green : Color.red;
        Gizmos.DrawWireCube(enemyBirthPoints[0], Vector2.one * 1.5f);

        Gizmos.color = canBirthOnBPoint ? Color.green : Color.red;
        Gizmos.DrawWireCube(enemyBirthPoints[1], Vector2.one * 1.5f);

        Gizmos.color = canBirthOnCPoint ? Color.green : Color.red;
        Gizmos.DrawWireCube(enemyBirthPoints[2], Vector2.one * 1.5f);
    }
    void EnemyWavesBirther()
    {
        //敌人分拨器：将a数量小球随机分成b组。从可选数中 选出（波数 — 1）个隔板。
        qwe = new int[enemyTotalCount - 1];
        for (int i = 1; i < enemyTotalCount; i++)
        {
            qwe[i - 1] = i;
        }
        theChooseInt = ChooseInt(enemyTotalWaves - 1, qwe);
        QuickSort(theChooseInt, 0, theChooseInt.Length - 1);


        theEnemyWaveCountArray = new int[theChooseInt.Length + 1];
        for (int i = 0; i < theChooseInt.Length + 1; i++)
        {
            if (i != 0)
            {
                if (i != theChooseInt.Length)
                {
                    theEnemyWaveCountArray[i] = theChooseInt[i] - theChooseInt[i - 1];
                }
                else
                {
                    theEnemyWaveCountArray[i] = enemyTotalCount - theChooseInt[i - 1];
                }
            }
            else
            {
                theEnemyWaveCountArray[i] = theChooseInt[i];
            }
        }
    }
    void EnemyBirther()//敌人生成器。
    {
        //分拨实例器：实例化。
        if (hasBirthEnemiesCountOfTheWave < theEnemyWaveCountArray[nowWave - 1])
        {
            canBirthOnAPoint = !Physics2D.BoxCast(enemyBirthPoints[0], Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank);
            canBirthOnBPoint = !Physics2D.BoxCast(enemyBirthPoints[1], Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank);
            canBirthOnCPoint = !Physics2D.BoxCast(enemyBirthPoints[2], Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank);


            if (Time.time - enemyBirthTimer >= enemyBirthTimeGap)//每隔一段时间尝试生成敌人
            {
                Vector2[] tempVectors = ChooseSet(enemyBirthPoints.Length, enemyBirthPoints);

                //由一次尝试生成敌人的次数 1 2 3 的 概率比 是 1 3 2 ；
                int tempRandomValue = Random.Range(1, 7);

                if (theEnemyWaveCountArray[nowWave - 1] - hasBirthEnemiesCountOfTheWave > 2)
                {
                    if (tempRandomValue < 2)
                    {
                        foreach (var tempVctr in tempVectors)
                        {
                            if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                            {
                                GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                                tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                                tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                                tempEnemy.GetComponent<FightCore>().isShield = true;

                                hasBirthEnemiesCountOfTheWave++;

                                enemyBirthTimer = Time.time;
                                enemyBirthTimeGap = Random.Range(5, 8);
                                break;
                            }
                        }
                    }
                    else if (tempRandomValue < 5)
                    {
                        foreach (var tempVctr in ChooseSet(2, tempVectors))
                        {
                            if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                            {
                                GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                                tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                                tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                                tempEnemy.GetComponent<FightCore>().isShield = true;

                                hasBirthEnemiesCountOfTheWave++;
                            }
                        }
                        enemyBirthTimer = Time.time;
                        enemyBirthTimeGap = Random.Range(5, 8);
                    }
                    else
                    {
                        foreach (var tempVctr in tempVectors)
                        {
                            if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                            {
                                GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                                tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                                tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                                tempEnemy.GetComponent<FightCore>().isShield = true;

                                hasBirthEnemiesCountOfTheWave++;
                            }
                            enemyBirthTimer = Time.time;
                            enemyBirthTimeGap = Random.Range(5, 8);
                        }
                    }

                }
                else if (theEnemyWaveCountArray[nowWave - 1] - hasBirthEnemiesCountOfTheWave > 1)
                {
                    if (tempRandomValue < 5)
                    {
                        foreach (var tempVctr in ChooseSet(2, tempVectors))
                        {
                            if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                            {
                                GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                                tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                                tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                                tempEnemy.GetComponent<FightCore>().isShield = true;

                                hasBirthEnemiesCountOfTheWave++;
                            }
                        }
                        enemyBirthTimer = Time.time;
                        enemyBirthTimeGap = Random.Range(5, 8);
                    }
                    else
                    {
                        foreach (var tempVctr in tempVectors)
                        {
                            if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                            {
                                GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                                tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                                tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                                tempEnemy.GetComponent<FightCore>().isShield = true;

                                hasBirthEnemiesCountOfTheWave++;

                                enemyBirthTimer = Time.time;
                                enemyBirthTimeGap = Random.Range(5, 8);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var tempVctr in tempVectors)
                    {
                        if (!Physics2D.BoxCast(tempVctr, Vector2.one * 1.5f, 0, Vector2.zero, 0, layerMaskTank))//如果 该位置 可以生成。
                        {
                            GameObject tempEnemy = Instantiate(Enemys[Random.Range(0, Enemys.Count)], tempVctr, Quaternion.identity);
                            tempEnemy.GetComponent<SpriteRenderer>().sprite = enemysSprite[Random.Range(0, enemysSprite.Count)];
                            tempEnemy.transform.GetChild(0).gameObject.SetActive(true);
                            tempEnemy.GetComponent<FightCore>().isShield = true;

                            hasBirthEnemiesCountOfTheWave++;

                            enemyBirthTimer = Time.time;
                            enemyBirthTimeGap = Random.Range(5, 8);
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (nowWave != enemyTotalWaves)
            {
                if (enemyCountOnMap == 0)//这一波的坦克已经被消灭干净了！
                {
                    enemyBirthTimer = Time.time;
                    nowWave++;
                    hasBirthEnemiesCountOfTheWave = 0;
                    UIMapDateManager();
                }
            }
            else
            {
                if (enemyCountOnMap == 0)//最后一波的坦克已经被消灭干净了！
                {
                    if (hasGameOver == false)
                    {
                        hasGameOver = true;
                        DatePlayerCanControll.Instance.isGameOver_Win = true;

                        gameOver.SetActive(true);

                    }
                }
            }
        }
    }
    RaycastHit2D TankRecognition(Vector2 pos, Vector2 offset, Vector2 dir, float lineLength, LayerMask layerMask)
    {

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, dir, lineLength, layerMask);

        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, dir, color, 0.5f);

        return hit;
    }
    void bfManager()//BUFF 生成器。
    {
        if (Time.time - buffTimer >= randomTimeGap)
        {
            randomTimeGap = Random.Range(5, 13);
            int randmValueTempToBuff = Random.Range(1, 101);
            if (randmValueTempToBuff > 65)
            {
                Instantiate(Buffs[0], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//星星。
            }
            else if (randmValueTempToBuff > 40)
            {
                Instantiate(Buffs[1], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//坦克。
            }
            else if (randmValueTempToBuff > 32)
            {
                Instantiate(Buffs[1], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//一格血。
            }
            else if (randmValueTempToBuff > 24)
            {
                Instantiate(Buffs[3], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//子弹加速。
            }
            else if (randmValueTempToBuff > 10)
            {
                Instantiate(Buffs[4], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//铲子。
            }
            else
            {
                Instantiate(Buffs[5], new Vector2(Random.Range(0, XCount), Random.Range(0, YCount)), Quaternion.identity);//手雷。
            }

            buffTimer = Time.time;
        }
    }
    void PointsBirth()//地图上的点生成器。
    {
        for (int i = 0; i < XCount; i++)
        {
            for (int j = 0; j < YCount; j++)
            {
                spawnPoints.Add(new Vector2(i, j));
            }
        }


        spawnPoints.Remove(new Vector2(0, YCount - 1));
        spawnPoints.Remove(new Vector2(XCount - 1, YCount - 1));
        int Xcenter = (int)((XCount - 1) / 2);
        int Ycenter = (int)((YCount - 1) / 2);
        spawnPoints.Remove(new Vector2(Xcenter, YCount - 1));


        enemyBirthPoints[0] = new Vector2(0, YCount - 1);//添加敌人生成点
        enemyBirthPoints[1] = new Vector2(XCount - 1, YCount - 1);
        enemyBirthPoints[2] = new Vector2(Xcenter, YCount - 1);


        spawnPoints.Remove(new Vector2(Xcenter, 0));
        spawnPoints.Remove(new Vector2(Xcenter - 1, 0));
        spawnPoints.Remove(new Vector2(Xcenter + 1, 0));

        spawnPoints.Remove(new Vector2(Xcenter, 1));
        spawnPoints.Remove(new Vector2(Xcenter - 1, 1));
        spawnPoints.Remove(new Vector2(Xcenter + 1, 1));


        spawnPoints.Remove(new Vector2(Xcenter - 2, 0));
        spawnPoints.Remove(new Vector2(Xcenter + 2, 0));


        //生成外围墙壁坐标。
        for (int u = -1; u < XCount + 1; u++)
        {
            fencePoints.Add(new Vector2(u, -1));
            fencePoints.Add(new Vector2(u, YCount));
        }
        for (int v = -1; v < YCount + 1; v++)
        {
            fencePoints.Add(new Vector2(-1, v));
            fencePoints.Add(new Vector2(XCount, v));
        }
        fencePoints.Remove(new Vector2(-1, -1));
        fencePoints.Remove(new Vector2(-1, YCount));
        fencePoints.Remove(new Vector2(XCount, -1));
        fencePoints.Remove(new Vector2(XCount, YCount));
        // for (int i = 0; i < 2 * (XCount + YCount) - 4; i++)
        // {
        //     fencePoints[i] = new Vector2();
        // }
    }
    int[] ChooseInt(int numRequired, int[] qwe) //int不重复选择器。
    {
        int[] result = new int[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = qwe.Length; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = qwe[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }
    Vector2[] ChooseSet(int numRequired, Vector2[] qwe) //vector2不重复选择器。
    {
        Vector2[] result = new Vector2[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = qwe.Length; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = qwe[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }
    Vector2[] ChooseSet(int numRequired, List<Vector2> spawnPoints) //Vector2不重复选择器。
    {
        Vector2[] result = new Vector2[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = spawnPoints.Count; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = spawnPoints[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }
    void MapFill()//砖墙填充器。
    {
        switch (mapType)
        {
            case MapType.AbsolutelyRandom:
                foreach (var item in choosePoints)
                {
                    Instantiate(Walls[Random.Range(0, Walls.Count)], item, Quaternion.identity);
                }
                break;

            //通过曲线来进行非线性的加权比例分布。 //后期再做。
            case MapType.BigRock:
                break;
            case MapType.BigSeel:
                break;
            case MapType.BigGrassBush:
                break;
            case MapType.BigRiver:
                break;
            default:
                break;
        }

    }
    void FenceWallFill()//屏障生成器。
    {
        foreach (var item in fencePoints)
        {
            Instantiate(fenceWall, item, Quaternion.identity);
        }
    }
    void Die_PlayerA()
    {
        if (fightCore_A.health < 1 && !fightCore_A.isDie)//死后 减益效果：降级，减一条命。
        {
            //已经驾崩！：
            playerA.transform.GetChild(1).gameObject.SetActive(true);
            playerA.transform.position = rebrithPoint_A;

            if (fightCore_A.level > 1)
            {
                fightCore_A.level -= 1;
            }
            if (fightCore_A.livesCount > 0)
            {
                fightCore_A.livesCount -= 1;

            }

            // 尝试重生：
            if (fightCore_A.livesCount > 0)
            {
                BornStarForplayerA.SetActive(true);
                fightCore_A.livesCanGive = 0;//自己重生的话，此值归零。
                fightCore_A.LevelUp();
            }
            else
            {
                if (fightCore_B.livesCanGive > 0)
                {
                    //重生！！！
                    BornStarForplayerA.SetActive(true);
                    fightCore_B.livesCanGive -= 1;//对方
                    fightCore_A.LevelUp();
                }
                else
                {
                    // 阵亡！！！
                    fightCore_A.isDie = true;
                }
            }
        }

        //复活 队友。
        if (DatePlayerCanControll.Instance.playerCount > 1)
        {
            if (fightCore_A.livesCanGive > 0 && fightCore_B.isDie)//后者为对方
            {
                if ((int)(fightCore_A.level * 0.2f) != 0)
                {
                    fightCore_A.level = (int)(fightCore_A.level * 0.2f);
                }
                else
                {
                    fightCore_A.level = 1;
                }
                fightCore_A.livesCanGive -= 1;
                fightCore_A.LevelUp();

                // fightCore_B.livesCount += 1;
                fightCore_B.isDie = false; //对方。
                BornStarForplayerB.SetActive(true);//对方。

                fightCore_B.LevelUp();
            }
        }
    }
    void Die_PlayerB()
    {
        if (fightCore_B.health < 1 && !fightCore_B.isDie)//死后 减益效果：降级，减一条命。
        {
            //已经驾崩！：
            playerB.transform.GetChild(1).gameObject.SetActive(true);
            playerB.transform.position = rebrithPoint_B;

            if (fightCore_B.level > 1)
            {
                fightCore_B.level -= 1;
            }
            if (fightCore_B.livesCount > 0)
            {
                fightCore_B.livesCount -= 1;
            }
            SoundMananger.instance.PlayAudioClip(SoundMananger.instance.audioClip_Die);
            // 尝试重生：
            if (fightCore_B.livesCount > 0)
            {
                BornStarForplayerB.SetActive(true);
                fightCore_B.livesCanGive = 0;//自己重生的话，此值归零。
                fightCore_B.LevelUp();
            }
            else
            {
                if (fightCore_A.livesCanGive > 0)//
                {
                    //重生！！！
                    BornStarForplayerB.SetActive(true);
                    fightCore_A.livesCanGive -= 1;//对方
                    fightCore_B.LevelUp();
                }
                else
                {
                    // 阵亡！！！
                    fightCore_B.isDie = true;
                }
            }
        }
        //复活 队友。
        if (DatePlayerCanControll.Instance.playerCount > 1)
        {
            if (fightCore_B.livesCanGive > 0 && fightCore_A.isDie)//后者为对方
            {

                if ((int)(fightCore_B.level * 0.2f) != 0)
                {
                    fightCore_B.level = (int)(fightCore_B.level * 0.2f);
                }
                else
                {
                    fightCore_B.level = 1;
                }
                fightCore_B.livesCanGive -= 1;
                fightCore_B.LevelUp();

                // fightCore_A.livesCount += 1;
                fightCore_A.isDie = false; //对方。
                BornStarForplayerA.SetActive(true);//对方。

                fightCore_A.LevelUp();
            }
        }

    }
    void QuickSort(int[] array, int start, int end)
    {
        //递归的出口（起始值大于或等于终止值的时候，不再执行，return）
        if (start >= end) return;

        //假设第一个元素作为我们的基准
        int pivot = array[start];

        //升序
        //定义两个指针指向我们数组的开头和结尾
        int left = start; //左边为开始
        int right = end; //右边为结束


        //ToDo
        //按照基准排序（小的数放左边，大的数放右边）
        //直到两个数相遇结束排序
        //如果左边小于右边
        while (left < right)
        {
            //从右往左搜索比pivot大的数值
            while (left < right && array[right] >= pivot)
            {
                right--;
            }
            //比pivot小的数值放左边
            array[left] = array[right];
            //从左往右搜索比pivot大的数值
            while (left < right && array[left] <= pivot)
            {
                left++;
            }
            //比pivot大的数值放右边
            array[right] = array[left];
        }
        //跳出循环的时候，left=right
        //左边都比pivot小，右边都比pivot大，将pivot放在下标当前位置
        array[left] = pivot;

        //递归
        QuickSort(array, start, left - 1);
        QuickSort(array, left + 1, end);
    }
}
