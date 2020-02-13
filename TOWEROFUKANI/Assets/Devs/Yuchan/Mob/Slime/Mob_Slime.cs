using UnityEngine;

public class Mob_Slime : GroundMob_Base
{
    [SerializeField] float m_addSpeed = 2f;
    [SerializeField] float m_AtkActJumpHeight = 3f;
    bool m_bAtkEndActing = false;

    public override float VelX => m_bAtkEndActing ? WalkSpeed * m_addSpeed : base.VelX;
    public override bool CanFollow => m_bAtkEndActing ? false : base.CanFollow;
    public override bool CanAttack => m_bAtkEndActing ? false: base.CanAttack;

    public override void OnAttackEnd()
    {   
        base.OnAttackEnd();
        m_jumpData.height = m_jumpHeight + m_AtkActJumpHeight;

        m_bJumpStart = true;
        m_bAtkEndActing = true;
        m_CurAniST = eMobAniST.AirborneUp;          
        Dir = GM.PlayerPos.x > transform.position.x ? -1 : 1;
    }

    public override void OnGroundEnter()
    {
        base.OnGroundEnter();
        m_bAtkEndActing = false;
        m_jumpData.height = m_jumpHeight;
    }


    //=================================================================
    //      ## Mob_Slime :: Dead
    //=================================================================

    public override void OnDead()
    {
        base.OnDead();
        GetComponent<CorpseSpawner>().Spawn();
    }

    public override void OnSuccessfulAttack()
    {
        //this.CreateStatusStun(mobAction: MobAction.None, statusType: StatusType.Debuff, endTime: 1f);
    }
}