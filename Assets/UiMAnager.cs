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
        GameController.Health = GameController.MaxHealth;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        GameController.Health = GameController.MaxHealth;
    }

}
