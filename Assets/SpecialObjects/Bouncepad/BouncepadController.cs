using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncepadController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            GameController.Instance.PlaySound(GetComponent<AudioSource>().clip, transform.position);
        }
    }
}
