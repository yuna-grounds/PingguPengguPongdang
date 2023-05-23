using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    CharacterController characterController;

    Vector3 movePower;

    public float Speed;
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;   // 캐릭터에게 작용하는 중력.

    Vector3 sendPos;
    Quaternion sendRot;

    Animator playerAni;
    private static Vector3 MoveDir;

    float MyLife = 100;

    public GameObject Bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        playerAni = this.transform.GetChild(0).GetComponent<Animator>();
        MoveDir = Vector3.zero;
        characterController = GetComponent<CharacterController>();
        this.transform.position = sendPos;
        this.transform.rotation = sendRot;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Jump();
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
            this.transform.position = Vector3.Lerp(this.transform.position, sendPos, Time.deltaTime * 20f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, sendRot, Time.deltaTime * 20f);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, sendPos, Time.deltaTime * 20f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, sendRot, Time.deltaTime * 20f);
        }
        playerAni.SetFloat("Speed", characterController.velocity.magnitude);
    }


    private void Jump()
    {
        if (characterController.isGrounded)
        {
            // 플레이어가 바라보는 방향으로 세팅
            MoveDir = Vector3.zero;

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= Speed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jumpSpeed;
            }

        }

        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        characterController.Move(MoveDir * Time.deltaTime);
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0f || v != 0f)
        {
            float y = Camera.main.transform.rotation.eulerAngles.y;
            float targetAngle = y;
            if (h > 0f) // D 키를 눌렀을 때
            {
                targetAngle = y + 90f;
                if (v > 0f)
                {
                    targetAngle -= 45f;
                }
                else if (v < 0f)
                {
                    targetAngle += 45f;
                }

            }
            else if (h < 0f) // A 키를 눌렀을 때
            {
                targetAngle = y - 90f;
                if (v > 0f)
                {
                    targetAngle += 45f;
                }
                else if (v < 0f)
                {
                    targetAngle -= 45f;
                }
            }
            else if (v < 0f) // S 키를 눌렀을 때
            {
                targetAngle = y + 180f;
            }

            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            movePower = transform.forward * Time.deltaTime * Speed;
            characterController.Move(movePower);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // mine인 상태에서 Remote에게 데이터를 넘겨줄 때 - 현재값을 저장
        if (stream.IsWriting)
        {
            sendPos = transform.position;
            sendRot = transform.rotation;

            stream.SendNext(sendPos);  // 값에 대한 객체(이동/회전값)
            stream.SendNext(sendRot);  // 값에 대한 객체(이동/회전값)
        }
        // remote 상태에서 mine의 정보를 받을 때 - 저장된 값을 읽어온다.
        else
        {
            // Warning -- SendNext로 전달된 데이터 순서대로 받을 수 있다.
            sendPos = (Vector3)stream.ReceiveNext();   // 읽어온 값을 받을 객체
            sendRot = (Quaternion)stream.ReceiveNext();   // 읽어온 값을 받을 객체
        }
    }
}
