using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject BulletSign;
    public int indexJianFa = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShiftOption();

    }
    void ShiftOption()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (indexJianFa != 0)
            {
                BulletSign.transform.position += Vector3.up;
            }
            indexJianFa = Mathf.Clamp(indexJianFa - 1, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (indexJianFa != 1)
            {
                BulletSign.transform.position += Vector3.down;
            }
            indexJianFa = Mathf.Clamp(indexJianFa + 1, 0, 1);

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (indexJianFa)
            {
                case 0:
                    DatePlayerCanControll.Instance.playerCount = 1;
                    SceneManager.LoadScene("TwoPlayerMode");
                    break;
                case 1:
                    DatePlayerCanControll.Instance.playerCount = 2;
                    SceneManager.LoadScene("TwoPlayerMode");
                    //加载多人模式场景   //记得设置过场动画。
                    break;
                default:
                    break;
            }
        }
    }
}
