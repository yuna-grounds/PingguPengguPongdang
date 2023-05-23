using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject guideCanvas;
    [SerializeField] private GameObject guide1Pannel;
    [SerializeField] private GameObject guide2Pannel;
    [SerializeField] private GameObject guide3Pannel;
    public static GameObject prevCanvas;

    private int idx = 1;



    private void OnEnable()
    {
        idx = 1;
    }

    private void Start()
    {
        idx = 1;
    }

    public void OnClickNext()
    {
        idx++;
        if (idx > 3)
        {
            OnClickExit();
            return;
        }
        GuideChange();
    }

    public void OnClickPrev()
    {
        idx--;
        if (idx < 1)
        {
            OnClickExit();
            return;
        }
        GuideChange();
    }

    public void GuideChange()
    {
        switch (idx)
        {
            case 1:
                guide1Pannel.SetActive(true);
                guide2Pannel.SetActive(false);
                guide3Pannel.SetActive(false);
                break;

            case 2:
                guide1Pannel.SetActive(false);
                guide2Pannel.SetActive(true);
                guide3Pannel.SetActive(false);
                break;

            case 3:
                guide1Pannel.SetActive(false);
                guide2Pannel.SetActive(false);
                guide3Pannel.SetActive(true);
                break;
        }
    }
    public void OnClickExit()
    {
        guide1Pannel.SetActive(true);
        guide2Pannel.SetActive(false);
        guide3Pannel.SetActive(false);

        guideCanvas.SetActive(false);
        prevCanvas.SetActive(true);
        idx = 1;
        print(idx);
    }
}
