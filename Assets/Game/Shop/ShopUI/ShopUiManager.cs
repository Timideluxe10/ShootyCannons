using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI gemsText;

    [SerializeField] private Button[] buyButtons;
    [SerializeField] private TextMeshProUGUI[] offerTexts;
    [SerializeField] private TextMeshProUGUI[] priceTexts;

    public void UpdateDynamicSalesUI(SalesManager.Sale[] dynamicSales)
    {
        for(int i = 0; i < dynamicSales.Length; ++i)
        {
            SalesManager.Sale sale = dynamicSales[i];
            buyButtons[i].enabled = true;
            offerTexts[i].text = InventoryManager.Instance.GetGameObjectName(sale.ItemId) + "\n *" + sale.ItemAmount;
            priceTexts[i].text = "" + sale.Price;
        }
    }

    public void UpdateCurrencyTexts(int coins, int gems)
    {
        coinsText.text = "" + coins;
        gemsText.text = "" + gems;
    }

    public void OnItemBought(int indexOfferPanel)
    {
        offerTexts[indexOfferPanel].text = "sold out";
        priceTexts[indexOfferPanel].text = "-";
        buyButtons[indexOfferPanel].enabled = false;
    }
}
