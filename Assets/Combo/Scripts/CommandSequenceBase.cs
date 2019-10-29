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
    public System.Action<CommandSequenceBase> onExecutedSequence;
    public System.Action<CommandSequenceBase> onInterruptedSequence;

    public CommandSequenceBase(ICommandController controller, params InputData[] inputDatas)
    {
        this.controller = controller;
        this.inputDatas = inputDatas;
    }

    public virtual void Init(ICommandController controller)
    {
        this.controller = controller;
        currentInputIndex = 0;
    }

    public virtual void Execute()
    {
        onExecutedSequence?.Invoke(this);
        Debug.Log("fuckingYeah");
    }

    int currentInputIndex = 0;
    public void HandleInputSequence()
    {
        if (inputDatas[currentInputIndex].CheckInputPressed())
        {
            if (currentInputIndex == 0)
            {
                controller.currentSequences.Add(this);
                currentInput = inputDatas[currentInputIndex];
                currentInputIndex++;
                onStartSequence?.Invoke(this);
                onCorrectInput?.Invoke(currentInput);
                if (currentInputIndex == inputDatas.Length)
                {
                    Execute();
                    ResetSequence();
                }
                return;
            }

            currentInput = inputDatas[currentInputIndex];
            currentInputIndex++;
            onCorrectInput?.Invoke(currentInput);

            if (currentInputIndex == inputDatas.Length)
            {
                Execute();
                ResetSequence();
            }
        }
    }

    public void ResetSequence()
    {
        currentInputIndex = 0;
        currentInput = null;
        onInterruptedSequence?.Invoke(this);
    }

    public InputData GetInput(int index)
    {
        return inputDatas[index];
    }
}