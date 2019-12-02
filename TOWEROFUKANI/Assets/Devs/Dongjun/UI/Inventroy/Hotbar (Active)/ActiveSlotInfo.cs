using UnityEngine;
using UnityEngine.UI;

public class ActiveSlotInfo : UI_Bar
{
    [SerializeField] private GameObject cooldownIcon;
    [SerializeField] private Text cooldownTime;
    [SerializeField] private GameObject activeFrame;

    public void ShowActiveFrame(bool show)
    {
        activeFrame.SetActive(show);
    }
    public void ShowCooldown(TimerData cooldownTimer)
    {
        if (!cooldownTimer.IsEnded)
        {
            activeFrame.SetActive(false);

            cooldownIcon.SetActive(true);
            Value = 1 - (cooldownTimer.CurTime / cooldownTimer.EndTime);
            cooldownTime.text = (cooldownTimer.EndTime - cooldownTimer.CurTime).ToString("0.0");
        }
        else
        {
            cooldownIcon.SetActive(false);
            return;
        }
    }
}
