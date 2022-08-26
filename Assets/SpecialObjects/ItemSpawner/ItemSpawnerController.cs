using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{
    [SerializeField] private float itemSpawningProbability = 0.3f;

    [SerializeField] private GameObject[] itemTemplates;
    [SerializeField] private int[] probabilityTickets;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.value <= itemSpawningProbability)
        {
            GameObject toInstantiate = ChooseRandomTemplate(itemTemplates, probabilityTickets);
            GameObject item = GameObject.Instantiate(toInstantiate, transform.position, toInstantiate.transform.rotation);
            item.transform.SetParent(transform);
        }
    }


    /* ---------------------------------------------------------------------------------------------------------------------------------------------------- */


    private GameObject ChooseRandomTemplate(GameObject[] templates, int[] probabilityTickets)
    {
        int sumTickets = sumProbabilityTickets(probabilityTickets);
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        for (int i = 0; i < templates.Length; ++i)
        {
            ticketCount += probabilityTickets[i];
            if (ticketCount >= randomTicket)
            {
                return templates[i];
            }
        }
        return null; /* Should never happen. */
    }

    // Return total number of probability tickets
    private int sumProbabilityTickets(int[] probabilityTickets)
    {
        int sum = 0;
        for (int i = 0; i < probabilityTickets.Length; ++i)
        {
            sum += probabilityTickets[i];
        }
        return sum;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .3f);
    }
}
