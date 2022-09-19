using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("Adapting difficulty...");
        }
    }

    private void AdaptDifficulty()
    {
        ShiftProbabilityTickets(dynamicGenerationManager.CannonProbabilityTickets, minValueCannons, maxValueCannons, false);
        Debug.Log("Cannon probabilities shifted: " + dynamicGenerationManager.CannonProbabilityTickets);

        ShiftProbabilityTickets(dynamicGenerationManager.CoinProbabilityTickets, minValueCoins, maxValueCoins, true);
        Debug.Log("Coin probabilities shifted: " + dynamicGenerationManager.CoinProbabilityTickets);

        ShiftProbabilityTickets(roomManager.DifficultyProbabilityTickets, minValueRooms, maxValueRooms, true);
        Debug.Log("Room probabilities shifted: " + roomManager.DifficultyProbabilityTickets);

    }

    private void ShiftProbabilityTickets(int[] probabilityTickets, int minValue, int maxValue, bool scaleInfinitely)
    {
        /* Find index of first entry which is greater than minValue. */
        int startIndexToShift = 0;
        for(int i = 0; i < probabilityTickets.Length; ++i)
        {
            startIndexToShift = i;
            if(probabilityTickets[i] >= minValue)
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
