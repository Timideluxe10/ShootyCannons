using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private int maxNumberOfItemsToCollect = 3;

    [SerializeField] private GameObject itemUIManagement;
    private ItemUIManager itemUIManager;

    private CollectableItemController[] collectedItems;

    // Start is called before the first frame update
    void Start()
    {
        collectedItems = new CollectableItemController[maxNumberOfItemsToCollect];

        itemUIManager = itemUIManagement.GetComponent<ItemUIManager>();
        itemUIManager.InitialiseItemPanels(maxNumberOfItemsToCollect);
    }

    // Collect item if there is space. If not, destroy it.
    public void Collect(GameObject collectableItem)
    {
        bool isCollected = Collect(collectableItem.GetComponent<CollectableItemController>());
        if (!isCollected)
            GameObject.Destroy(collectableItem);
        UpdateUI();
    }

    private void UpdateUI()
    {
        itemUIManager.UpdateCollectedItemsUI(collectedItems);
    }

    private bool Collect(CollectableItemController item)
    {
        for (int i = collectedItems.Length - 1; i >= 0; --i)
        {
            if (collectedItems[i] == null)
            {
                collectedItems[i] = item;
                return true;
            }
        }
        return false;
    }

    public void UseItem()
    {
        CollectableItemController firstItem = GetFirstItem(out int indexOfFirstItem);
        if(firstItem == null)
        {
            // TODO: implement functionality to show user that no item is being held.
            return;
        }
        firstItem.StartEffect();
        collectedItems[indexOfFirstItem] = null;
        itemUIManager.UpdateCollectedItemsUI(collectedItems);
        UpdateUI();
    }

    public void DiscardItem()
    {
        CollectableItemController lastItem = collectedItems[collectedItems.Length - 1];
        if(lastItem == null)
        {
            // TODO: implement functionality to show user that no item is being held.
            return;
        }
        GameObject.Destroy(lastItem.gameObject);
        ShiftCollectedItemsArray();
        UpdateUI();
    }

    private CollectableItemController GetFirstItem(out int indexOfFirstItem)
    {
        for(int i = 0; i < collectedItems.Length; ++i)
        {
            if (collectedItems[i] != null)
            {
                indexOfFirstItem = i;
                return collectedItems[i];
            }
        }
        indexOfFirstItem = -1;
        return null;
    }

    private void ShiftCollectedItemsArray()
    {
        for(int i = collectedItems.Length - 1; i > 0; --i)
        {
            collectedItems[i] = collectedItems[i - 1];
            collectedItems[i - 1] = null;
        }
    }
}
