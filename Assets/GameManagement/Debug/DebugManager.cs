using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("Shooting Trajectory")]
    [SerializeField] private bool drawShootingTrajectory;
    private List<GameObject> trajectorySpheres; /* Stores the spheres of the current trajectory (if it's calculated) to be able to remove them in the next call of FixedUpdate. */

    [Header("Automatic Shooting")]
    [SerializeField] private bool doAutoshoot;
    [SerializeField] private float minTurnTime = .3f; /* The minimum time a cannon has to turn before it can shoot automatically. */
    private float autoshootTimer;

    [Header("Time Scale")]
    [SerializeField] private bool doScaleTime = false;
    [SerializeField] private float timeScale = 1f;

    private GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
        playerController = GameController.Instance.GetPlayerController();

        if (drawShootingTrajectory)
            trajectorySpheres = new List<GameObject>();

        if (doAutoshoot)
            autoshootTimer = minTurnTime;

        if (doScaleTime)
            GameController.Instance.SetTimeScale(timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject currentCannon = playerController.CurrentCannon;

        if (currentCannon != null)
        {
            CannonController currentCannonController = currentCannon.GetComponent<CannonController>();
            List<Vector3> trajectoryPoints = CreateTrajectoryPoints(currentCannon, currentCannonController);

            if (drawShootingTrajectory)
            {
                DeleteOldTrajectorySpheres();
                DrawTrajectorySpheres(trajectoryPoints);
            }

            if (doAutoshoot)
            {
                autoshootTimer -= Time.deltaTime;
                if (autoshootTimer <= 0f && CheckForShootingTarget(trajectoryPoints, currentCannon, currentCannonController))
                {
                    currentCannonController.Shoot();
                    autoshootTimer = minTurnTime;
                }
            }

        }

    }

    // Use current cannon and its controller to get necessary parameters for the trajectory points delivered by ShootingPrediction script.
    private List<Vector3> CreateTrajectoryPoints(GameObject currentCannon, CannonController currentCannonController)
    {
        Vector3 startPosition = currentCannon.transform.position + currentCannon.transform.up * 2;
        Vector3 launchDirection = currentCannon.transform.up;
        float startVelocity = ((currentCannonController.Power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;
        List<Vector3> trajectoryPoints = ShootingPrediction.Instance.GetTrajectoryPoints(startPosition, launchDirection, startVelocity);
        return trajectoryPoints;
    }

    // Return true if a target is found on current trajectory (cannon which is not yet visited), false otherwise.
    private bool CheckForShootingTarget(List<Vector3> trajectoryPoints, GameObject currentCannon, CannonController currentCannonController)
    {
        RaycastHit raycastHit;
        for (int i = 0; i < trajectoryPoints.Count - 1; ++i)
        {
            Vector3 start = trajectoryPoints[i];
            Vector3 direction = trajectoryPoints[i + 1] - start;
            Debug.DrawRay(start, direction, Color.blue, direction.magnitude); /* can be removed (but looks epic if Gizmos are on) */
            if (Physics.Raycast(start, direction, out raycastHit, direction.magnitude))
            {
                GameObject hitObject = raycastHit.collider.gameObject;
                if (hitObject.CompareTag("Cannon") && !hitObject.GetComponent<CannonController>().IsVisited)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DeleteOldTrajectorySpheres()
    {
        foreach(GameObject sphere in trajectorySpheres)
        {
            GameObject.Destroy(sphere);
        }
        trajectorySpheres = new List<GameObject>();
    }

    private void DrawTrajectorySpheres(List<Vector3> points)
    {
        foreach(Vector3 point in points)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(.2f, .2f, .2f);
            sphere.transform.position = point;
            sphere.transform.SetParent(transform);
            trajectorySpheres.Add(sphere);
        }
    }
}
