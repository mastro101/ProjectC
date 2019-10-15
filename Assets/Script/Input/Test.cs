using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] Image      image;
    [SerializeField] InputData _southInput;
    [SerializeField] InputData _estInput;
    [SerializeField] InputData _westInput;
    [SerializeField] InputData _northInput;

    void Start()
    {
        CustomInput.southInput = _southInput;
        CustomInput.estInput   = _estInput;
        CustomInput.westInput  = _westInput;
        CustomInput.northInput = _northInput;

        image.sprite = CustomInput.southInput.XboxSprite;
    }
}