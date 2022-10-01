using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static readonly KeyCode SHOOT = KeyCode.Space;
    public static readonly KeyCode USE_ITEM = KeyCode.F;
    public static readonly KeyCode DISCARD_ITEM = KeyCode.LeftShift;

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
            if (Input.GetKeyDown(USE_ITEM))
            {
                GameController.Instance.UseItem();
            }
            if (Input.GetKeyDown(DISCARD_ITEM))
            {
                GameController.Instance.DiscardItem();
            }
        }
    }
}
