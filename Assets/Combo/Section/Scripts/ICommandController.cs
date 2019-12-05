using UnityEngine;
using System.Collections.Generic;

public interface ICommandController
{
    GameObject gameObject { get; }
    Transform transform { get; }
    List<CommandSequenceBase> currentSequences { get; set; }
    Vector3 aimDirection { get; }
    System.Action OnDestroy { get; set; }
}
