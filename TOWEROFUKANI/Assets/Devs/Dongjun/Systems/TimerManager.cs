using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TimerData
{
    // Timer State
    [SerializeField]
    private bool StartWithMax = false;
    public bool IsActive { get; private set; } = true;
    public bool IsTimerAtMax { get; private set; } = false;

    // Time Data
    [HideInInspector]
    public float curTime = 0; // 타이머의 현재 시간.
    public float endTime = 0; // 타이머릐 최대 시간.

    // Actions
    private Action OnTimerTick;
    private Action OnTimerMax;


    /// <summary>
    /// 타이머를 사용하려면 이 함수를 무조건 Start()에서 실행 시켜 주세요!!!
    /// </summary>
    /// <param name="self">게임오브젝트(자기자신).</param>
    /// <param name="OnTimerTick">타이머가 틱 될때 마다(LateUpdate()에서) 실행되는 함수.</param>
    /// <param name="OnTimerMax">타이머가 최대 시간에 도달하면 실행되는 함수.</param>
    public void Init(GameObject self, Action OnTimerTick = null, Action OnTimerMax = null)
    {
        TimerManager.Inst.AddTimer(self, this);

        this.OnTimerTick = OnTimerTick;
        this.OnTimerMax = OnTimerMax;

        if (StartWithMax)
            SetMax();
    }

    /// <summary>
    /// 타이머 이벤트를 설정하는 함수.
    /// </summary>
    /// <param name="OnTimerTick">타이머가 틱 될때 마다(LateUpdate()에서) 실행되는 함수.</param>
    /// <param name="OnTimerMax">타이머가 최대 시간에 도달하면 실행되는 함수.</param>
    public void SetOnTimerMethod(Action OnTimerTick = null, Action OnTimerMax = null)
    {
        this.OnTimerTick = OnTimerTick;
        this.OnTimerMax = OnTimerMax;
    }

    /// <summary>
    /// 타이머를 자동으로 실행할지 결정하는 함수.
    /// </summary>
    /// <param name="self">게임오브젝트(자기자신).</param>
    /// <param name="use">true: 자동으로 실행(LateUpdate()에서), false: 자동으로 실행 안됨.</param>
    public void UseAutoTick(GameObject self, bool use)
    {
        if (use)
        {
            Init(self);
        }
        else
        {
            TimerManager.Inst.RemoveTimer(self, this);
        }
    }

    /// <summary>
    /// 타이머의 활성화 상태를 정하는 함수.
    /// </summary>
    /// <param name="active">true: 활성화 됨., false: 비활성화 됨.</param>
    public void SetActive(bool active)
    {
        IsActive = active;
    }

    /// <summary>
    /// 타이머의 시간을 흐르게 하는 함수. 
    /// 최대 시간에 도달하면 더 이상 작동 안함. (이어서 작동 시키려면 Restart()함수를 실행하세요.)
    /// </summary>
    public void Tick()
    {
        if (!IsActive || IsTimerAtMax)
            return;

        curTime += Time.deltaTime;
        OnTimerTick?.Invoke();

        if (curTime >= endTime)
        {
            IsTimerAtMax = true;
            OnTimerMax?.Invoke();
            curTime = 0;
        }
    }

    /// <summary>
    /// 타이머가 최대 시간에 도달하면 자동으로 멈춤니다. 다시 시작하려면 이 함수를 실행 시켜주세요.
    /// </summary>
    public void Restart()
    {
        IsTimerAtMax = false;
        curTime = 0;
    }

    /// <summary>
    /// 현재 타이머를 0으로 바꿔줍니다.
    /// </summary>
    public void SetZero() => curTime = 0;
    /// <summary>
    /// 현재 타이머를 최대 시간으로 바꿔줍니다. (바로 OnTimerMax에 등록된 함수를 실행하고 싶을 때 쓰면 좋습니다.)
    /// </summary>
    public void SetMax() => curTime = endTime;
}

public class TimerManager : MonoBehaviour
{
    public static TimerManager Inst { get; private set; }

    private Dictionary<GameObject, List<TimerData>> timers = new Dictionary<GameObject, List<TimerData>>();
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
        if (timers.ContainsKey(go))
        {
            timers[go].Add(data);
            return;
        }

        if (timers.ContainsKey(go) && timers[go].Contains(data))
            return;

        if (!timers.ContainsKey(go))
            timers.Add(go, new List<TimerData>());

        timers[go].Add(data);
    }
    public void RemoveTimer(GameObject go, TimerData data)
    {
        if (!timers.ContainsKey(go) || !timers[go].Contains(data))
            return;

        timers[go].Remove(data);
    }
    public void TickTimers()
    {
        if (timers.Count == 0)
            return;

        for (int i = timers.Count - 1; i >= 0; i--)
        {
            curTickingObj = timers.ElementAt(i).Key;

            if (curTickingObj == null)
            {
                timers.Remove(curTickingObj);
                continue;
            }

            for (int j = timers[curTickingObj].Count - 1; j >= 0; j--)
                timers[curTickingObj][j].Tick();
        }
    }
}
