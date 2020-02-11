using UnityEngine;

public class Fist_Dash : Melee_State_Base<FistItem>
{
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private Vector2 damageSize;

    private OverlapCheckData overlapCheckData;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(damagePoint.position, damageSize);
    }
    protected override void Awake()
    {
        base.Awake();
        overlapCheckData = new OverlapCheckData(onEnter: OnDashHit);
    }

    public override void OnEnter()
    {
        // Look Dir
        Vector3 lookRot = transform.localEulerAngles;
        lookRot.y = GM.Player.Dir == 1 ? 0f : 180f;
        transform.localRotation = Quaternion.Euler(lookRot);
    }
    public override void OnUpdate()
    {
        // Get Overlaps
        overlapCheckData.OverlapCheck(Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer));

        // Animation
        weapon.animator.Play("Dash");
    }

    private void OnDashHit(Collider2D overlap)
    {
        PlayerStats.Inst.DealDamage(new AttackData(5), overlap.gameObject);
    }
}
