using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ChargeStation : MonoBehaviour
{
    private SphereCollider _collider;

    private void Awake()
    {
        TryGetComponent(out _collider);
    }

    private void Start()
    {
        _collider.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter.Instance.IsChargeStation = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter.Instance.IsChargeStation = false;
        }
    }
}
