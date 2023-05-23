using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public enum GameState
{
    idle, ready, roulette, settingPanCake, play, finished
}
public class GameManager : MonoBehaviourPun
{
    public Transform iceGround;
    public Transform giantPos;
    public Transform miniPos;
    public Material deadMat;
    List<Transform> ices;
    Transform deadZone;
    GameState gameState;

    bool playingSettings = false;
    int round = 0;



    void Start()
    {
        gameState = GameState.idle;
        ices = new List<Transform>(iceGround.GetComponentsInChildren<Transform>());
        ices.RemoveAt(0);           // IceGround를 담은 부모 객체에 스크립트가 들어가는 것을 방지
        foreach (Transform t in ices)
        {
            t.transform.AddComponent<IceAction_KSW>();
        }
    }


    void Update()
    {
        GameFlow();
        GameRule();
    }

    void GameFlow()
    {
        switch (gameState)
        {
            case GameState.idle:
                // idle 상태에서 플레이어 접속을 체크
                round++;
                gameState = GameState.ready;
                break;
            case GameState.ready:
                // 새로운 Turn을 시작하기 위한 준비 단계
                gameState = GameState.roulette;
                break;
            case GameState.roulette:
                // 우선 빙판을 깰 수 없도록
                if (playingSettings)
                {
                    DisableBreaking();
                    playingSettings = false;
                    round++;
                }
                // 돌림판이 돌아가는 중의 상태

                // 돌림판이 끝나면, 데드존 세팅 등등..
                SetDeadZone();
                gameState = GameState.settingPanCake;
                break;
            case GameState.settingPanCake:
                // 세팅전을 굽는다
                if (!playingSettings)
                {
                    SettingsToPlay();
                    playingSettings = true;
                    print("Round " + round);
                }
                Invoke("ChangeToPlay", 3f);
                break;
            case GameState.play:
                // 특수효과 등이 적용된 후 실제로 빙판을 깨는 단계
                // 빙판을 깰 수 있도록 세팅, 미니펭귄 이동 가능, 

                break;
        }
    }

    void GameRule()
    {
        switch (gameState)
        {
            case GameState.roulette:
                if (deadZone.GetComponent<IceAction_KSW>().GetLife() == 0)
                {
                    DeadZoneKill();
                    gameState = GameState.finished;
                }
                break;
            case GameState.play:
                if (deadZone.GetComponent<IceAction_KSW>().GetLife() == 0)
                {
                    DeadZoneKill();
                }
                break;
        }
    }

    void ChangeToPlay()
    {
        gameState = GameState.play;
    }

    // 세팅전 굽기
    void SettingsToPlay()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().CanBreak();
            if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                t.transform.GetComponent<IceAction_KSW>().ResetLife();
        }
    }

    // 깨지말아요
    void DisableBreaking()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().NotBreak();
        }
    }

    // 데드존 세팅
    void SetDeadZone()
    {
        if (deadZone != null)
        {
            deadZone.GetComponent<IceAction_KSW>().ThisIsNotDeadZone();
            deadZone = null;
        }
        List<Transform> temp = new List<Transform>();
        foreach (Transform t in ices)
        {
            if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                temp.Add(t);
        }
        deadZone = temp[Random.Range(0, temp.Count)];
        deadZone.GetComponent<IceAction_KSW>().ThisIsDeadZone();
        deadZone.GetComponent<MeshRenderer>().material = deadMat;
    }

    // 데드존 깨면 아이스 제거
    void DeadZoneKill()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().LifeZero();
        }
    }



    // Ice에서 본인이 깨졌을 때 이 함수를 호출
    public void Breaked()
    {
        gameState = GameState.ready;
    }

}