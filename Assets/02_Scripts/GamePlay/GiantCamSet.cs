using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GiantCamSet : MonoBehaviourPun
{
    public Transform giantCam;
    void Start()
    {
        if (photonView.IsMine)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 30, transform.position.z);
            Quaternion rot = GameObject.Find("GiantPosition").transform.rotation;
            GameObject cam = PhotonNetwork.Instantiate(giantCam.name, pos, rot);
            cam.transform.Translate(new Vector3(0, 0, 10), Space.Self);
            cam.AddComponent<OVRManager>();
            cam.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
