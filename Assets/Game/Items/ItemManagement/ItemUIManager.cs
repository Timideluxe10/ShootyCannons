using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [Header("Collectable Items UI")]
    [SerializeField] private GameObject collectableItemsPanelTemplate;
    [SerializeField] private GameObject collectableItemsPanelHolder;

    [Header("Active Items UI")]
    [SerializeField] private GameObject[] activeItemsPanels;
    private Slider[] activeItemsSliders;
    private Text[] activeItemsTexts;
    private ItemController[] activeItems;

    private GameObject[] collectableItemsPanels;
    private Text[] collectableItemsEffectNameTexts;

    private void Start()
    {
        activeItemsSliders = new Slider[activeItemsPanels.Length];
        activeItemsTexts = new Text[activeItemsPanels.Length];
        activeItems = new ItemController[activeItemsPanels.Length];

        for (int i = 0; i < activeItemsPanels.Length; ++i)
        {
            activeItemsPanels[i].SetActive(false);
            activeItemsSliders[i] = activeItemsPanels[i].GetComponentInChildren<Slider>();
            activeItemsTexts[i] = activeItemsPanels[i].GetComponentInChildren<Text>();
        }
    }

    private void Update()
    {
        if(activeItems[0] != null) /* If at least one item is active */
            UpdateActiveItemsDurationBars();
    }

    public void InitialiseCollectedItemsPanel(int number)
    {
        collectableItemsPanels = new GameObject[number];
        collectableItemsEffectNameTexts = new Text[number];

        for (int i = 0; i < number; ++i)
        {
            GameObject itemPanel = GameObject.Instantiate(collectableItemsPanelTemplate);
            itemPanel.transform.SetParent(collectableItemsPanelHolder.transform);
            itemPanel.transform.position = new Vector3(0, -i * 60 - (i+1) * 10 - 100 + 400, 0);
            collectableItemsPanels[i] = itemPanel;
            collectableItemsEffectNameTexts[i] = itemPanel.GetComponentInChildren<Text>();
        }

    }

    public void UpdateCollectedItemsUI(CollectableItemController[] collectedItems)
    {
        
        for (int i = 0; i < collectableItemsPanels.Length; ++i)
        {
            CollectableItemController item = collectedItems[i];
            collectableItemsEffectNameTexts[i].text = item == null ? "" : item.GetName();
        }
    }

    public void UpdateActiveItemsUI(List<ItemController> activeItems)
    {
        int count = 0;
        foreach(ItemController itemController in activeItems)
        {
            if (count >= activeItemsPanels.Length)
                break;
            this.activeItems[count] = itemController;
            activeItemsSliders[count].maxValue = itemController.MaxDuration;
            activeItemsTexts[count].text = itemController.GetName();
            activeItemsPanels[count].SetActive(true);
            ++count;
        }
        for(int i = count; i < activeItemsPanels.Length; ++i)
        {
            activeItemsPanels[i].SetActive(false);
        }
    }

    public void UpdateActiveItemsDurationBars()
    {
        for(int i = 0; i < activeItems.Length; ++i)
        {
            if (activeItems[i] == null)
                return;
            activeItemsSliders[i].value = activeItems[i].DurationLeft;
        }
    }
}
