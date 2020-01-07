using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDeviceInput : MonoBehaviour
{
    public static CheckDeviceInput instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}