using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceLog : MonoBehaviour
{
    [SerializeField] PlayerData player;
    [Space]
    [SerializeField] GameObject key;
    [SerializeField] GameObject xBox;
    [SerializeField] GameObject play;
    [Space]
    [SerializeField] SequenceUI sequenceUI;
    [SerializeField] Image IconPrefab;

    private void Start()
    {
        foreach (var sequence in player.sequences)
        {
            InstantiateSequenceView(key, sequence);
            InstantiateSequenceView(xBox, sequence);
            InstantiateSequenceView(play, sequence);
        }
    }

    void InstantiateSequenceView(GameObject go, CommandSequenceBase sequence)
    {
        SequenceUI _sequenceUI = Instantiate(sequenceUI, go.transform);
        foreach (var input in sequence.inputDatas)
        {
            Image inputImage = Instantiate(IconPrefab, _sequenceUI.inputList);
            if (go == key) inputImage.sprite = input.keySprite;
            else if (go == xBox) inputImage.sprite = input.XboxSprite;
            else if (go == play) inputImage.sprite = input.PSSprite;
        }
        _sequenceUI.Init(sequence);
    }

    public void OpenKeyUI()
    {
        key.SetActive(true);
        xBox.SetActive(false);
        play.SetActive(false);
    }
    public void OpenXBoxUI()
    {
        key.SetActive(false);
        xBox.SetActive(true);
        play.SetActive(false);
    }
    public void OpenPlayUI()
    {
        key.SetActive(false);
        xBox.SetActive(false);
        play.SetActive(true);
    }
}