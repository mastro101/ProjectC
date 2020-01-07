using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    GameObject gameObject { get; }
    Transform transform { get; }
    Vector3 aimDirection { get; }

    System.Action OnDestroy { get; set; }
}
