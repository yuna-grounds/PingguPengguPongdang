using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(this.transform.forward * 700f);
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
