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

        // Deal Damage
        overlapCheckData = new OverlapCheckData(
            onEnter: overlap => 
            {
                PlayerStats.Inst.DealDamage(weapon.attackData_Dash, overlap.gameObject);
            });
    }

    public override void OnEnter()
    {
        // Look Dir
        Flip_Logic.FlipXTo(GM.Player.Dir, transform);

        // Animation
        weapon.animator.Play("Dash");
    }
    public override void OnUpdate()
    {
        // Get Overlaps
        overlapCheckData.OverlapCheck(Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer));
    }
}
