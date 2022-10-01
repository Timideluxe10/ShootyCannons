using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour, Generateable
{
    [SerializeField] private int difficulty; /* Value between 1 and 5 */
    [SerializeField] private int probabilityTickets = 100; /* Lower for uncommon, higher for more common rooms WITHIN the SAME difficulty */

    [SerializeField] private GameObject lastCannon;

    public GameObject LastCannon { get => lastCannon; }
    public int Difficulty { get => difficulty; }
    public int ProbabilityTickets { get => probabilityTickets; }

    public GameObject GetCannonToGenerateFrom()
    {
        return lastCannon;
    }
}
