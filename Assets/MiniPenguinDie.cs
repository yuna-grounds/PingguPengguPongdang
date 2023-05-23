using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPenguinDie : MonoBehaviour
{
    bool flag = false;
    private void Update()
    { 
        if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            if (hit.transform.CompareTag("Ice"))
            {
                return;
            }
            this.GetComponent<VRRig>().enabled = false;
        }
        
    }

    
}
