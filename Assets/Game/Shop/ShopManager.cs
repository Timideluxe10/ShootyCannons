using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopUiManagement;
    private ShopUiManager shopUiManager;

    [SerializeField] private GameObject salesManagement;
    private SalesManager salesManager;

    private void Start()
    {
        shopUiManager = shopUiManagement.GetComponent<ShopUiManager>();
        salesManager = salesManagement.GetComponent<SalesManager>();
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
