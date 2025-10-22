using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TextMeshProUGUI text;
    public int score;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool CanBuyChest(int cost)
    {
        return score >= cost ? true : false;
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        text.text = "X" + score.ToString();
    }

    public void MinusScore(int coinValue)
    {
        score -= coinValue;
        text.text = "X" + score.ToString();

    }

    public int GetScore()
    {
        return score;
    }
}
