using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Pistol_Data : OBB_Data
{
    public bool IsWeaponSelected = true;

    public Animator Animator
    { get; private set; }

    public int magazineSize = 3;
    public int loadedBullets;

    public readonly TimerStat Timer_Shoot = new TimerStat();
    public readonly TimerStat Timer_Reload = new TimerStat();
    public readonly TimerStat Timer_SwapMagazine = new TimerStat();

    public override void Init_Awake(GameObject gameObject)
    {
        Animator = gameObject.GetComponent<Animator>();

        loadedBullets = magazineSize;
    }
    public override void Init_Start(GameObject gameObject)
    {
        Timer_Shoot.SetTick(gameObject);
        Timer_Shoot.EndTime = new FloatStat(1f);
        Timer_Shoot.ToEnd();

        Timer_Reload.SetTick(gameObject);
        Timer_Reload.EndTime = new FloatStat(0.5f);
        Timer_Reload.SetActive(false);

        Timer_SwapMagazine.SetTick(gameObject);
        Timer_SwapMagazine.EndTime = new FloatStat(0.7f);
        Timer_SwapMagazine.SetActive(false);
    }
}

public class OBB_Pistol : OBB_Controller<OBB_Pistol_Data, OBB_State<OBB_Pistol_Data>>
{
    private OBB_Pistol_Idle state_Idle;
    private OBB_Pistol_Main state_Main;
    private OBB_Pistol_Reload state_Reload;
    private OBB_Pistol_SwapMagazine state_SwapMagazine;

    private Single bvr_Idle;
    private Continue bvr_Reload;
    private Choice bvr_Normal;

    protected override void InitStates()
    {
        GetState(ref state_Idle);
        GetState(ref state_Main);
        GetState(ref state_Reload);
        GetState(ref state_SwapMagazine);
        SetDefaultState(state_Idle, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Idle = new Single(
            state_Idle,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Reload = new Continue(
            (state_SwapMagazine,
            EMPTY_STATE_ACTION,
            () => data.Timer_SwapMagazine.IsEnded),
            (state_Reload,
            EMPTY_STATE_ACTION,
            () => data.Timer_Reload.IsEnded));

        bvr_Normal = new Choice(
            (state_Idle,
            EMPTY_STATE_ACTION,
            () => 
            {
                if (Input.GetKey(KeyCode.Mouse0) && data.Timer_Shoot.IsEnded)
                    return state_Main;

                return state_Idle;
            }),
            (state_Main,
            EMPTY_STATE_ACTION,
            () =>
            {
                if (data.Timer_Shoot.IsEnded)
                    return finish;

                return state_Main;
            }));
    }
    protected override void InitObjectives()
    {
        // Pause Weapon
        CreateObjective(
            () => !data.IsWeaponSelected, true).
            AddBehaviour(bvr_Idle);

        // Reload
        CreateObjective(
            () => data.loadedBullets == 0 && data.Timer_Shoot.IsEnded).
            AddBehaviour(bvr_Reload);

        // Normal
        CreateObjective(
            () => data.loadedBullets > 0).
            AddBehaviour(bvr_Normal, true);

        SetDefaultObjective().
            AddBehaviour(bvr_Idle);
    }

    // Test
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            data.IsWeaponSelected = !data.IsWeaponSelected;
            Debug.Log($"Weapon Selected: {data.IsWeaponSelected}");
        }
    }
}
