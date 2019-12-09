using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set", menuName = "SetCombo")]
public class SetSequencesData : ScriptableObject
{
    ICommandController controller;
    public CommandSequenceBase[] comboSections;
    public int level;
    public int exp;
    public float cooldown;

    public CommandSequenceBase currentSection { get; private set; }
    public System.Action<SetSequencesData> onStartSequence;
    public System.Action<SetSequencesData> onCompletedSection;
    public System.Action<SetSequencesData> onCompletedSet;
    public System.Action<SetSequencesData> onResetSequence;

    int currentSectionIndex;
    bool completed;

    public virtual void Init(ICommandController controller)
    {
        this.controller = controller;
        currentSectionIndex = 0;
        completed = false;
        comboSections[0].onStartSequence += StartSection;
        foreach (CommandSequenceBase sequences in comboSections)
        {
            sequences.onCompletedSequence += NextSection;
        }
        this.controller.OnDestroy += UnsubscribeEvent;
    }

    void UnsubscribeEvent()
    {
        comboSections[0].onStartSequence -= StartSection;
        foreach (CommandSequenceBase sequences in comboSections)
        {
            sequences.onCompletedSequence -= NextSection;
        }
        this.controller.OnDestroy -= UnsubscribeEvent;
    }

    void StartSection(CommandSequenceBase sequence)
    {
        onStartSequence?.Invoke(this);
    }

    void NextSection(CommandSequenceBase sequence)
    {
        Debug.Log("section fatta");
        currentSection = comboSections[currentSectionIndex];
        onCompletedSection?.Invoke(this);
        currentSectionIndex++;
        if (currentSectionIndex == level)
        {
            completed = true;
            onCompletedSet?.Invoke(this);
        }
    }

    public void ResetSequence()
    {
        foreach (CommandSequenceBase sequence in comboSections)
        {
            sequence.ResetSequence();
        }
        completed = false;
        currentSectionIndex = 0;
        onResetSequence?.Invoke(this);
    }

    public void HandleSetSequences()
    {
        if (currentSectionIndex < comboSections.Length && currentSectionIndex < level && !completed)
        {
            comboSections[currentSectionIndex].HandleInputSequence();
        }
    }

    public void Execute()
    {
        Debug.Log("Vai combo");
        currentSection.Execute();
        ResetSequence();
    }
}