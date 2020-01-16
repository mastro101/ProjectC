using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSection", menuName = "Combo/Section")]
public class CommandSequenceData : ScriptableObject
{
    [SerializeField] public InputData[] inputDatas;
    [SerializeField] public BulletBase skillPrefab;
}