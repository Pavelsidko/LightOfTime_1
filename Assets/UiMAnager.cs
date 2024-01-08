using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMAnager : MonoBehaviour
{
    public GameObject gameOverMenu;
    

    private void OnEnable()
    {
        GameController.onPlayerDeath += EnableGameOverMenu;
    }
    private void OnDisable()
    {
        GameController.onPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        WipeProgress();


    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        WipeProgress();


    }

    private void WipeProgress()
    {
        GameController.Health = GameController.MaxHealth;
        GameController.MoveSpeed = 5f;
        GameController.FireRate = 0.6f;
        GameController.BulletSize = 0.5f;
        CoinManager.instance.score = 0;
    }

}
