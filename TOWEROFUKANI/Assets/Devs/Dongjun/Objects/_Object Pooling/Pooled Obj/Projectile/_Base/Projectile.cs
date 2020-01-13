using UnityEngine;

public class Projectile : PoolingObj
{
    #region Var: Inspector
    [Header("Object Detection")]
    [SerializeField] protected Vector2 detectSize;
    [SerializeField] protected LayerMask detectLayers;
    [SerializeField] protected string[] ignoreTags;

    [Header("Visual")]
    [SerializeField] private bool rotateToMovingDir = true;

    [Header("Effects")]
    [SerializeField] protected PoolingObj particle_Hit;
    [SerializeField] protected float particle_HitOffset;
    #endregion

    #region Var: Data
    protected Vector2 velocity;
    protected ProjectileData projectileData;
    protected AttackData attackData;
    #endregion

    #region Method: Init PoolingObj
    public override void ResetOnSpawn()
    {
        projectileData.travelDist.ModFlat = 0;
    }
    #endregion

    #region Method: Init Data
    public void InitData(Vector2 startDir, ProjectileData projectileData, AttackData attackData = new AttackData())
    {
        // Init Data
        this.projectileData = projectileData;
        this.attackData = attackData;

        // Init Velocity
        velocity = projectileData.moveSpeed.Value * startDir;
    }
    #endregion

    #region Method: Unity
    protected virtual void FixedUpdate()
    {
        Move();
        DetectObject();

        if (projectileData.travelDist.Value >= projectileData.travelDist.Max)
            OnMaxDist();
    }
    #endregion

    #region Method: Move
    protected virtual void Move()
    {
        // Set Velocity
        velocity.y -= projectileData.gravity.Value * Time.fixedDeltaTime;

        // Translate
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
        
        // Update Travle Dist
        projectileData.travelDist.ModFlat += velocity.magnitude * Time.fixedDeltaTime;

        // Rotate Towards Moving Dir
        if (rotateToMovingDir)
            transform.right = velocity.normalized;
    }
    protected virtual void OnMaxDist()
    {
        ObjPoolingManager.Sleep(this);
    }
    #endregion

    #region Method: Hit
    protected bool IsIgnoreTag(Component check)
    {
        for (int j = 0; j < ignoreTags.Length; j++)
            if (check.CompareTag(ignoreTags[j]))
                return true;

        return false;
    }
    protected virtual void DetectObject()
    {
        Vector2 pos = (transform.position + (transform.right * detectSize.x * 0.5f)) - (transform.right * (projectileData.moveSpeed.Value * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits =
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, projectileData.moveSpeed.Value * Time.fixedDeltaTime, detectLayers);

        Vector2 hitPos = transform.position;
        GameObject hitObj = null;

        if (hits.Length == 0)
            return;

        bool hasHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            // Check Ignore Tag
            if (IsIgnoreTag(hits[i].collider))
                continue;

            // Set Data
            hasHit = true;
            hitPos = hits[i].point;
            hitObj = hits[i].collider.gameObject;

            if (CheckHit(hits[i]))
                break;
        }

        if (hasHit)
            OnHit(hitPos, hitObj);
    }
    protected virtual bool CheckHit(RaycastHit2D hit)
    {
        return false;
    }
    protected virtual void OnHit(Vector2 hitPos, GameObject hitObject)
    {
        // Sleep
        ObjPoolingManager.Sleep(this);

        // Spawn Hit Effect
        if (particle_Hit == null) return;
        Transform hitParticle = particle_Hit.Spawn(hitPos, Quaternion.identity).transform;
        hitParticle.right = -transform.right;
        hitParticle.position -= transform.right * particle_HitOffset;
    }
    #endregion
}
