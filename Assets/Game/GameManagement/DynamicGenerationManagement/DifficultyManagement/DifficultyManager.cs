using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DifficultyManager : MonoBehaviour
{
    private static readonly float PointsUntilDifficultyUpdate = 50f; /* Every 'PointsUntilDifficultyUpdate' player points (divided by 'difficultyMultiplier') there will be a shift of difficulties. */

    [SerializeField] private bool isActive = true;
    [SerializeField] private float difficultyMultiplier = 1f;

    [SerializeField] private GameObject dynamicGenerationManagement;
    private DynamicGenerationManager dynamicGenerationManager;

    [SerializeField] private GameObject roomManagement;
    private RoomManager roomManager;

    [Header("Min and max values (number of tickets) for cannons, coins and rooms")]
    [SerializeField] private int minValueCannons = 3;
    [SerializeField] private int maxValueCannons = 10;
    [SerializeField] private int minValueCoins = 1;
    [SerializeField] private int maxValueCoins = 10;
    [SerializeField] private int minValueRooms = 5;
    [SerializeField] private int maxValueRooms = 100;

    private float nextDifficultyStepThreshold;

    // Start is called before the first frame update
    void Start()
    {
        nextDifficultyStepThreshold = PointsUntilDifficultyUpdate / difficultyMultiplier;

        dynamicGenerationManager = dynamicGenerationManagement.GetComponent<DynamicGenerationManager>();
        roomManager = roomManagement.GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        float playerScore = GameController.Instance.GetScore();
        if(playerScore > nextDifficultyStepThreshold)
        {
            nextDifficultyStepThreshold += PointsUntilDifficultyUpdate / difficultyMultiplier;
            AdaptDifficulty();
        }
    }

    private void AdaptDifficulty()
    {
        ShiftProbabilityTickets(dynamicGenerationManager.CannonProbabilityTickets, minValueCannons, maxValueCannons, false);

        ShiftProbabilityTickets(dynamicGenerationManager.CoinProbabilityTickets, minValueCoins, maxValueCoins, false);

        ShiftProbabilityTickets(roomManager.DifficultyProbabilityTickets, minValueRooms, maxValueRooms, true);

        /* DEBUG */
        string cannons = "Cannons: ", coins = "Coins: ", rooms = "Rooms: ";
        for (int i = 0; i < dynamicGenerationManager.CannonProbabilityTickets.Length; ++i)
            cannons += dynamicGenerationManager.CannonProbabilityTickets[i].ToString() + " ";
        for (int i = 0; i < dynamicGenerationManager.CoinProbabilityTickets.Length; ++i)
            coins += dynamicGenerationManager.CoinProbabilityTickets[i].ToString() + " ";
        for (int i = 0; i < roomManager.DifficultyProbabilityTickets.Length; ++i)
            rooms += roomManager.DifficultyProbabilityTickets[i].ToString() + " ";
        Debug.Log(cannons);
        Debug.Log(coins);
        Debug.Log(rooms);
    }

    private void ShiftProbabilityTickets(int[] probabilityTickets, int minValue, int maxValue, bool scaleInfinitely)
    {
        /* Find index of first entry which is greater than minValue. */
        int startIndexToShift = 0;
        for(int i = 0; i < probabilityTickets.Length; ++i)
        {
            startIndexToShift = i;
            if(probabilityTickets[i] > minValue)
            {
                break;
            }
        }

        /* Case: All probability tickets already reached minValue. If scaleInfinitely, keep incrementing last entry, otherwise do nothing and return. */
        if (startIndexToShift == probabilityTickets.Length - 1)
        {
            if(scaleInfinitely)
                probabilityTickets[startIndexToShift]++;
            return;
        }

        int ticketsToShift = (probabilityTickets[startIndexToShift] - 2 < minValue ? 1 : 2);
        probabilityTickets[startIndexToShift] -= ticketsToShift;

        for(int i = startIndexToShift + 1; i < probabilityTickets.Length && ticketsToShift != 0; ++i)
        {
            if(probabilityTickets[i] < maxValue)
            {
                probabilityTickets[i]++;
                ticketsToShift--;
            }
        }
    }

}
