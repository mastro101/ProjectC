using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSequence
{
    public CommandSequenceData data { get; private set; }

    protected IShooter controller;
    protected PlayerControllerInput playerController;

    public InputData currentInput { get; private set; }
    public System.Action<CommandSequence> onStartSequence;
    public System.Action<InputData> onCorrectInput;
    public System.Action<CommandSequence> onCompletedSequence;
    public System.Action<CommandSequence> onResetSequence;
    public System.Action onWrongInput;

    int currentInputIndex = 0;
    bool complated = false;

    public CommandSequence(CommandSequenceData data, PlayerControllerInput playerController, IShooter controller)
    {
        this.data = data;
        this.playerController = playerController;
        this.controller = controller;

        //this.playerController.OnInputPressed += CheckPressedInput;
        this.controller.OnDestroy += UnsubscribeEvent;
    }

    void UnsubscribeEvent()
    {
        //this.playerController.OnInputPressed -= CheckPressedInput;
        this.controller.OnDestroy -= UnsubscribeEvent;
    }

    public void CheckPressedInput(InputData input)
    {
        if (currentInputIndex < data.inputDatas.Length)
        {
            if (input != data.inputDatas[currentInputIndex])
            {
                onWrongInput?.Invoke();
            }
        }
    }

    public virtual void Complete()
    {
        complated = true;
        onCompletedSequence?.Invoke(this);
    }

    public virtual void Execute()
    {
        if (data.skillPrefab)
            BulletPoolManager.instance.TakeBullet(data.skillPrefab).Shoot(controller.shootPosition, controller.aimDirection, controller);
    }

    public void HandleInputSequence()
    {
        if (data.inputDatas[currentInputIndex].CheckInputPressed() && complated == false)
        {
            if (currentInputIndex == 0)
            {
                onStartSequence?.Invoke(this);
            }

            currentInput = data.inputDatas[currentInputIndex];
            currentInputIndex++;
            onCorrectInput?.Invoke(currentInput);

            if (currentInputIndex == data.inputDatas.Length)
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
        return data.inputDatas[index];
    }
}