using UnityEngine;
using System.Collections.Generic;

public interface ICommandController
{
    GameObject gameObject { get; }
    Transform transform { get; }
    List<SetSequencesData> currentSequencesSet { get; set; }
    Vector3 aimDirection { get; }
    System.Action OnDestroy { get; set; }
}
