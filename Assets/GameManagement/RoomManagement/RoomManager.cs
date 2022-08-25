using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private static readonly int Num_Difficulties = 5;

    [SerializeField] private List<GameObject> roomTemplates;
    [SerializeField] private int[] difficultyProbabilityTickets = new int[Num_Difficulties];

    private List<GameObject>[] roomsPerDifficulty;

    public void InitRoomsPerDifficulty()
    {
        roomsPerDifficulty = new List<GameObject>[Num_Difficulties];
        for (int i = 0; i < roomsPerDifficulty.Length; ++i)
        {
            roomsPerDifficulty[i] = new List<GameObject>();
        }

        foreach(GameObject roomTemplate in roomTemplates) /* Sort room templates based on difficulty (add them to respective list) */
        {
            roomsPerDifficulty[roomTemplate.GetComponent<RoomController>().Difficulty - 1].Add(roomTemplate);
        }
    }

    public GameObject ChooseRandomRoom()
    {
        /* Choose random difficulty (list of roomTemplates of that difficulty) */
        GameObject[] roomsOfRandomDifficulty = ChooseRandomTemplateList(roomsPerDifficulty, difficultyProbabilityTickets).ToArray();

        /* Choose random room from the list of chosen difficulty */
        int[] probabilityTickets = new int[roomsOfRandomDifficulty.Length];
        for(int i = 0; i < probabilityTickets.Length; ++i)
        {
            probabilityTickets[i] = roomsOfRandomDifficulty[i].GetComponent<RoomController>().ProbabilityTickets;
        }
        GameObject chosenRoom = ChooseRandomTemplate(roomsOfRandomDifficulty, probabilityTickets);
        return chosenRoom;
    }


    /* ------------------------------------------------------------------------------------------------------------------------------------------------------------------- */
    /* Only copied helper methods for random pick from templates weighted by probability tickets. */


    // Return random template list (probabilities in probabilityTickets)
    private List<GameObject> ChooseRandomTemplateList(List<GameObject>[] templates, int[] probabilityTickets)
    {
        int sumTickets = sumProbabilityTickets(probabilityTickets);
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        if(templates == null)
            Debug.Log("templates is null");
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

    // Return random template (probabilities in probabilityTickets)
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


}
