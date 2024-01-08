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
    bool hasBought = false;
    int costToOpen = 4;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public bool CanBuyChest(int cost)
    {
        if (score >= cost)
        {
            score -= cost;
            text.text = "X" + score.ToString();
            return true;
        }
        else
        {
            return false;
        }
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
