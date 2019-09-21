using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// READ ME:
// - TimerData의 Init() 함수를 꼭 Awake()에서 실행 시켜 주세요!
// - 타이머가 최대 시간에 도달하면 자동으로 멈춤니다. 다시 실행하려면 Continue() 함수를 실행하세요.

[System.Serializable]
public class TimerData
{
    // Timer State
    [HideInInspector]
    public bool IsActive { get; private set; } = true;
    public bool IsTimerAtMax { get; private set; } = false;

    // Time Data
    [HideInInspector]
    public float Timer_Cur = 0;
    public float Timer_Max = 0;

    // Actions
    private Action OnTimerTick;
    private Action OnTimerMax;

    public void Init(GameObject self)
    {
        TimerManager.Inst.AddTimer(self, this);
    }
    public void Init(GameObject self, Action OnTimerTick = null, Action OnTimerMax = null)
    {
        TimerManager.Inst.AddTimer(self, this);

        this.OnTimerTick = OnTimerTick;
        this.OnTimerMax = OnTimerMax;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
    public void Tick()
    {
        if (!IsActive || IsTimerAtMax)
            return;

        Timer_Cur += Time.deltaTime;
        OnTimerTick?.Invoke();

        if (Timer_Cur >= Timer_Max)
        {
            IsTimerAtMax = true;
            OnTimerMax?.Invoke();
            Timer_Cur = 0;
        }
    }
    public void Continue() => IsTimerAtMax = false;
    public void SetToMax() => Timer_Cur = Timer_Max;
}

public class TimerManager : MonoBehaviour
{
    public static TimerManager Inst { get; private set; }

    private Dictionary<GameObject, List<TimerData>> cooldowns = new Dictionary<GameObject, List<TimerData>>();
    private GameObject curTickingObj;

    private void Awake()
    {
        Inst = this;
    }
    private void LateUpdate()
    {
        TickTimers();
    }

    public void AddTimer(GameObject go, TimerData data)
    {
        if (cooldowns.ContainsKey(go))
        {
            cooldowns[go].Add(data);
            return;
        }

        cooldowns.Add(go, new List<TimerData>());
        cooldowns[go].Add(data);
    }
    public void TickTimers()
    {
        if (cooldowns.Count == 0)
            return;

        for (int i = cooldowns.Count - 1; i >= 0; i--)
        {
            curTickingObj = cooldowns.ElementAt(i).Key;

            if (curTickingObj == null)
            {
                cooldowns.Remove(curTickingObj);
                continue;
            }

            for (int j = cooldowns[curTickingObj].Count - 1; j >= 0; j--)
                cooldowns[curTickingObj][j].Tick();
        }
    }
}
