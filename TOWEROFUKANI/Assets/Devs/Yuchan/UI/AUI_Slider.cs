using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AUI_Slider : MonoBehaviour
{
    [Disable] public float MAX_;
    [Disable] public float CUR_;

    [SerializeField] Slider mSlider;
    [SerializeField] UnityEvent ZeroEvent;

    void Update()
    {
        mSlider.value = CUR_ / MAX_;
        if (CUR_ <= 0) ZeroEvent.Invoke();
    }
}
