using UnityEngine;

public abstract class Bow : Weapon
{
    [Header("Timer")]
    [SerializeField] public TimerData shootTimer;
    [SerializeField] public TimerData drawTimer;

    [Header("Arrow Sprite")]
    [SerializeField] public GameObject arrowSprite;

    [HideInInspector] public bool hasBeenDrawn = false;
    [HideInInspector] public float drawPower = 0;

    protected override void Start()
    {
        base.Start();

        // Init Timer
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}
