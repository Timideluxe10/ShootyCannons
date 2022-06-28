using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPrediction
{
    private static ShootingPrediction instance;

    private readonly float timeStepInterval = 0.2f;
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

            // TODO: check for Collision (cannon at prediction position) and react accordingly.
        }

        return predictionPoints;
    }
}
