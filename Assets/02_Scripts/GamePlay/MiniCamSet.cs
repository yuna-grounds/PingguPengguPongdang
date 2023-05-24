using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniCamSet : MonoBehaviourPun
{
    public Transform miniCam;
    void Start()
    {
        SetMiniCam();
    }

    [PunRPC]
    public void SetMiniCam()
    {
        if (photonView.IsMine)
        {
            GameObject cam = PhotonNetwork.Instantiate(miniCam.name, transform.position, Quaternion.identity);
            cam.AddComponent<OVRManager>();

            //cam.AddComponent<C>();

            cam.transform.SetParent(transform);
            cam.transform.parent.gameObject.AddComponent<OVRPlayerController>();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}