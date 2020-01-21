using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPCollideDetection : MonoBehaviour
{
    BulletBase bullet;

    private void Awake()
    {
        bullet = GetComponentInParent<BulletBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bullet.OnEnter(other);
    }
}