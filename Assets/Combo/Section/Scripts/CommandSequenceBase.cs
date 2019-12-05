using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSequenceBase : ScriptableObject
{
    protected ICommandController controller;
    [SerializeField] public InputData[] inputDatas;

    public InputData currentInput { get; private set; }
    public System.Action<CommandSequenceBase> onStartSequence;
    public System.Action<InputData> onCorrectInput;
    public System.Action<CommandSequenceBase> onCompletedSequence;
    public System.Action<CommandSequenceBase> onResetSequence;

    int currentInputIndex = 0;
    bool complated;

    public CommandSequenceBase(ICommandController controller, params InputData[] inputDatas)
    {
        this.controller = controller;
        this.inputDatas = inputDatas;
    }

    public virtual void Init(ICommandController controller)
    {
        this.controller = controller;
        currentInputIndex = 0;
        complated = false;
    }

    public virtual void Complete()
    {
        complated = true;
        onCompletedSequence?.Invoke(this);
    }

    public virtual void Execute()
    {
        ResetSequence();
    }

    public void HandleInputSequence()
    {
        if (inputDatas[currentInputIndex].CheckInputPressed() && complated == false)
        {
            if (currentInputIndex == 0)
            {
                onStartSequence?.Invoke(this);
            }

            currentInput = inputDatas[currentInputIndex];
            currentInputIndex++;
            onCorrectInput?.Invoke(currentInput);

            if (currentInputIndex == inputDatas.Length)
            {
                Complete();
            }
        }
    }

    public void ResetSequence()
    {
        currentInputIndex = 0;
        currentInput = null;
        complated = false;
        onResetSequence?.Invoke(this);
    }

    public InputData GetInput(int index)
    {
        return inputDatas[index];
    }
}