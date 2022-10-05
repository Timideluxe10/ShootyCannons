using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int numInventorySlots = 15;
    [SerializeField] private GameObject inventoryUiManagement;

    private static InventoryManager instance;

    private MonoBehaviour[] collectedObjects;
    private InventoryUiManager inventoryUiManager;


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
    }

    private void Start()
    {
        collectedObjects = new MonoBehaviour[numInventorySlots];
        inventoryUiManager = inventoryUiManagement.GetComponent<InventoryUiManager>();
    }

    public bool CanBeCollected(int numberOfObjects)
    {
        int numFreeSlots = 0;
        for(int i = 0; i < collectedObjects.Length; ++i)
        {
            if(collectedObjects[i] == null)
            {
                if (++numFreeSlots >= numberOfObjects)
                    return true;
            }
        }
        return false;
    }

    public bool Add(MonoBehaviour toCollect)
    {
        for(int i = 0; i < collectedObjects.Length; ++i)
        {
            if(collectedObjects[i] == null)
            {
                collectedObjects[i] = toCollect;
                return true;
            }
        }
        return false;
    }

    public bool Remove(MonoBehaviour toRemove)
    {
        for (int i = 0; i < collectedObjects.Length; ++i)
        {
            if (collectedObjects[i] == toRemove)
            {
                collectedObjects[i] = null;
                return true;
            }
        }
        return false;
    }




}
