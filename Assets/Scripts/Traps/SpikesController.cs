using UnityEngine;
using System.Collections;

enum SpikeState
{
    Down,
    Up,
}

public class SpikeController : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Урон от шипов
    [SerializeField] private float damageDelay = 0.5f; // Задержка перед нанесением урона
    [SerializeField] private float cooldownAfterDamage = 1f; // Время до следующего возможного урона
    [SerializeField] private AudioSource SoundEffect;

    private SpikeState currentState = SpikeState.Down;
    private bool playerOnSpikes = false;
    private bool isDamaging = false;
    private float timeOnSpikes = 0f;
    private float lastDamageTime = 0f;
    private Animator animator;



    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnSpikes = true;
            timeOnSpikes = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnSpikes = false;
            isDamaging = false;
        }
    }

    private void Update()
    {
        if (playerOnSpikes && Time.time > lastDamageTime + cooldownAfterDamage)
        {
            timeOnSpikes += Time.deltaTime;

            // Если игрок на шипах достаточно долго и урон еще не начал наноситься
            if (timeOnSpikes >= damageDelay && !isDamaging)
            {
                isDamaging = true;
                SetState(SpikeState.Up);
            }
        }
    }

    private void SetState(SpikeState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case SpikeState.Up:
                animator.SetBool("Is_Up", true);
                SoundEffect.Play();
                MakeDamage();
                break;
            case SpikeState.Down:
                animator.SetBool("Is_Up", false);
                break;
        }
    }

    private void MakeDamage()
    {
        GameController.DamagePlayer(damage);
        lastDamageTime = Time.time;

        // Сбрасываем таймер, даже если урон не был нанесен
        StartCoroutine(SetStateAfterDelay(SpikeState.Down, cooldownAfterDamage));
        isDamaging = false;
        timeOnSpikes = 0f;
    }

    private IEnumerator SetStateAfterDelay(SpikeState newState, float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(newState);
    }

    // Внешний метод, чтобы поднять шипы вручную (например, из другого скрипта или по таймеру)
    public void RaiseSpikes()
    {
        if (currentState == SpikeState.Down)
        {
            SetState(SpikeState.Up);

            //Автоматически перевести в cooldown после damageDelay, если игрока нет — просто опустить шипы
            StartCoroutine(SetStateAfterDelay(SpikeState.Down, cooldownAfterDamage));
        }
    }
}
