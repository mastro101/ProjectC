using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Set", menuName = "SetCombo")]
public class SetSequencesData : ScriptableObject
{
    ICommandController controller;
    public CommandSequenceBase[] ComboSections;
    public int level;
    public int exp;
    public float cooldown;

    public CommandSequenceBase currentSection { get; private set; }
    public System.Action<SetSequencesData> onStartSequence;
    public System.Action<SetSequencesData> onCompletedSequence;
    public System.Action<SetSequencesData> onResetSequence;

    int currentSectionIndex;
    bool completed;

    public virtual void Init(ICommandController controller)
    {
        this.controller = controller;
        currentSectionIndex = 0;
        completed = false;
        ComboSections[0].onStartSequence += StartSection;
        foreach (CommandSequenceBase sequences in ComboSections)
        {
            sequences.onCompletedSequence += NextSection;
        }
        this.controller.OnDestroy += UnsubscribeEvent;
    }

    void UnsubscribeEvent()
    {
        ComboSections[0].onStartSequence -= StartSection;
        foreach (CommandSequenceBase sequences in ComboSections)
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
        currentSectionIndex++;
        if (currentSectionIndex == level)
        {
            completed = true;
            onCompletedSequence?.Invoke(this);
            return;
        }

        currentSection = ComboSections[currentSectionIndex];
    }

    public void ResetSequence()
    {
        foreach (CommandSequenceBase sequence in ComboSections)
        {
            sequence.ResetSequence();
        }
        completed = false;
        currentSectionIndex = 0;
        onResetSequence?.Invoke(this);
    }

    public void HandleSetSequences()
    {
        if (currentSectionIndex < ComboSections.Length && currentSectionIndex < level && !completed)
        {
            ComboSections[currentSectionIndex].HandleInputSequence();
        }
    }

    public void Execute()
    {
        currentSection.Execute();
        ResetSequence();
    }
}