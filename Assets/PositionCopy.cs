using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCopy : MonoBehaviour
{
    public GameObject Target;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = Target.transform.position;
    }
}
