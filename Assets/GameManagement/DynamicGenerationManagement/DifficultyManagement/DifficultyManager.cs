using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private static readonly float PointsUntilDifficultyUpdate = 50f; /* Every 'PointsUntilDifficultyUpdate' player points (times 'difficultyMultiplier') there will be a shift of difficulties. */

    [SerializeField] private bool isActive = true;
    [SerializeField] private float difficultyMultiplier = 1f;
    private float nextDifficultyStepThreshold;

    [SerializeField] private GameObject dynamicGenerationManagement;
    private DynamicGenerationManager dynamicGenerationManager;

    [SerializeField] private GameObject roomManagement;
    private RoomManager roomManager;

    private GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
        playerController = player.GetComponent<PlayerController>();

        nextDifficultyStepThreshold = PointsUntilDifficultyUpdate / difficultyMultiplier;

        dynamicGenerationManager = dynamicGenerationManagement.GetComponent<DynamicGenerationManager>();
        roomManager = roomManagement.GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        float playerPoints = playerController.Points;
        if(playerPoints > nextDifficultyStepThreshold)
        {
            nextDifficultyStepThreshold += PointsUntilDifficultyUpdate / difficultyMultiplier;
            AdaptDifficulty();
        }
    }

    private void AdaptDifficulty()
    {
        ShiftProbabilityTickets(dynamicGenerationManager.CannonProbabilityTickets, 3, 10, false);
        ShiftProbabilityTickets(dynamicGenerationManager.CoinProbabilityTickets, 1, 10, true);
        ShiftProbabilityTickets(roomManager.DifficultyProbabilityTickets, 5, 100, true);
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
