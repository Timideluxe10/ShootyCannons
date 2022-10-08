using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private int numberOfDynamicSales = 3;

    [SerializeField] private float shopRefreshTimeInSeconds = 60;
    private float secondsUntilShopReset;

    [SerializeField] private GameObject shopUiManagement;
    private ShopUiManager shopUiManager;

    [SerializeField] private GameObject salesManagement;
    private SalesManager salesManager;

    private SalesManager.Sale[] dynamicSales;

    private void Start()
    {
        PlayerPrefs.SetInt("Coins", 1000); /* For testing */

        secondsUntilShopReset = shopRefreshTimeInSeconds;

        shopUiManager = shopUiManagement.GetComponent<ShopUiManager>();
        salesManager = salesManagement.GetComponent<SalesManager>();

        dynamicSales = new SalesManager.Sale[numberOfDynamicSales];

        UpdateCurrencyUI();
        GenerateNewSales();
    }

    private void Update()
    {
        secondsUntilShopReset -= Time.deltaTime;
        if(secondsUntilShopReset <= 0)
        {
            secondsUntilShopReset = shopRefreshTimeInSeconds;
            GenerateNewSales();
        }
    }

    private void GenerateNewSales()
    {
        for (int i = 0; i < dynamicSales.Length; ++i)
        {
            dynamicSales[i] = salesManager.GenerateSale();
        }
        UpdateDynamicSalesUI();
    }

    public void OnDynamicSalesItemBought(int indexOfSale)
    {
        SalesManager.Sale sale = dynamicSales[indexOfSale];
        int playerCoins = PlayerPrefs.GetInt(FileManager.COINS, 0);
        if(playerCoins < sale.Price)
        {
            return; /* Player doesn't have enough coins. TODO: Disable button beforhand or play sound. */
        }
        PlayerPrefs.SetInt(FileManager.COINS, playerCoins - sale.Price);
        InventoryManager.Instance.Add(sale.ItemId, sale.ItemAmount);

        UpdateCurrencyUI();
        shopUiManager.OnItemBought(indexOfSale);
    }

    public void UpdateDynamicSalesUI()
    {
        shopUiManager.UpdateDynamicSalesUI(dynamicSales);
    }

    public void UpdateCurrencyUI()
    {
        shopUiManager.UpdateCurrencyTexts(
            PlayerPrefs.GetInt(FileManager.COINS, 0),
            PlayerPrefs.GetInt(FileManager.GEMS, 0));
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void OnSalesRefresh()
    {
        GenerateNewSales();
    }


}
