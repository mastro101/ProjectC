using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    
    public virtual void OnTriggerEnter(BoxCollider other)
     
    {
          if (other.CompareTag ("Player") && !other.isTrigger)
        {
            virtualCamera.SetActive(true);
    }
    
}


public virtual void OnTriggerExit(BoxCollider other)
    {
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                virtualCamera.SetActive(false);
            }
        }
    }


}




