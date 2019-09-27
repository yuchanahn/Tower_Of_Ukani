using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TimerData
{
    // Timer State
    [SerializeField]
    private bool MaxOnStart = false;
    public bool IsActive { get; private set; } = true;
    public bool IsTimerAtMax { get; private set; } = false;

    // Time Data
    [HideInInspector]
    public float Timer_Cur = 0; // 타이머의 현재 시간.
    public float Timer_Max = 0; // 타이머릐 최대 시간.

    // Actions
    private Action OnTimerTick;
    private Action OnTimerMax;


    /// <summary>
    /// 타이머를 사용하려면 이 함수를 무조건 Start()에서 실행 시켜 주세요!!!
    /// </summary>
    /// <param name="self">게임오브젝트(자기자신).</param>
    /// <param name="OnTimerTick">타이머가 틱 될때 마다(LateUpdate) 실행되는 함수.</param>
    /// <param name="OnTimerMax">타이머가 최대 시간에 도달하면 실행되는 함수.</param>
    public void Init(GameObject self, Action OnTimerTick = null, Action OnTimerMax = null)
    {
        TimerManager.Inst.AddTimer(self, this);

        this.OnTimerTick = OnTimerTick;
        this.OnTimerMax = OnTimerMax;

        if (MaxOnStart) SetToMax();

    }

    /// <summary>
    /// 타이머 이벤트를 설정하는 함수.
    /// </summary>
    /// <param name="OnTimerTick">타이머가 틱 될때 마다(LateUpdate) 실행되는 함수.</param>
    /// <param name="OnTimerMax">타이머가 최대 시간에 도달하면 실행되는 함수.</param>
    public void SetOnTimerMethod(Action OnTimerTick = null, Action OnTimerMax = null)
    {
        this.OnTimerTick = OnTimerTick;
        this.OnTimerMax = OnTimerMax;
    }

    /// <summary>
    /// 타이머의 현재 상태를 설정하는 함수.
    /// </summary>
    /// <param name="active">true: 작동함, false: 멈춤.</param>
    public void SetActive(bool active)
    {
        IsActive = active;
    }

    /// <summary>
    /// 이 함수는 자동으로 실행됩니다. 외부에서 사용하지 마십시오.
    /// </summary>
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

    /// <summary>
    /// 타이머가 최대 시간에 도달하면 자동으로 멈춤니다. 다시 시작하려면 이 함수를 실행 시켜주세요.
    /// </summary>
    public void Continue() => IsTimerAtMax = false;

    /// <summary>
    /// 현재 타이머를 최대 시간으로 바꿔줍니다. (바로 OnTimerMax에 등록된 함수를 실행하고 싶을 때 쓰면 좋습니다.)
    /// </summary>
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
