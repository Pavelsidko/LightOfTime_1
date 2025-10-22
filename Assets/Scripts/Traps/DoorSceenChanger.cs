using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class DoorSceenChanger : MonoBehaviour
{
    //[SerializeField] private string nextSceneName; // Имя следующей сцены
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private LevelLoader levelLoader;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            //SceneManager.LoadScene(nextSceneName);
            Debug.Log("Player reached tower");
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
    IEnumerator LoadLevel(int index)
    {
        levelLoader.FadeOut();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }
}
