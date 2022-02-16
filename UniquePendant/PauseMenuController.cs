using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public int indexJianFa_PauseMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            indexJianFa_PauseMenu = Mathf.Clamp(indexJianFa_PauseMenu - 1, 0, 1);
            ToActive();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            indexJianFa_PauseMenu = Mathf.Clamp(indexJianFa_PauseMenu + 1, 0, 1);
            ToActive();
        }
    }
    void ToActive()
    {
        switch (indexJianFa_PauseMenu)
        {
            case 0:
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
                break;
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1f;
                break;
            default:
                break;
        }
    }
}
