using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGenerationManager : MonoBehaviour
{
    [SerializeField] private GameObject startCannon;
    // [SerializeField] private GameObject cannonTemplate;

    [SerializeField] private GameObject[] cannonTemplates;
    [SerializeField] private int[] cannonProbabilityTickets;

    [SerializeField] private int cannonBufferSize = 10;

    [SerializeField] private int minAngle; /* Minimum angle which a new cannon can be generated for (>0, <360). */
    [SerializeField] private int maxAngle; /* Maximum angle which a new cannon can be generated for (>0, <360, >minAngle) */

    private GameObject[] cannonBuffer;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        cannonBuffer = new GameObject[cannonBufferSize];
        player = GameController.Instance.Player;
        GenerateInitialCannons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateInitialCannons()
    {
        cannonBuffer[0] = startCannon;
        for(int i = 1; i < cannonBuffer.Length; ++i)
        {
            GameObject cannon = cannonBuffer[i - 1];
            CannonController cannonController = cannon.GetComponent<CannonController>();
            List<Vector3> trajectoryPoints = CreateTrajectoryPointsAtRandomAngle(cannon, cannonController);
            DrawTrajectorySpheres(trajectoryPoints);
            cannonBuffer[i] = GenerateNewCannon(trajectoryPoints);
        }
    }

    // Use cannon and its controller to get necessary parameters for the trajectory points delivered by ShootingPrediction script.
    private List<Vector3> CreateTrajectoryPointsAtRandomAngle(GameObject cannon, CannonController cannonController)
    {
        float angle = Random.Range(minAngle, maxAngle);

        Vector3 launchDirection = Quaternion.Euler(0, 0, -angle) * cannon.transform.up;
        Vector3 startPosition = cannon.transform.position + launchDirection * 2;
        float startVelocity = ((cannonController.Power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;

        List<Vector3> trajectoryPoints = ShootingPrediction.Instance.GetTrajectoryPoints(startPosition, launchDirection, startVelocity);
        return trajectoryPoints;
    }

    private GameObject GenerateNewCannon(List<Vector3> trajectoryPoints)
    {
        int numberOfTrajectoryPoints = trajectoryPoints.Count;
        Vector3 randomTrajectoryPoint = trajectoryPoints[Random.Range(numberOfTrajectoryPoints / 2, numberOfTrajectoryPoints)];
        GameObject cannonTemplate = ChooseRandomCannonTemplate();
        GameObject newCannon = GameObject.Instantiate(cannonTemplate, randomTrajectoryPoint, Quaternion.identity);
        newCannon.transform.SetParent(transform);
        return newCannon;
    }

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

    private int sumProbabilityTickets()
    {
        int sum = 0;
        for(int i = 0; i < cannonProbabilityTickets.Length; ++i)
        {
            sum += cannonProbabilityTickets[i];
        }
        return sum;
    }

    private void DrawTrajectorySpheres(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = point;
            sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
            sphere.transform.SetParent(transform);
        }
    }


}
