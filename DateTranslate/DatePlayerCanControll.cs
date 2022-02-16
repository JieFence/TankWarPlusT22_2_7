using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatePlayerCanControll : MonoBehaviour
{
    // Start is called before the first frame update
    public static DatePlayerCanControll Instance { get; private set; }
    public int WallDensity = 21;//随意
    public int enemyTotalCount = 10;//随意
    public int enemyTotalWaves = 3;//随意
    public int playerCount;
    public bool isGameOver_Win = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
