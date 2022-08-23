using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private bool doDrawTrajectory = false; /* Bool value that decides if trajectory is to be drawn. This should get set by the according Item. */

    private GameObject player;
    private PlayerController playerController;

    private List<GameObject> trajectorySpheres;

    // private List<string> activeEffects;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
        playerController = player.GetComponent<PlayerController>();

        trajectorySpheres = new List<GameObject>();

        // activeEffects = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doDrawTrajectory)
        {
            GameObject currentCannon = playerController.CurrentCannon;
            if (currentCannon != null)
            {
                CannonController currentCannonController = currentCannon.GetComponent<CannonController>();
                List<Vector3> trajectoryPoints = CreateTrajectoryPoints(currentCannon, currentCannonController);

                DeleteOldTrajectorySpheres();
                DrawTrajectorySpheres(trajectoryPoints);
            }
        }
    }

    public void SetDrawTrajectory(bool doDrawTrajectory)
    {
        this.doDrawTrajectory = doDrawTrajectory;
        if (!doDrawTrajectory)
            DeleteOldTrajectorySpheres();
    }

    private List<Vector3> CreateTrajectoryPoints(GameObject currentCannon, CannonController currentCannonController)
    {
        Vector3 startPosition = currentCannon.transform.position + currentCannon.transform.up * 2;
        Vector3 launchDirection = currentCannon.transform.up;
        float startVelocity = ((currentCannonController.Power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;
        List<Vector3> trajectoryPoints = ShootingPrediction.Instance.GetTrajectoryPoints(startPosition, launchDirection, startVelocity);
        return trajectoryPoints;
    }
    private void DrawTrajectorySpheres(List<Vector3> points)
    {
        foreach (Vector3 point in points)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(.2f, .2f, .2f);
            sphere.transform.position = point;
            sphere.transform.SetParent(transform);
            trajectorySpheres.Add(sphere);
        }
    }

    private void DeleteOldTrajectorySpheres()
    {
        foreach (GameObject sphere in trajectorySpheres)
        {
            GameObject.Destroy(sphere);
        }
        trajectorySpheres = new List<GameObject>();
    }

}
