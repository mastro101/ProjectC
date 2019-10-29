using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : CharacterBase
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(1);
    }
}
