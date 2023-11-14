using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SetStartVelocity : MonoBehaviour
{
    [SerializeField] private float startVelocity = 10f;

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * startVelocity;
    }
}
