using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GameRule : MonoBehaviourPun
{
    private int score;
    public int life;

    public GameObject Player;

    [HideInInspector]
    public GameObject myPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector2 initPos = Random.insideUnitCircle * 5f;
        
        PhotonNetwork.Instantiate(Player.name, new Vector3(initPos.x, 2f, initPos.y), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
