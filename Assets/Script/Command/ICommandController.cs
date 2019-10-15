using UnityEngine;
using System.Collections.Generic;

public interface ICommandController
{
    Transform transform { get; }
    List<CommandSequenceBase> currentSequences { get; set; }
}
