using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private Button Bgm;
    [SerializeField] private Button Sfx;

    public Sprite muteSprite;
    public Sprite unMuteSprite;
    public static GameObject prevCanvas;

    private bool bgmState = false;
    private bool sfxState = false;
    public void OnClickExit()
    {
        optionsCanvas.SetActive(false);
        prevCanvas.SetActive(true);
    }

    private void muteFunc(ref bool state, Button target)
    {
        state = !state;
        if (state)
        {
            target.image.sprite = muteSprite;
        }
        else
        {
            target.image.sprite = unMuteSprite;
        }
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
