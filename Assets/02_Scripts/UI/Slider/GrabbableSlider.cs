using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabbableSlider : MonoBehaviour
{

    public Slider slider;
    // Update is called once per frame
    void Update()
    {
        slider.value = GetComponent<RectTransform>().anchoredPosition.x;
    }
}
