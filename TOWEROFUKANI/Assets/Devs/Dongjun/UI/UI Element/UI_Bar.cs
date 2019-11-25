using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    [SerializeField] protected Image bar;

    public float Value
    {
        get { return bar.fillAmount; }
        set { bar.fillAmount = value; }
    }
}
