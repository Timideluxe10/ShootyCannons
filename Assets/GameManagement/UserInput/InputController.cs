using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static readonly KeyCode SHOOT = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Only check for keyboard input if game is running
        if(GameController.Instance.GameState_ == GameController.GameState.RUNNING)
        {
            if (Input.GetKeyDown(SHOOT))
            {
                GameController.Instance.GetPlayerController().TryShoot();
            }
        }
    }
}
