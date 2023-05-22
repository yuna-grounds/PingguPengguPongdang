using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IceAction_KSW : MonoBehaviourPun
{
    int life;
    MeshCollider mesh;
    MeshRenderer render;
    bool isBreakable;
    bool isDeadZone;
    Material[] mats;

    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        mesh = GetComponent<MeshCollider>();
        render = GetComponent<MeshRenderer>();
        isBreakable = false;
        isDeadZone = false;
        mats = GameObject.Find("MaterialChango").GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        LifeCheck();
        ChangeMaterial();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Picker") && isBreakable)
        {
            other.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(CoolSet(other));
            life--;
            print("맞아부러써");
            if (life == 0)
            {

                GameObject.Find("GameManager").SendMessage("Breaked");
            }
        }
    }
    void LifeCheck()
    {
        if (life == 0)
        {
            mesh.enabled = false;
            render.enabled = false;

        }
    }

    void ChangeMaterial()
    {
        if (!isDeadZone)
        {
            switch(life)
            {
                case 3:
                    render.material = mats[0];
                    break;
                case 2:
                    render.material = mats[1];
                    break;
                case 1:
                    render.material = mats[2];
                    break;
                default:
                    break;
            }
        }
    }

    public void ReBuild()
    {
        life = 3;
        mesh.enabled = true;
        render.enabled = true;
    }

    public void ResetLife()
    {
        life = 3;
    }

    public void LifeZero()
    {
        life = 0;
    }
    public void NotBreak()
    {
        isBreakable = false;
    }
    public void CanBreak()
    {
        isBreakable = true;
    }

    public int GetLife()
    {
        return life;
    }

    public void ThisIsDeadZone()
    {
        isDeadZone = true;
    }

    public void ThisIsNotDeadZone()
    {
        isDeadZone = false;
    }

    IEnumerator CoolSet(Collider other)
    {
        yield return new WaitForSeconds(0.73f);
        other.GetComponent<BoxCollider>().enabled = true;
    }
}