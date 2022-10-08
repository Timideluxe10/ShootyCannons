using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesManager : MonoBehaviour
{
    [SerializeField] private int maxAmount = 3;
    [SerializeField] private float discountProbability = 0.3f;
    [SerializeField] private float maxDiscount = 0.5f;

    [SerializeField] private int[] itemIdsForSale;
    [SerializeField] private int[] itemPrices;
    [SerializeField] private int[] probabilityTickets;

    public class Sale
    {
        private int itemId;
        private int itemAmount;
        private int price;

        public Sale(int itemId, int itemAmount, int price)
        {
            this.itemId = itemId;
            this.itemAmount = itemAmount;
            this.price = price;
        }

        public int ItemId { get => itemId; }
        public int ItemAmount { get => itemAmount; }
        public int Price { get => price; }
    }

    public Sale GenerateSale()
    {
        int indexForSale = ChooseRandomIndex();
        int itemAmount = (int)(Random.value * maxAmount) + 1;
        int price = itemAmount * itemPrices[indexForSale];
        if(Random.value <= discountProbability)
        {
            float discount = Random.Range(0f, maxDiscount);
            price = (int) (((float)price) * (1f - discount));
        }
        return new Sale(itemIdsForSale[indexForSale], itemAmount, price);
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
