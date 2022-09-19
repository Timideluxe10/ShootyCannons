using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text coinText;

    private int coinsCollected;

    public void CoinCollected(int value)
    {
        coinsCollected += value;
        coinText.text = coinsCollected.ToString();
    }
}
