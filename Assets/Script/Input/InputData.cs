using UnityEngine;

[CreateAssetMenu()]
public class InputData : ScriptableObject
{
    public KeyCode key, XBoxKey;
    public Sprite keySprite, XboxSprite, PSSprite;

    public bool CheckInputPressed()
    {
        if (Input.GetKeyDown(key) || Input.GetKeyDown(XBoxKey))
        {
            return true;
        }

        return false;
    }
}