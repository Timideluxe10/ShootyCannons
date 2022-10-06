using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUiManager : MonoBehaviour
{
    [SerializeField] private Button itemButtonTemplate;
    [SerializeField] private TextMeshProUGUI itemAmountTextTemplate;

    [SerializeField] private GameObject scrollViewContent;
    [SerializeField] private Button[] equippedItemButtons;

    [SerializeField] private GameObject itemCreationManagement;
    private ItemFactory itemFactory;

    [SerializeField] private int widthOffset, heightOffset;
    private int widthDistance;
    private int heightDistance;

    private List<Button> itemButtons = new List<Button>();
    private List<TextMeshProUGUI> itemAmountTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        widthDistance = (int) (itemButtonTemplate.GetComponent<RectTransform>().rect.width + itemAmountTextTemplate.GetComponent<RectTransform>().rect.width + widthOffset);
        heightDistance = (int) (itemButtonTemplate.GetComponent<RectTransform>().rect.height + heightOffset);

        itemFactory = itemCreationManagement.GetComponent<ItemFactory>();
    }

    public void UpdateUI(List<InventoryManager.ItemSlot> itemSlots, int[] equippedItemIds)
    {
        UpdateItemButtons(itemSlots);
        UpdateEquippedItemButtons(equippedItemIds);
    }

    private void UpdateItemButtons(List<InventoryManager.ItemSlot> itemSlots)
    {
        ResetItemUI();
        int i = 0;
        foreach(InventoryManager.ItemSlot itemSlot in itemSlots)
        {
            itemButtons.Add(InstantiateItemButton(itemSlot.ItemId, i));
            itemAmountTexts.Add(InstantiateItemAmountText(itemSlot.Amount, i));
            i++;
        }
    }

    private Button InstantiateItemButton(int itemId, int positionIndex)
    {
        int xPos = (positionIndex % 2) * widthDistance;
        int yPos = (positionIndex / 2) * -heightDistance;

        Button itemButton = GameObject.Instantiate(itemButtonTemplate, new Vector3(xPos, yPos, 0), itemButtonTemplate.transform.rotation, scrollViewContent.transform);
        itemButton.transform.localPosition = new Vector3(xPos, yPos, 0);
        itemButton.GetComponentInChildren<TextMeshProUGUI>().text = itemFactory.GetGameObjectName(itemId);

        itemButton.onClick.AddListener(() => InventoryManager.Instance.Equip(itemId));
        itemButton.onClick.AddListener(() => InventoryManager.Instance.UpdateInventoryUI());

        return itemButton;
    }

    private TextMeshProUGUI InstantiateItemAmountText(int amount, int positionIndex)
    {
        int xPos = (positionIndex % 2) * widthDistance + (int) itemButtonTemplate.GetComponent<RectTransform>().rect.width;
        int yPos = (positionIndex / 2) * -heightDistance - (int) (itemButtonTemplate.GetComponent<RectTransform>().rect.height / 3);

        TextMeshProUGUI itemAmountText = GameObject.Instantiate(itemAmountTextTemplate, new Vector3(xPos, yPos, 0), itemAmountTextTemplate.transform.rotation, scrollViewContent.transform);
        itemAmountText.transform.localPosition = new Vector3(xPos, yPos, 0);
        itemAmountText.text = "*" + amount;

        return itemAmountText;
    }

    private void ResetItemUI()
    {
        for(int i = 0; i < itemButtons.Count; ++i)
        {
            if(itemButtons[i] != null)
                GameObject.Destroy(itemButtons[i].gameObject);
            if (itemAmountTexts[i] != null)
                GameObject.Destroy(itemAmountTexts[i].gameObject);
        }
        itemButtons = new List<Button>();
        itemAmountTexts = new List<TextMeshProUGUI>();
    }

    private void UpdateEquippedItemButtons(int[] equippedItemIds)
    {
        for(int i = 0; i < equippedItemButtons.Length; ++i)
        {
            int itemId = equippedItemIds[i];
            equippedItemButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = itemFactory.GetGameObjectName(itemId);
        }
    }

    public void ResetDependencies()
    {
        equippedItemButtons = new Button[3];
        for(int i = 0; i < equippedItemButtons.Length; ++i)
        {
            equippedItemButtons[i] = GameObject.Find("EquippedItemButton" + (i+1)).GetComponent<Button>();
            int indexToUnequip = i;
            equippedItemButtons[i].onClick.AddListener(() => InventoryManager.Instance.Unequip(indexToUnequip));
        }
        scrollViewContent = GameObject.Find("Content");
        ResetItemUI();
    }
}
