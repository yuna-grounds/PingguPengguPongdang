using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private Toggle Bgm;
    [SerializeField] private Toggle Sfx;

    public Sprite muteSprite;
    public Sprite unMuteSprite;
    public static GameObject prevCanvas;

    private bool bgmState = true;
    private bool sfxState = true;
    public void OnClickExit()
    {
        optionsCanvas.SetActive(false);
        prevCanvas.SetActive(true);
    }

    private void muteFunc(ref bool state, Toggle target)
    {
        state = !state;
        target.isOn = state;
    }

    public void OnBgmMuteToggleBtn()
    {
        muteFunc(ref bgmState, Bgm);
    }

    public void OnSfxMuteToggleBtn()
    {
        muteFunc(ref sfxState, Sfx);
    }
}
