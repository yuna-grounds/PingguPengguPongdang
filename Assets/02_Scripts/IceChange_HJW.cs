using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChange_HJW : MonoBehaviour
{

    public Material material1;
    public Material material2;
    public GameObject[] gameObjects;

    private int currentIndex = 0;
    private float waitTime = 1f;

    private bool isChangingMaterials = false;

    void Start()
    {
        // Material 변경 코루틴
        StartCoroutine(ChangeMaterialsCoroutine());
    }

    IEnumerator ChangeMaterialsCoroutine()
    {
        while (currentIndex < gameObjects.Length) 
        {
            if (!isChangingMaterials)
            {
                isChangingMaterials = true;
                StartCoroutine(ChangeMaterials());
            }

            yield return null;
        }
    }

    IEnumerator ChangeMaterials()
    {
        List<Renderer> childRenderers = new List<Renderer>(); // 자식 객체들의 Renderer 컴포넌트를 저장할 리스트
        List<Transform> childTransforms = new List<Transform>(); // 자식 객체들의 Transform 컴포넌트를 저장할 리스트



        // 자식 객체들의 Renderer 컴포넌트와 Transform 컴포넌트 가져오기
        foreach (Transform childTransform in gameObjects[currentIndex].transform)
        {
            Renderer renderer = childTransform.GetComponent<Renderer>();
            childRenderers.Add(renderer);
            childTransforms.Add(childTransform);
        }

        // Material1로 변경
        foreach (Renderer renderer in childRenderers)
        {
            renderer.material = material1;
        }

        yield return new WaitForSeconds(waitTime); // 머테리얼1 변경 후 머테리얼2 변경까지 1초 기다림

        // Material2로 변경
        foreach (Renderer renderer in childRenderers)
        {
            renderer.material = material2;
        }

        // 회전 변경
        float numRotations = 3f; // 로테이션 반복값
        float duration = 0.3f; // 애니메이션 동작하는 시간
        Quaternion[] startRotations = new Quaternion[childTransforms.Count];        //자식객체의 rotation시작점
        Quaternion[] targetRotations = new Quaternion[childTransforms.Count];       //자식객체의 rotation시 target점

        for (int i = 0; i < childTransforms.Count; i++)
        {
            startRotations[i] = childTransforms[i].rotation;        //자식객체 각각의 rotation값
            targetRotations[i] = Quaternion.Euler(-70f, childTransforms[i].rotation.eulerAngles.y, childTransforms[i].rotation.eulerAngles.z);
            //자식객체 각 타겟 로테이션 값은 x -70f y,z 축은 그대로
        }

        for (int i = 0; i < numRotations; i++) //로테이션 횟수
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                for (int j = 0; j < childTransforms.Count; j++)
                {
                    childTransforms[j].rotation = Quaternion.Lerp(startRotations[j], targetRotations[j], t);
                    //자식객체 각각의 스타트점과 타겟점 보간값
                }

                yield return null;
            }

            // 회전이 완료되면 시작 회전과 목표 회전을 교환하여 다음 회전을 준비
            for (int j = 0; j < childTransforms.Count; j++)
            {
                Quaternion tempRotation = startRotations[j];
                startRotations[j] = targetRotations[j];
                targetRotations[j] = tempRotation;
            }
        }

        // 자식 객체들의 포지션 값 변경
        Vector3[] startPositions = new Vector3[childTransforms.Count];
        Vector3[] targetPositions = new Vector3[childTransforms.Count];

        for (int i = 0; i < childTransforms.Count; i++)
        {
            startPositions[i] = childTransforms[i].position;
            targetPositions[i] = new Vector3(childTransforms[i].position.x, -20f, childTransforms[i].position.z);
        }

        float positionDuration = 1f;
        float positionElapsedTime = 0f;

        while (positionElapsedTime < positionDuration)
        {
            positionElapsedTime += Time.deltaTime;
            float t = positionElapsedTime / positionDuration;

            for (int i = 0; i < childTransforms.Count; i++)
            {
                childTransforms[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], t);
            }

            yield return null;
        }

        currentIndex++;
        isChangingMaterials = false;
    }
}

