using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LArm_Rot_HJW : MonoBehaviour
{
    public GameObject object1;  // 로테이션 값을 받아올 게임 오브젝트
    public GameObject object2;  // 로테이션 값을 적용할 게임 오브젝트

    private void Update()
    {
        if (object1 == null || object2 == null)
        {
            Debug.LogError("Object references are not assigned!");
            return;
        }

        // object1의 로테이션 값을 object2에 적용합니다.
        object2.transform.rotation = object1.transform.rotation;
    }
}
