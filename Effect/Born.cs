using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    public GameObject player;
    public void CloseBonAnimAndSetActive()
    {
        gameObject.SetActive(false);
        player.SetActive(true);

        player.transform.GetChild(0).gameObject.SetActive(true);
        player.GetComponent<FightCore>().isShield = true;
    }
}
