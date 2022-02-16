using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    void CloseAnimAndCancelIsShield()
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.GetComponent<FightCore>().isShield = false;
    }
}
