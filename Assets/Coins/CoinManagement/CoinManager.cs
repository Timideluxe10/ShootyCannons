using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Text coinText;

    private int coinsCollected;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CoinCollected(int value)
    {
        coinsCollected += value;
        coinText.text = coinsCollected.ToString();
    }
}
