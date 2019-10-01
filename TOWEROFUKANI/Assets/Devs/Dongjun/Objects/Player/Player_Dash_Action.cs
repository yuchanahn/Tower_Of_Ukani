using UnityEngine;

public class Player_Dash_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Visual")]
    [SerializeField] private Transform spriteRoot;

    [Header("Dash")]
    [SerializeField] private float dashDist;
    [SerializeField] private float dashTime;
    #endregion

    #region Var: Dash
    private float dashTime_Cur = 0;
    #endregion

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
    #endregion

    #region Var: Properties
    public bool IsDasing { get; private set; } = false;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        Animation();
        IsDasing = true;
        rb2D.velocity = new Vector2(PlayerInputManager.Inst.Input_DashDir * (dashDist / dashTime), 0);
    }
    public override void OnExit()
    {
        PlayerInputManager.Inst.Input_DashDir = 0;
        dashTime_Cur = 0;
        rb2D.velocity = new Vector2(0, 0);
    }
    public override void OnFixedUpdate()
    {
        dashTime_Cur += Time.fixedDeltaTime;

        if (dashTime_Cur >= dashTime)
            IsDasing = false;
    }
    #endregion

    #region Animation
    private enum AnimState
    {
        Dash_Forward = 5,
        Dash_Backward = 6,
    }
    private void Animation()
    {
        if ((PlayerInputManager.Inst.Input_DashDir == 1 && spriteRoot.rotation.eulerAngles.y == 0) ||
            (PlayerInputManager.Inst.Input_DashDir == -1 && spriteRoot.rotation.eulerAngles.y == 180))
            animator.Play("Player_Dash_Forward", 0, 0f);
        else
            animator.Play("Player_Dash_Backward", 0, 0f);
    }
    #endregion
}
