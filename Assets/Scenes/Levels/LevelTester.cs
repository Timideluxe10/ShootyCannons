using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTester : MonoBehaviour
{
    private enum PlayerSpawnPosition
    {
        START /* Normal player starting position (start of level). */, 
        LAST_JUMP /* Player spawn pos. gets set to second to last cannon (to test the last jump). */,
        FIXED_JUMP /* Player spawn pos. gets set to specific cannon (of index 'indexFixedJump'). */
    }

    private GameObject player;
    [SerializeField] private PlayerSpawnPosition playerSpawnPosition = PlayerSpawnPosition.START;
    [SerializeField] private int indexFixedJump = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
        SetPlayerSpawnPosition();
    }

    private void SetPlayerSpawnPosition()
    {
        GameObject cannonsHolder = GameObject.Find("Cannons");
        Transform[] children = cannonsHolder.GetComponentsInChildren<Transform>();
        List<GameObject> cannons = new List<GameObject>();
        foreach(Transform transform in children){
            GameObject gameObject = transform.gameObject;
            if (gameObject.CompareTag("Cannon"))
                cannons.Add(gameObject);
        }

        Vector3 position;
        switch (playerSpawnPosition)
        {
            case PlayerSpawnPosition.START: 
                return;  /* Leave setting player spawn to general management script. */
            case PlayerSpawnPosition.LAST_JUMP:
                position = cannons[cannons.Count - 2].transform.position + new Vector3(0, 2, 0);
                break;
            case PlayerSpawnPosition.FIXED_JUMP:
                position = cannons[indexFixedJump].transform.position + new Vector3(0, 2, 0);
                break;
            default: 
                return; /* Should never happen. */
        }

        player.transform.position = position;
    }
}
