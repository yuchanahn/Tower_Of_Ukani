using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Vector2[] mPath;
    public Queue<Vector2> mMovement_queue = new Queue<Vector2>();

    [SerializeField] Transform PlayerPos;
    [SerializeField] JPS_PathFinder pathFinder;

    Vector2 Vel = Vector2.zero;
    Vector2 targetPos = Vector2.zero;


    bool IsFirst => (mMovement_queue.Count == mPath.Length);
    bool IsFar => IsFirst ? false : Vector2.Distance(targetPos, transform.position) > 0.1f;
    Vector2 TargetPos => targetPos = (IsFar ? targetPos : mMovement_queue.Dequeue());

    Vector2 GetVel => (TargetPos - (Vector2)transform.position).normalized;
    bool FollowStart = false;

    void UpdatePath(Vector2[] path)
    {
        mPath = path;
        mMovement_queue.Clear();
        foreach(var i in mPath) mMovement_queue.Enqueue(i);
    }


    private void OnGUI()
    {
        if( GUI.Button(new Rect(500,10,100,20), "Start"))
        {
            Vector2[] path = pathFinder.Find(transform.position, PlayerPos.position);
            UpdatePath(path);
        }
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (mMovement_queue.Count > 0) Vel = GetVel;
        else Vel = !IsFar ? Vector2.zero : Vel;
        transform.position += (Vector3)Vel * Time.deltaTime;
    }
}
