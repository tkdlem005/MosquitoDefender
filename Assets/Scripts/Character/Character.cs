using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Character : MonoBehaviour
{
    [SerializeField] protected Rigidbody _characterRigidBody;
    [SerializeField] protected Collider _characterCollider;

    public Rigidbody CharacterRigidBody { get { return _characterRigidBody; } }

    public Collider CharacterCollider { get { return _characterCollider; } }

    protected virtual void Awake()
    {
        TryGetComponent<Rigidbody>(out _characterRigidBody);
        TryGetComponent<Collider>(out _characterCollider);
    }

    protected virtual void Start() { }

    protected virtual void Update() { }
}
