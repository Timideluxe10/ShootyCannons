using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.Instance.GemCollected();
            GameController.Instance.PlaySound(audioClip, transform.position);
            Destroy(gameObject);
        }
    }
}
