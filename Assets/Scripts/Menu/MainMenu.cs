using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    public float transitionTime = 1f;

    public void PlayGame()
    {

       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    IEnumerator LoadLevel(int index)
    {
        levelLoader.FadeOut();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }
}
