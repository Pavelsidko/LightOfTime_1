using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chest : MonoBehaviour, IPointerClickHandler
{
    public int costToOpen = 5;
    public float range = 3f;

    [SerializeField] private GameObject costBubble;
    [SerializeField] private AudioSource soundEffect;

    private bool hasBeenClicked = false;
    private Animator anim;
    private GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
        costBubble.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // BoxCollider2d
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked!");
        if (!hasBeenClicked && CoinManager.instance.CanBuyChest(costToOpen) && IsPlayerInRange(range))
        {
            hasBeenClicked = true;
            CoinManager.instance.MinusScore(costToOpen);
            anim.SetBool("Opened", true);
            StartCoroutine(PlaySoundAndDestroy());
        }
        else
        {
            Debug.Log("No money or already clicked or not in range");
        }
    }
    // BoxCollider3d
    //private void OnMouseDown()
    //{
    //    if (!hasBeenClicked && CoinManager.instance.CanBuyChest(costToOpen))
    //    {
    //        hasBeenClicked = true;
    //        var lootBag = GetComponent<ChestLootBag>();
    //        StartCoroutine(PlaySoundAndDestroy(lootBag));
    //    }
    //    else
    //    {
    //        Debug.Log("No money or already clicked");
    //    }
    //}

    private void Update()
    {
        if (IsPlayerInRange(range))
        {
            costBubble.SetActive(true);
        }
        else
        {
            costBubble.SetActive(false);
        }
    }

    public bool IsPlayerInRange(float range)
    {
        if (transform == null || player == null || player.transform == null)
        {
            return false;
        }
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    IEnumerator PlaySoundAndDestroy()
    {
        soundEffect.Play();
        yield return new WaitForSeconds(soundEffect.clip.length);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }
}
