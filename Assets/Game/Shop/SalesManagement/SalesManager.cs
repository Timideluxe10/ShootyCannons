using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] itemTemplatesForSale;
    [SerializeField] private int[] itemPrices;
    [SerializeField] private int[] probabilityTickets;

    private int GenerateSaleIndex()
    {
        int indexForSale = ChooseRandomIndex();
        return indexForSale;
    }


    // Return random index (probabilities in probabilityTickets).
    private int ChooseRandomIndex()
    {
        int sumTickets = sumProbabilityTickets(probabilityTickets);
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        for (int i = 0; i < probabilityTickets.Length; ++i)
        {
            ticketCount += probabilityTickets[i];
            if (ticketCount >= randomTicket)
            {
                return i;
            }
        }
        return -1; /* Should never happen. */
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

}
