using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    public GameObject pauseMenu;
    public static bool isPaused;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Обязательно снимаем паузу при старте сцены
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }

    public void SetPaused(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(pause);
    }

    public void SetPaused(bool pause, bool showPauseMenu)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(pause && showPauseMenu);
    }

    public void PauseGame() => SetPaused(true);
    public void ResumeGame() => SetPaused(false);
}
