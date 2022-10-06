using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static readonly int Num_Equip_Slots = 3;

    public class ItemSlot
    {
        private int itemId;
        private int amount;

        public ItemSlot(int itemId, int amount)
        {
            this.itemId = itemId;
            this.amount = amount;
        }

        public int ItemId { get => itemId;  }
        public int Amount { get => amount; set => amount = value; }
    }


    [SerializeField] private GameObject inventoryUiManagement;
    [SerializeField] private GameObject itemCreationManagement;

    private static InventoryManager instance;

    private List<ItemSlot> itemSlots;
    private int[] equippedItemIds = new int[Num_Equip_Slots];

    private InventoryUiManager inventoryUiManager;
    private ItemFactory itemFactory;


    public static InventoryManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            GameObject.Destroy(gameObject);

        itemSlots = new List<ItemSlot>();
        inventoryUiManager = inventoryUiManagement.GetComponent<InventoryUiManager>();
        itemFactory = itemCreationManagement.GetComponent<ItemFactory>();
    }

    private void Start()
    {
        /* Test */
        Add(1, 5);
        Add(2, 3);
        Add(3, 1);
        Add(4, 2);
        Add(5, 3);
        Add(6, 3);
        Add(7, 3);
        Add(8, 3);
        UpdateInventoryUI();
    }

    public void Add(int itemIdToCollect, int amount)
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            if(itemIdToCollect == itemSlot.ItemId)
            {
                itemSlot.Amount += amount;
                return; ;
            }
        }
        itemSlots.Add(new ItemSlot(itemIdToCollect, amount));
    }

    public bool Remove(int itemIdToRemove)
    {
        ItemSlot toRemove = null;
        bool wasItemRemoved = false;
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemIdToRemove == itemSlot.ItemId)
            {
                itemSlot.Amount--;
                if (itemSlot.Amount <= 0)
                    toRemove = itemSlot;
                wasItemRemoved = true;
                break;
            }
        }
        if (toRemove != null)
            itemSlots.Remove(toRemove);
        return wasItemRemoved;
    }

    public bool Equip(int itemId)
    {
        for(int i = 0; i < equippedItemIds.Length; ++i)
        {
            if(equippedItemIds[i] == 0)
            {
                if (Remove(itemId))
                {
                    equippedItemIds[i] = itemId;
                    return true;
                }
            }
        }
        return false;
    }

    public void Unequip(int indexEquippedItems)
    {
        int itemId = equippedItemIds[indexEquippedItems];
        if(itemId == 0)
        {
            return;
        }
        else
        {
            equippedItemIds[indexEquippedItems] = 0;
            Add(itemId, 1);
            UpdateInventoryUI();
            return;
        }
    }

    public void UpdateInventoryUI()
    {
        inventoryUiManager.UpdateUI(itemSlots, equippedItemIds);
    }

    public List<GameObject> GetItemsAsInstantiatedGameObjects()
    {
        List<GameObject> instantiatedItems = new List<GameObject>();
        for(int i = 0; i < equippedItemIds.Length; ++i)
        {
            if(equippedItemIds[i] != 0)
            {
                GameObject template = itemFactory.GetGameObjectTemplate(equippedItemIds[i]);
                instantiatedItems.Add(GameObject.Instantiate(template, new Vector3(-100, 0, 0), template.transform.rotation));
                equippedItemIds[i] = 0;
            }
        }
        return instantiatedItems;
    }

    public void ResetDependencies()
    {
        inventoryUiManager.ResetDependencies();
    }
}