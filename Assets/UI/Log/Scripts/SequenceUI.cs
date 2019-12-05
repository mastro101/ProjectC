using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceUI : MonoBehaviour
{
    public Image slider;
    public Transform inputList;
    public float percent;
    public CommandSequenceBase sequence;

    public void Init(CommandSequenceBase _sequence)
    {
        percent = 1.0f / _sequence.inputDatas.Length;
        slider.fillAmount = 0;
        sequence = _sequence;

        sequence.onCorrectInput += FillSlider;
        sequence.onResetSequence += EmptySlider;
    }

    public void FillSlider(InputData i)
    {
        slider.fillAmount += percent;
    }

    public void EmptySlider(CommandSequenceBase c)
    {
        slider.fillAmount = 0;
    }

    private void OnDestroy()
    {
        sequence.onCorrectInput -= FillSlider;
        sequence.onResetSequence -= EmptySlider;
    }
}