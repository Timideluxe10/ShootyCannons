using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjectTemplates;

    private Dictionary<int, GameObject> idsTogameObjectTemplates = new Dictionary<int, GameObject>();

    private void Start()
    {
        int id = 1;
        foreach(GameObject gameObjectTemplate in gameObjectTemplates)
        {
            idsTogameObjectTemplates.Add(id++, gameObjectTemplate);
        }
    }

    public GameObject GetGameObjectTemplate(int id)
    {
        return id == 0 ? null : idsTogameObjectTemplates[id];
    }

    public string GetGameObjectName(int id)
    {
        // return id == 0 ? "" : idsTogameObjectTemplates[id].name;
        return id == 0 ? "" : idsTogameObjectTemplates[id].GetComponent<ItemController>().GetName();
    }

    public List<int> GetValidIds()
    {
        return new List<int>(idsTogameObjectTemplates.Keys);
    }
}
