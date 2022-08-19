using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class is responsible for dynamic cannon generation.  */
public class DynamicGenerationManager : MonoBehaviour
{
    [SerializeField] private GameObject startCannon;

    [SerializeField] private GameObject[] cannonTemplates;
    [SerializeField] private int[] cannonProbabilityTickets;

    [SerializeField] private int cannonBufferSize = 10;

    [SerializeField] private int minAngle; /* Minimum angle which a new cannon can be generated for (>0, <360). */
    [SerializeField] private int maxAngle; /* Maximum angle which a new cannon can be generated for (>0, <360, >minAngle) */

    private GameObject[] cannonBuffer;

    private GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        cannonBuffer = new GameObject[cannonBufferSize];
        player = GameController.Instance.Player;
        playerController = player.GetComponent<PlayerController>();
        GenerateInitialCannons();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.CurrentCannon != null)
        {
            int numberOfCannonsToGenerate = calculateNumberOfCannonsToGenerate(); /* This is important because it may be possible to skip one or more cannons. */
            if(numberOfCannonsToGenerate > 0)
            {
                for(int i = 0; i < numberOfCannonsToGenerate; ++i)
                {
                    GameObject lastCannon = cannonBuffer[cannonBuffer.Length - 1];
                    CannonController cannonController = lastCannon.GetComponent<CannonController>();
                    List<Vector3> trajectoryPoints = CreateTrajectoryPointsAtRandomAngle(lastCannon, cannonController);
                    ShiftCannonBuffer();
                    cannonBuffer[cannonBuffer.Length - 1] = GenerateNewCannon(trajectoryPoints);
                }
            }
        }
    }

    private void ShiftCannonBuffer()
    {
        GameObject.Destroy(cannonBuffer[0]);
        for(int i = 0; i < cannonBuffer.Length - 1; ++i)
        {
            cannonBuffer[i] = cannonBuffer[i + 1];
        }
    }

    // Note: This will return 0 or 1 almost exclusively, except for the case if the player skipped at least one cannon in the buffer.
    private int calculateNumberOfCannonsToGenerate()
    {
        int indexFromWhichToGenerate = cannonBufferSize / 2 - 1;
        int indexOfCurrentCannon = GetIndexOfCurrentCannonInBuffer();
        int numberOfCannonsToGenerate = indexOfCurrentCannon - indexFromWhichToGenerate;

        return numberOfCannonsToGenerate < 0 ? 0 : numberOfCannonsToGenerate;
    }

    private int GetIndexOfCurrentCannonInBuffer()
    {
        GameObject currentCannon = playerController.CurrentCannon;
        for(int i = 0; i < cannonBuffer.Length; ++i)
        {
            if (cannonBuffer[i] == currentCannon)
                return i;
        }
        return -1; /* Should never happen. */
    }

    // Completely fill cannon buffer with randomly generated cannons (first cannon in buffer is start cannon)
    private void GenerateInitialCannons()
    {
        cannonBuffer[0] = startCannon;
        for(int i = 1; i < cannonBuffer.Length; ++i)
        {
            GameObject cannon = cannonBuffer[i - 1];
            CannonController cannonController = cannon.GetComponent<CannonController>();
            List<Vector3> trajectoryPoints = CreateTrajectoryPointsAtRandomAngle(cannon, cannonController);
            cannonBuffer[i] = GenerateNewCannon(trajectoryPoints);
        }
    }

    // Use cannon and its controller to get necessary parameters for the trajectory points delivered by ShootingPrediction script. Return trajectory points.
    private List<Vector3> CreateTrajectoryPointsAtRandomAngle(GameObject cannon, CannonController cannonController)
    {
        float angle = Random.Range(minAngle, maxAngle);

        Vector3 launchDirection = Quaternion.Euler(0, 0, -angle) * cannon.transform.up;
        Vector3 startPosition = cannon.transform.position + launchDirection * 2;
        float startVelocity = ((cannonController.Power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;

        List<Vector3> trajectoryPoints = ShootingPrediction.Instance.GetTrajectoryPoints(startPosition, launchDirection, startVelocity);
        return trajectoryPoints;
    }

    // Return a random, newly instantiated cannon
    private GameObject GenerateNewCannon(List<Vector3> trajectoryPoints)
    {
        int numberOfTrajectoryPoints = trajectoryPoints.Count;
        Vector3 randomTrajectoryPoint = trajectoryPoints[Random.Range(numberOfTrajectoryPoints / 2, numberOfTrajectoryPoints)];
        GameObject cannonTemplate = ChooseRandomCannonTemplate();
        GameObject newCannon = GameObject.Instantiate(cannonTemplate, randomTrajectoryPoint, Quaternion.identity);
        newCannon.transform.SetParent(transform);
        return newCannon;
    }

    // Return random cannon template (probabilities in cannonProbabilityTickets)
    private GameObject ChooseRandomCannonTemplate()
    {
        int sumTickets = sumProbabilityTickets();
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        for(int i = 0; i < cannonTemplates.Length; ++i)
        {
            ticketCount += cannonProbabilityTickets[i];
            if(ticketCount >= randomTicket)
            {
                return cannonTemplates[i];
            }
        }
        return null; /* Should never happen. */
    }

    // Return total number of probability tickets
    private int sumProbabilityTickets()
    {
        int sum = 0;
        for(int i = 0; i < cannonProbabilityTickets.Length; ++i)
        {
            sum += cannonProbabilityTickets[i];
        }
        return sum;
    }

}
