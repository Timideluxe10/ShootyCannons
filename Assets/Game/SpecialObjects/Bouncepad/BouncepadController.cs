using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncepadController : MonoBehaviour
{

    [SerializeField] private AudioClip audioClip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            GameController.Instance.PlaySound(audioClip, transform.position);
        }
    }
}
