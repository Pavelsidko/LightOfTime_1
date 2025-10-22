using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZoneController : MonoBehaviour
{

    public AudioSource audioSource;
    public float maxHearingDistance = 5f;
    public float minVolume = 0.1f;      

    public Transform player; 

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= maxHearingDistance)
        {
            float volume = 1 - (distance / maxHearingDistance);
            audioSource.volume = volume;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime * 5f);
            if (audioSource.volume < 0.01f)
                audioSource.Stop();
        }
    }
}
