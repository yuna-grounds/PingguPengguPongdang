using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class CameraController : MonoBehaviourPun
{
    public GameObject FirstCam;
    public GameObject ThirdCam;
    // Start is called before the first frame update
    void Start()
    {
        FirstCam = GameObject.Find("FirstPersonCamera");
        ThirdCam = GameObject.Find("ThirdPersonCamera");
        if (photonView.IsMine)
        {
            FirstCam.GetComponent<FirstPersonCameraController>().Player = this.transform;
            ThirdCam.GetComponent<ThirdPersonCameraController>().Player = this.transform;

            FirstCam.transform.position = this.transform.position;
            ThirdCam.transform.position = this.transform.position;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
