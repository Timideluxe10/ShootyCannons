using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private int maxNumberOfItemsToCollect = 3;

    [SerializeField] private GameObject itemUIManagement;
    private ItemUIManager itemUIManager;

    private CollectableItemController[] collectedItems;

    private List<ItemController> activeItems;

    // Start is called before the first frame update
    void Start()
    {
        collectedItems = new CollectableItemController[maxNumberOfItemsToCollect];

        activeItems = new List<ItemController>();

        itemUIManager = itemUIManagement.GetComponent<ItemUIManager>();
        itemUIManager.InitialiseCollectedItemsPanel(maxNumberOfItemsToCollect);
    }

    public void OnUse(ItemController item)
    {
        ItemController toRemove = null;
        foreach(ItemController activeItem in activeItems)
        {
            if(activeItem.GetEffectType() == item.GetEffectType())
            {
                toRemove = activeItem;
                break;
            }
        }
        if(toRemove != null)
        {
            activeItems.Remove(toRemove);
            toRemove.StopEffect();
        }
        item.gameObject.transform.SetParent(transform); /* Moves the item in the hierarchy to this component to avoid problems with dynamically destroying items that are still active etc. */
        activeItems.Add(item);
        UpdateActiveItemsUI();
    }

    public void OnExpire(ItemController item)
    {
        activeItems.Remove(item);
        UpdateActiveItemsUI();
    }

    // Collect item if there is space. If not, destroy it.
    public void Collect(GameObject collectableItem)
    {
        bool isCollected = Collect(collectableItem.GetComponent<CollectableItemController>());
        if (!isCollected)
            GameObject.Destroy(collectableItem);
        UpdateCollectedItemsUI();
    }

    private void UpdateCollectedItemsUI()
    {
        itemUIManager.UpdateCollectedItemsUI(collectedItems);
    }

    private void UpdateActiveItemsUI()
    {
        itemUIManager.UpdateActiveItemsUI(activeItems);
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
        UpdateCollectedItemsUI();
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
        UpdateCollectedItemsUI();
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
