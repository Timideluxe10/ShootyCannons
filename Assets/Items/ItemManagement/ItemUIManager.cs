using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPanelTemplate;
    [SerializeField] private GameObject itemPanelHolder;

    private GameObject[] itemPanels;
    private Text[] effectNameTexts;

    public void InitialiseItemPanels(int number)
    {
        itemPanels = new GameObject[number];
        effectNameTexts = new Text[number];

        for (int i = 0; i < number; ++i)
        {
            GameObject itemPanel = GameObject.Instantiate(itemPanelTemplate, Vector3.zero, itemPanelTemplate.transform.rotation);
            itemPanel.transform.SetParent(itemPanelHolder.transform);
            itemPanel.transform.position = new Vector3(0, -i * 50 + 312, 0);
            itemPanels[i] = itemPanel;
            effectNameTexts[i] = itemPanel.GetComponentInChildren<Text>();
        }

    }

    public void UpdateCollectedItemsUI(CollectableItemController[] collectedItems)
    {
        
        for (int i = 0; i < itemPanels.Length; ++i)
        {
            CollectableItemController item = collectedItems[i];
            effectNameTexts[i].text = item == null ? "" : item.GetEffectName();
        }
    }
}
