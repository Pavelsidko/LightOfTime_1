using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject winGameMenu;

    private void OnEnable()
    {
        EnemyController.OnBossDestroyed += HandleBossDestroyed;
    }

    private void OnDisable()
    {
        EnemyController.OnBossDestroyed -= HandleBossDestroyed;
    }

    private void Start()
    {
        StartCoroutine(CheckForBoss());
    }

    private IEnumerator CheckForBoss()
    {
        yield return new WaitForSeconds(5f);

        if (DoesBossExist())
        {
            winGameMenu.SetActive(false);
        }
        else
        {
            winGameMenu.SetActive(true);
        }
    }

    bool DoesBossExist()
    {
        return GameObject.FindGameObjectWithTag("Boss") != null;
    }

    private void HandleBossDestroyed()
    {
        winGameMenu.SetActive(true);
    }
}
