using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Base : MonoBehaviour
{
    [SerializeField] TimerData RandomMoveTimer;
    int RandomDir => Random.Range(0,2)==0 ? -1 : 1;
    int curDir;
    private Rigidbody2D rb2D;
    [SerializeField] private int walkSpeed;
    [SerializeField] JumpData jumpData;
    [SerializeField] GravityData gravityData;
    private bool isGrounded;
    private bool isJumping;

    public void RandomMove()
    {
        rb2D.velocity = new Vector2(walkSpeed * curDir, rb2D.velocity.y);
    }
    void SetRandomDireaction()
    {
        curDir = RandomDir;
        RandomMoveTimer.Continue();
    }
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        
        RandomMoveTimer.Init(gameObject, OnTimerMax : SetRandomDireaction);
        RandomMoveTimer.SetToMax();
    }
    void FixedUpdate()
    {
        Gravity_Logic.ApplyGravity(rb2D, isGrounded ? new GravityData(0, 0) : !isJumping ? gravityData : new GravityData(jumpData.jumpGravity, 0));
    }
}