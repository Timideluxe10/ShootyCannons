using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class is responsible for dynamic cannon generation.  */
public class DynamicGenerationManager : MonoBehaviour
{
    [SerializeField] private GameObject startCannon;

    [Header("Variables for dynamic generation")]
    [SerializeField] private int cannonBufferSize = 10;
    [SerializeField] private int minAngle; /* Minimum angle which a new cannon can be generated for (>0, <360). */
    [SerializeField] private int maxAngle; /* Maximum angle which a new cannon can be generated for (>0, <360, >minAngle) */

    [Header("Cannon types and probabilities")]
    [SerializeField] private GameObject[] cannonTemplates;
    [SerializeField] private int[] cannonProbabilityTickets;

    [Header("Coin types and probabilities")]
    [SerializeField] private bool doGenerateCoins = true;
    [SerializeField] private float coinGenerationProbability; /* Value between 0 (0% chance for coin between two cannons) and 1 (100% chance). */
    [SerializeField] private GameObject[] coinTemplates;
    [SerializeField] private int[] coinProbabilityTickets;

    [Header("Item types and probabilities")]
    [SerializeField] private bool doGenerateItems = true;
    [SerializeField] private float itemGenerationProbability; /* Value between 0 (0% chance for item between two cannons) and 1 (100% chance) */
    [SerializeField] private GameObject[] itemTemplates;
    [SerializeField] private int[] itemProbabilityTickets;

    [Header("Room management and probabilities")]
    [SerializeField] private bool doGenerateRooms = true;
    [SerializeField] private float roomGenerationProbability;/* Value between 0 (0% chance for room to generate instead of cannon) and 1 (100% chance) */
    [SerializeField] private GameObject roomManagement;
    private RoomManager roomManager;

    private GameObject[] cannonAndRoomBuffer;
    private GameObject[] coinBuffer;
    private GameObject[] itemBuffer;

    private GameObject player;
    private PlayerController playerController;

    private Vector3 startingGravity;

    /* Getter for probability tickets for difficulty manager. */
    public int[] CannonProbabilityTickets { get => cannonProbabilityTickets; }
    public int[] CoinProbabilityTickets { get => coinProbabilityTickets; }

    // Start is called before the first frame update
    void Start()
    {
        cannonAndRoomBuffer = new GameObject[cannonBufferSize];
        coinBuffer = new GameObject[cannonBufferSize];
        itemBuffer = new GameObject[cannonBufferSize];

        player = GameController.Instance.Player;
        playerController = player.GetComponent<PlayerController>();

        startingGravity = Physics.gravity;

        roomManager = roomManagement.GetComponent<RoomManager>();
        roomManager.InitRoomsPerDifficulty();

        GenerateInitialGameObjects();

    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.CurrentCannon != null)
        {
            int numberOfCannonsToGenerate = calculateNumberOfGenerations(); /* This is important because it may be possible to skip one or more cannons. */
            if(numberOfCannonsToGenerate > 0)
            {
                for(int i = 0; i < numberOfCannonsToGenerate; ++i)
                {
                    GameObject lastCannon = cannonAndRoomBuffer[cannonAndRoomBuffer.Length - 1].GetComponent<Generateable>().GetCannonToGenerateFrom();
                    CannonController cannonController = lastCannon.GetComponent<CannonController>();
                    List<Vector3> trajectoryPoints = CreateTrajectoryPointsAtRandomAngle(lastCannon, cannonController);
                    ShiftBuffers();
                    cannonAndRoomBuffer[cannonAndRoomBuffer.Length - 1] = GenerateNewCannonOrRoom(trajectoryPoints, out Vector3 potentialCoinPosition, out Vector3 potentialItemPosition);
                    coinBuffer[coinBuffer.Length - 1] = TryGenerateCoin(potentialCoinPosition);
                    itemBuffer[itemBuffer.Length - 1] = TryGenerateItem(potentialItemPosition);
                }
            }
        }
    }

    // Shift cannon and coin buffer and destroy gameObjects that leave the buffer.
    private void ShiftBuffers()
    {
        GameObject.Destroy(cannonAndRoomBuffer[0]);
        if (coinBuffer[0] != null)
            GameObject.Destroy(coinBuffer[0]);
        if (itemBuffer[0] != null && itemBuffer[0].GetComponent<ItemController>() != null && !itemBuffer[0].GetComponent<ItemController>().ProtectFromDestroy)
            GameObject.Destroy(itemBuffer[0]);

        for(int i = 0; i < cannonAndRoomBuffer.Length - 1; ++i)
        {
            cannonAndRoomBuffer[i] = cannonAndRoomBuffer[i + 1];
            coinBuffer[i] = coinBuffer[i + 1];
            itemBuffer[i] = itemBuffer[i + 1];
        }
    }

    // Note: This will return 0 or 1 almost exclusively, except for the case if the player skipped at least one cannon in the buffer.
    private int calculateNumberOfGenerations()
    {
        int indexFromWhichToGenerate = cannonBufferSize / 2 - 1;
        int indexOfCurrentCannon = GetIndexOfCurrentCannonInBuffer();
        int numberOfGenerations = indexOfCurrentCannon - indexFromWhichToGenerate;

        return numberOfGenerations < 0 ? 0 : numberOfGenerations;
    }

    private int GetIndexOfCurrentCannonInBuffer()
    {
        GameObject currentCannon = playerController.CurrentCannon;
        for(int i = 0; i < cannonAndRoomBuffer.Length; ++i)
        {
            if (cannonAndRoomBuffer[i].GetComponent<Generateable>().GetCannonToGenerateFrom() == currentCannon)
                return i;
        }
        return -1; /* Should never happen. */
    }

    // Completely fill cannon buffer with randomly generated cannons (first cannon in buffer is start cannon). Generate coins and save them in coinBuffer.
    private void GenerateInitialGameObjects()
    {
        cannonAndRoomBuffer[0] = startCannon;
        for(int i = 1; i < cannonAndRoomBuffer.Length; ++i)
        {
            GameObject cannon = cannonAndRoomBuffer[i - 1].GetComponent<Generateable>().GetCannonToGenerateFrom();
            CannonController cannonController = cannon.GetComponent<CannonController>();
            List<Vector3> trajectoryPoints = CreateTrajectoryPointsAtRandomAngle(cannon, cannonController);
            cannonAndRoomBuffer[i] = GenerateNewCannonOrRoom(trajectoryPoints, out Vector3 potentialCoinPosition, out Vector3 potentialItemPosition);
            coinBuffer[i] = TryGenerateCoin(potentialCoinPosition);
            itemBuffer[i] = TryGenerateItem(potentialItemPosition);
        }
    }

    // Use cannon and its controller to get necessary parameters for the trajectory points delivered by ShootingPrediction script. Return trajectory points.
    private List<Vector3> CreateTrajectoryPointsAtRandomAngle(GameObject cannon, CannonController cannonController)
    {
        float angle = Random.Range(minAngle, maxAngle);

        Vector3 launchDirection = Quaternion.Euler(0, 0, -angle) * cannon.transform.up;
        Vector3 startPosition = cannon.transform.position + launchDirection * 2;
        float startVelocity = ((cannonController.Power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;

        List<Vector3> trajectoryPoints = ShootingPrediction.Instance.GetTrajectoryPointsWithFixedGravity(startPosition, launchDirection, startVelocity, startingGravity);
        return trajectoryPoints;
    }

    // Return a random, newly instantiated cannon. Fill out parameter "potentialCoinPosition" to save position where a coin could generate.
    private GameObject GenerateNewCannonOrRoom(List<Vector3> trajectoryPoints, out Vector3 potentialCoinPosition, out Vector3 potentialItemPosition)
    {
        int numberOfTrajectoryPoints = trajectoryPoints.Count;
        int randomIndex = Random.Range(numberOfTrajectoryPoints / 2, numberOfTrajectoryPoints);
        Vector3 randomTrajectoryPoint = trajectoryPoints[randomIndex];
        GameObject template = null;
        if(doGenerateRooms && Random.value <= roomGenerationProbability) /* Generate Room instead of single cannon */
            template = roomManager.ChooseRandomRoom();
        else /* Generate single cannon */
            template = ChooseRandomTemplate(cannonTemplates, cannonProbabilityTickets);
        GameObject generatedObject = GameObject.Instantiate(template, randomTrajectoryPoint, Quaternion.identity);
        generatedObject.transform.SetParent(transform);
        potentialCoinPosition = trajectoryPoints[randomIndex / 2];
        potentialItemPosition = trajectoryPoints[randomIndex / 2 - 2];
        return generatedObject;
    }

    private GameObject TryGenerateCoin(Vector3 potentialCoinPosition)
    {
        if (doGenerateCoins && Random.value <= coinGenerationProbability)
            return GenerateCoinAt(potentialCoinPosition);
        return null;
    }

    private GameObject GenerateCoinAt(Vector3 position)
    {
        GameObject coinTemplate = ChooseRandomTemplate(coinTemplates, coinProbabilityTickets);
        GameObject newCoin = GameObject.Instantiate(coinTemplate, position, coinTemplate.transform.rotation);
        newCoin.transform.SetParent(transform);
        return newCoin;
    }

    private GameObject TryGenerateItem(Vector3 potentialItemPosition)
    {
        if (doGenerateItems && Random.value <= itemGenerationProbability)
            return GenerateItemAt(potentialItemPosition);
        return null;
    }

    private GameObject GenerateItemAt(Vector3 position)
    {
        GameObject itemTemplate = ChooseRandomTemplate(itemTemplates, itemProbabilityTickets);
        GameObject newItem = GameObject.Instantiate(itemTemplate, position, itemTemplate.transform.rotation);
        newItem.transform.SetParent(transform);
        return newItem;
    }

    // Return random template (probabilities in probabilityTickets)
    private GameObject ChooseRandomTemplate(GameObject[] templates, int[] probabilityTickets)
    {
        int sumTickets = sumProbabilityTickets(probabilityTickets);
        int randomTicket = Random.Range(1, sumTickets);

        int ticketCount = 0;
        for(int i = 0; i < templates.Length; ++i)
        {
            ticketCount += probabilityTickets[i];
            if(ticketCount >= randomTicket)
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
        for(int i = 0; i < probabilityTickets.Length; ++i)
        {
            sum += probabilityTickets[i];
        }
        return sum;
    }

}
