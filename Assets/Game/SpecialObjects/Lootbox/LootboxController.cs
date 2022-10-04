using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootboxController : MonoBehaviour
{
    [SerializeField] private int minCoins, maxCoins;
    [SerializeField] private int coinDropProbabilityTickets;
    [SerializeField] private GameObject coinTemplate;

    [SerializeField] private GameObject[] itemDrops;
    [SerializeField] private int[] dropProbabilityTickets;

    public class Drop
    {
        GameObject item;
        int amount;

        public Drop(GameObject item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public GameObject Item { get => item; }
        public int Amount { get => amount; }
    }

    public Drop Open()
    {
        // GameObject.Destroy(this);
        return ChooseRandomDrop();
    }

    // Return random template (probabilities in probabilityTickets)
    private Drop ChooseRandomDrop()
    {
        int sumTickets = sumProbabilityTickets();
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        for (int i = 0; i < itemDrops.Length; ++i)
        {
            ticketCount += dropProbabilityTickets[i];
            if (ticketCount >= randomTicket)
            {
                return new Drop(itemDrops[i], 1);
            }
        }
        return new Drop(coinTemplate, (int) Random.Range(minCoins, maxCoins) + 1); /* Coin drop if no item rolled. */
    }

    // Return total number of probability tickets
    private int sumProbabilityTickets()
    {
        int sum = coinDropProbabilityTickets; /* Include probability tickets for coin drop. */
        for (int i = 0; i < dropProbabilityTickets.Length; ++i)
        {
            sum += dropProbabilityTickets[i];
        }
        return sum;
    }

}

