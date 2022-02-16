using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOut : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bg1;
    public GameObject bg2;
    private Image bg2_image;
    public float alpha;

    private bool hasArrive1 = false;
    private bool hasArrive2 = false;

    void Start()
    {
        bg2_image = bg2.GetComponent<Image>();
        bg1.SetActive(false);
        bg2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        alpha += Time.deltaTime * 0.2f;
        bg2_image.color = new Color(0.26f, 0.2f, 0.2f, alpha);

        if (alpha >= 0.3f && !hasArrive1)
        {
            hasArrive1 = true;
            bg1.SetActive(true);

        }
        if (alpha >= 0.75f && !hasArrive2)
        {
            hasArrive2 = true;
            gameObject.SetActive(false);
        }



    }
}
