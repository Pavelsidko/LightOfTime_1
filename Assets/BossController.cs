using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public GameObject winGameMenu;

    private void Update()
    {
        if (DoesBossExist())
        {
            winGameMenu.SetActive(false);
        }
        else { winGameMenu.SetActive(true); }
    }

    bool DoesBossExist()
    {
        return GameObject.FindGameObjectWithTag("Boss") != null;
    }
}