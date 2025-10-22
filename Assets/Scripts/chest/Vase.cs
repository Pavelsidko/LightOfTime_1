using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    [SerializeField] private int hitsToDestroy; // Количество попаданий для разрушения
    [SerializeField] private float destroyDelay; // Задержка перед удалением обломков
    [SerializeField] private AudioSource breakSound; // Звук разрушения

    private int currentHits = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeHit()
    {
        currentHits++;
        animator.SetBool("Is_Hit", true);
        breakSound.Play();
        if (currentHits >= hitsToDestroy)
        {
            Debug.Log("Break");
            StartCoroutine(BreakVase());
        }
    }

    IEnumerator BreakVase()
    {
        float animDuration = 0.3f;
        animator.SetBool("Is_Broken", true);
        yield return new WaitForSeconds(animDuration);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
