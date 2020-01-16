using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSet", menuName = "Combo/Set")]
public class SetSequencesData : ScriptableObject
{
    public CommandSequenceData[] comboSections;
    public int level;
    public int exp;
    public float cooldown;
}