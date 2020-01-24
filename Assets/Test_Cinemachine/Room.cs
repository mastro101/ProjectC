using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    
    // Start is called before the first frame update
    public virtual void OnTriggerEnter(BoxCollider other)
     
    {
          if (other.CompareTag ("Player") && !other.isTrigger)
        {
            virtualCamera.SetActive(true);
    }
    }

    // Update is called once per frame
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




