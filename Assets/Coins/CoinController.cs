using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] float rotationSpeed = 1f;

    [SerializeField] AudioClip audioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * 50);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            GameController.Instance.CoinCollected(value);
            GameController.Instance.PlaySound(audioClip, transform.position);
            Destroy(gameObject);
        }
    }
}
