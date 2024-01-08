using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    public int costToOpen = 5;

    [SerializeField] private AudioSource shootSoundEffect;
    private bool hasBeenClicked = false;

    private void OnMouseDown()
    {
        if (!hasBeenClicked && CoinManager.instance.CanBuyChest(costToOpen))
        {
            hasBeenClicked = true;
            var lootBag = GetComponent<ChestLootBag>();
            StartCoroutine(PlaySoundAndDestroy(lootBag));
        }
        else
        {
            Debug.Log("No money or already clicked");
        }
    }
    IEnumerator PlaySoundAndDestroy(ChestLootBag lootBag)
    {
        shootSoundEffect.Play();
        yield return new WaitForSeconds(shootSoundEffect.clip.length);
        lootBag.InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
