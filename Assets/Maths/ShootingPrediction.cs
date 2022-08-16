using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPrediction
{
    private static ShootingPrediction instance;

    private readonly float timeStepInterval = 0.1f;
    private readonly int numberOfPredictionPoints = 15;

    private ShootingPrediction() { }

    public static ShootingPrediction Instance { get => instance == null ? (instance = new ShootingPrediction()) : instance; }

    public List<Vector3> GetPredictionPoints(Vector3 startPosition, Vector3 launchDirection, float startVelocity)
    {
        List<Vector3> predictionPoints = new List<Vector3>();

        for(int t = 1; t <= numberOfPredictionPoints; ++t)
        {
            Vector3 positionOfPrediction = startPosition + launchDirection * startVelocity * t * timeStepInterval;
            positionOfPrediction.y += Physics.gravity.y / 2 * Mathf.Pow(t * timeStepInterval, 2);

            predictionPoints.Add(positionOfPrediction);
        }


        // Check for Collision of the trajectory with another Cannon with help of a raycast (not optimal). If another Cannon is found, current cannon shoots.
        // TODO: clean that mess up!

        RaycastHit raycastHit;
        for(int i = 0; i < predictionPoints.Count - 1; ++i)
        {
            Vector3 start = predictionPoints[i];
            Vector3 direction = predictionPoints[i + 1] - start;
            if(Physics.Raycast(start, direction, out raycastHit, 1f))
            {
                Debug.DrawLine(start, raycastHit.point);

                GameObject hitObject = raycastHit.collider.gameObject;
                if(hitObject.CompareTag("Cannon"))
                {
                    Debug.Log("Found cannon!");
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CurrentCannon.GetComponent<CannonController>().Shoot();
                    return null;
                }
            }
                
        }

        return predictionPoints;
    }
}
