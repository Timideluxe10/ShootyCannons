using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPrediction
{
    private static ShootingPrediction instance;

    private readonly float timeStepInterval = 0.1f; /* Specifies the timely distance between two trajectory points. */
    private readonly int numberOfTrajectoryPoints = 15; /* Specifies the number of total trajectory points to be calculated. */

    private ShootingPrediction() { }

    public static ShootingPrediction Instance { get => instance == null ? (instance = new ShootingPrediction()) : instance; } /* Singleton instance call. */

    public List<Vector3> GetTrajectoryPoints(Vector3 startPosition, Vector3 launchDirection, float startVelocity)
    {
        List<Vector3> predictionPoints = new List<Vector3>();

        for(int t = 1; t <= numberOfTrajectoryPoints; ++t)
        {
            Vector3 positionOfPrediction = startPosition + launchDirection * startVelocity * t * timeStepInterval;
            positionOfPrediction.y += Physics.gravity.y / 2 * Mathf.Pow(t * timeStepInterval, 2);

            predictionPoints.Add(positionOfPrediction);
        }

        return predictionPoints;
    }
}
