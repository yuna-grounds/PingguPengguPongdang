using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    float dir_rot;
    float dir_fb;
    //public Slider MyEnergy;
    float MyLife = 100;
    // 이동 회전 저장변수
    public Vector3 setPos;      // 이동
    public Quaternion setRot;   // 회전
    public GameObject Bullet;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        // 처음은 초기화된 위치로 설정(0,0,0)
        this.transform.position = setPos;
        this.transform.rotation = setRot;
    }

    // Update is called once per frame
    void Update()
    {
        // 내 캐릭터는 네트워크에서 받아오는 위치정보가 아니라,
        // 직접 변경가능하므로 photoView.IsMine으로 조건 검사로 움직임을 표현낸다.
        if (photonView.IsMine)
        {
            MoveRot();    // 이동 및 회전
            Jump();       // 점프

            // 나중에 할 대포 발사 코드
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                print("공격");
                    GameObject temp = PhotonNetwork.Instantiate(Bullet.name, firePoint.position, firePoint.rotation);
                    //temp.GetComponent<Rigidbody>().AddForce(firePoint.forward * 600f);
            }
        }
        else
        {
            // 그게 아니라면, 즉 내 컴퓨터에서 최초로 생성된 캐릭터가 아니라면
            // 현재 객체의 위치를 정해진 위치(setPos)까지 보간하여 이동시켜라
            this.transform.position = Vector3.Lerp(this.transform.position, setPos, Time.deltaTime * 20f);
            // 현재 객체의 위치를 정해진 방향(setRot)까지 보간하여 회전시켜라
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, setRot, Time.deltaTime * 20f);
        }


    }

    void MoveRot()
    {
        dir_rot = Input.GetAxis("Horizontal");
        dir_fb = Input.GetAxis("Vertical");

        this.transform.Translate(Vector3.forward * dir_fb * Time.deltaTime * 5f);
        this.transform.Rotate(Vector3.up * dir_rot * Time.deltaTime * 110f);

    }

    // PhotonSerializeView 처리(콜백 메서드)
    /* PhotonStream: 이 매개변수는 데이터를 읽거나 쓰는 데 사용되는 
     * Photon 스트림 객체입니다. 
     * 데이터를 쓰기 위해서는 PhotonStream의 Write 메서드를 호출하여 
     * 데이터를 스트림에 쓸 수 있고, 
     * 데이터를 읽기 위해서는 PhotonStream의 Read 메서드를 사용하여 
     * 데이터를 스트림에서 읽을 수 있습니다.
     */

    /* PhotonMessageInfo: 이 매개변수는 Photon 메시지에 대한 정보를 포함하는 객체입니다. 
     * 메시지를 보낸 플레이어의 ID, 타임스탬프 등의 정보를 확인할 수 있습니다.
     */

    // 현재 위치의 정보를 remote에게 전달
    // 리모트라인 // mine의 정보를 받을 것이다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // mine인 상태에서 Remote에게 데이터를 넘겨줄 때 - 현재값을 저장
        if (stream.IsWriting)
        {
            setPos = transform.position;
            setRot = transform.rotation;

            stream.SendNext(setPos);  // 값에 대한 객체(이동/회전값)
            stream.SendNext(setRot);  // 값에 대한 객체(이동/회전값)
        }
        // remote 상태에서 mine의 정보를 받을 때 - 저장된 값을 읽어온다.
        else
        {
            // Warning -- SendNext로 전달된 데이터 순서대로 받을 수 있다.
            setPos = (Vector3)stream.ReceiveNext();   // 읽어온 값을 받을 객체
            setRot = (Quaternion)stream.ReceiveNext();   // 읽어온 값을 받을 객체
        }
    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 15f, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Dammaged();
        }
    }

    private void Dammaged()
    {
        if (MyLife <= 0f) return;
        this.MyLife -= 3f;
        //MyEnergy.value = MyLife;
        if (MyLife <= 0)
        {
            MyLife = 0;
        }
    }
}