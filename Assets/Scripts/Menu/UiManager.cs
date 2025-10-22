using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class UiManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    [SerializeField] private LevelLoader levelLoader;
    private float transitionTime = 0.1f;
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //WipeProgress();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void GoToMainMenu()
    {
        //levelLoader.FadeOut();
        //SceneManager.LoadScene(0);
        StartCoroutine(LoadLevel(0));
        //WipeProgress();
    }

    private void WipeProgress()
    {
        GameController.Health = GameController.MaxHealth;
        GameController.MoveSpeed = 5f;
        GameController.FireRate = 0.6f;
        GameController.BulletSize = 0.5f;
        CoinManager.instance.score = 0;
    }

    IEnumerator LoadLevel(int index)
    {
        levelLoader.FadeOut();
        yield return new WaitForSecondsRealtime(transitionTime);
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }
}
