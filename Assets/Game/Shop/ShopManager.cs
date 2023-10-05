using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

    private DateTime currentDate, oldDate;

    private void Start()
    {
        shopUiManager = shopUiManagement.GetComponent<ShopUiManager>();
        salesManager = salesManagement.GetComponent<SalesManager>();

        dynamicSales = new SalesManager.Sale[numberOfDynamicSales];

        UpdateCurrencyUI();

        ManageResetTimer();
    }

    private void ManageResetTimer()
    {
        currentDate = System.DateTime.Now;

        string lastLoginTime = PlayerPrefs.GetString("lastLoginTime");
        long binaryTime = (lastLoginTime == null || lastLoginTime == "") ? 0 : Convert.ToInt64(lastLoginTime);
        oldDate = (binaryTime == 0) ? DateTime.MinValue : DateTime.FromBinary(binaryTime);

        TimeSpan difference = currentDate.Subtract(oldDate);
        int differenceInSeconds = (int)difference.TotalSeconds;

        print("difference in seconds: " + differenceInSeconds);

        if (differenceInSeconds >= shopRefreshTimeInSeconds)
        {
            GenerateNewSales();
            secondsUntilShopReset = shopRefreshTimeInSeconds;
        }
        else
        {
            secondsUntilShopReset = shopRefreshTimeInSeconds - differenceInSeconds;
            for (int i = 0; i < dynamicSales.Length; ++i)
            {
                dynamicSales[i] = new SalesManager.Sale(
                    PlayerPrefs.GetInt("shopItemId" + i, 0),
                    PlayerPrefs.GetInt("shopItemAmount" + i, 0),
                    PlayerPrefs.GetInt("shopItemPrice" + i, 0)
                );
            }
            UpdateDynamicSalesUI();
        }

        print("Seconds until reset:" + secondsUntilShopReset);
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
            PlayerPrefs.SetInt("shopItemId" + i, dynamicSales[i].ItemId);
            PlayerPrefs.SetInt("shopItemAmount" + i, dynamicSales[i].ItemAmount);
            PlayerPrefs.SetInt("shopItemPrice" + i, dynamicSales[i].Price);
        }
        PlayerPrefs.SetString("lastLoginTime", System.DateTime.Now.ToBinary().ToString());
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

        GenerateNewSale(indexOfSale); // Generating a new sale overwrites "onItemBought" functionality and UI. Open design choice if leaving like this.
    }

    private void GenerateNewSale(int indexOfSale)
    {
        dynamicSales[indexOfSale] = salesManager.GenerateSale();
        PlayerPrefs.SetInt("shopItemId" + indexOfSale, dynamicSales[indexOfSale].ItemId);
        PlayerPrefs.SetInt("shopItemAmount" + indexOfSale, dynamicSales[indexOfSale].ItemAmount);
        PlayerPrefs.SetInt("shopItemPrice" + indexOfSale, dynamicSales[indexOfSale].Price);
        PlayerPrefs.SetString("lastLoginTime", System.DateTime.Now.ToBinary().ToString());
        UpdateDynamicSalesUI();
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
