using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public GameObject Slider_WallDensity;
    public GameObject Slider_enemyTotalCount;
    public GameObject Slider_enemyTotalWaves;
    public GameObject Text_WallDensity;
    public GameObject Text_enemyTotalCount;
    public GameObject Text_enemyTotalWaves;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DateSynchronization();
        Stylization();
    }
    void DateSynchronization()
    {
        DatePlayerCanControll.Instance.WallDensity = (int)Slider_WallDensity.GetComponent<Slider>().value;
        Text_WallDensity.GetComponent<Text>().text = ((int)Slider_WallDensity.GetComponent<Slider>().value).ToString();
        DatePlayerCanControll.Instance.enemyTotalCount = (int)Slider_enemyTotalCount.GetComponent<Slider>().value;
        Text_enemyTotalCount.GetComponent<Text>().text = ((int)Slider_enemyTotalCount.GetComponent<Slider>().value).ToString();
        DatePlayerCanControll.Instance.enemyTotalWaves = (int)Slider_enemyTotalWaves.GetComponent<Slider>().value;
        Text_enemyTotalWaves.GetComponent<Text>().text = ((int)Slider_enemyTotalWaves.GetComponent<Slider>().value).ToString();
    }
    void Stylization()
    {
        float temp_H = -0.004f * (Slider_enemyTotalCount.GetComponent<Slider>().value) + 0.4f;
        Text_enemyTotalCount.GetComponent<Text>().color = Color.HSVToRGB(temp_H, 1, 1);
    }
}
