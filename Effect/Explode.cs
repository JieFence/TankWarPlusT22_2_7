using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    void CloseAnimAndCloseTank()
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        if (gameObject.transform.parent.gameObject.CompareTag("Bullet") || gameObject.transform.parent.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject.transform.parent.gameObject);
        }

    }
}
