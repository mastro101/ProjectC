using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSequenceUI : MonoBehaviour
{
    [SerializeField] PlayerControllerInput controller;

    [SerializeField] Transform box;
    [SerializeField] Image iconPrefab;

    [SerializeField] inputDevice inputDevice;

    Queue<Image> inputImage;

    private void Start()
    {
        inputImage = new Queue<Image>();
        controller.OnInputPressed += UpdateInput;
        controller.OnInputReset += ResetInputView;
    }

    void UpdateInput(InputData _input)
    {
        Image image;
        image = Instantiate(iconPrefab, box);
        switch (inputDevice)
        {
            case inputDevice.keyboard:
                image.sprite = _input.keySprite;
                break;
            case inputDevice.playStation:
                image.sprite = _input.PSSprite;
                break;
            case inputDevice.xBox:
                image.sprite = _input.XboxSprite;
                break;
            default:
                break;
        }
        inputImage.Enqueue(image);
    }

    void ResetInputView()
    {
        int inputImageL = inputImage.Count;
        for (int i = 0; i < inputImageL; i++)
        {
            Destroy(inputImage.Dequeue().gameObject);
        }
    }
}
