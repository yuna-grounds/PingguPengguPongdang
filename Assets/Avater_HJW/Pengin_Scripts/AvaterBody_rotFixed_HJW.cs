using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaterBody_rotFixed_HJW : MonoBehaviour
{
    private float fixedXRotation;  // 고정할 X 로테이션 값
    private float fixedZRotation;  // 고정할 Z 로테이션 값

    private void Start()
    {
        // 시작할 때 현재 로테이션 값을 기록합니다.
        fixedXRotation = transform.rotation.eulerAngles.x;
        fixedZRotation = transform.rotation.eulerAngles.z;
    }

    private void LateUpdate()
    {
        // 로테이션 값을 고정된 값으로 설정합니다.
        Quaternion fixedRotation = Quaternion.Euler(fixedXRotation, transform.rotation.eulerAngles.y, fixedZRotation);
        transform.rotation = fixedRotation;
    }
}
