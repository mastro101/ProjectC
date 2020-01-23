using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        virtualCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //virtualCamera.SetActive(false);
    }
}
