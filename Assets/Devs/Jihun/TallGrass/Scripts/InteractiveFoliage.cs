using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

//[RequireComponent(typeof(MeshFilter))]
public class InteractiveFoliage : MonoBehaviour
{
    static GameObject tempQuad;

    const float BEND_FACTOR = 0.2f;
    const float BEND_FORCE_ON_EXIT = 0.5f;

    float _enterOffset = 0;
    float _exitOffset = 0;

    bool _isBending = false;
    bool _isRebounding = false;

    #region Var:Wind
    bool isWindEnabled = true;

    [SerializeField]
    float baseWindForce = 0.1f;
    [SerializeField]
    float windPeriod = 1;
    [SerializeField]
    float _windOffset = 1;
    [SerializeField]
    float windForceMultiplier = 1;
    #endregion

    float _colliderHalfWidth;

    MeshFilter _meshFilter;
    Spring _spring = new Spring();
    // Start is called before the first frame update
    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _colliderHalfWidth = transform.localScale.x / 2f;
        _windOffset = Random.Range(0f, 3f);
        windPeriod = Random.Range(1f, 3f);
        windForceMultiplier = 0.3f;

        //쿼드 넣어줌
        if (tempQuad == null)
        {
            tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        }
        _meshFilter.mesh = tempQuad.GetComponent<MeshFilter>().mesh;

    }

    void Start()
    {
        if (tempQuad != null) Destroy(tempQuad);
    }

    #region OnTriggers

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.GetComponent<Rigidbody2D>()) return;
        // addition to the OnTriggerEnter2D method to handle jumping into the foliage
        if (col.GetComponent<Rigidbody2D>().velocity.y < -3f)
        {
   //         Debug.Log("JUMP");
            // apply a force in the proper direction based on where we impacted
            if (col.transform.position.x < transform.position.x)
                _spring.applyAdditiveForce(BEND_FORCE_ON_EXIT);
            else
                _spring.applyAdditiveForce(-BEND_FORCE_ON_EXIT);
            _isRebounding = true;
        }

        _enterOffset = col.transform.position.x - transform.position.x;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        var offset = col.transform.position.x - transform.position.x;

        if (_isBending || Mathf.Sign(_enterOffset) != Mathf.Sign(offset))
        {
            _isRebounding = false;
            _isBending = true;

            // figure out how far we have moved into the trigger and then map the offset to -1 to 1.
            // 0 would be neutral, -1 to the left and +1 to the right.
            var radius = _colliderHalfWidth + col.bounds.size.x * 0.5f;
            _exitOffset = MathHelpers.map(offset, -radius, radius, -1f, 1f);
            setVertHorizontalOffset(_exitOffset);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (_isBending)
        {
            // apply a force in the opposite direction that we are currently bending
            _spring.applyForceStartingAtPosition(BEND_FORCE_ON_EXIT * Mathf.Sign(_exitOffset), _exitOffset);
        }

        _isBending = false;
        _isRebounding = true;
    }

    #endregion

    void setVertHorizontalOffset(float offset)
    {
        var verts = _meshFilter.mesh.vertices;


        verts[2].x = -0.5f + offset * BEND_FACTOR / transform.localScale.x;
        verts[3].x = 0.5f + offset * BEND_FACTOR / transform.localScale.x;

        _meshFilter.mesh.vertices = verts;
    }


    void Update()
    {// addition to the Update method to add a wind force
        if (isWindEnabled && !_isBending)
        {
            var windForce = baseWindForce + Mathf.Pow(Mathf.Sin(Time.time * windPeriod + _windOffset) * 0.7f + 0.05f, 4) * 0.05f * windForceMultiplier;
            _spring.applyAdditiveForce(windForce);
            // we only simulate if we are not rebounding. While rebounding the simulation will occur in the next block
            if (!_isRebounding)
                setVertHorizontalOffset(_spring.simulate());
        }


        if (_isRebounding)
        {
            setVertHorizontalOffset(_spring.simulate());

            // apply the spring until its acceleration dies down
            if (Mathf.Abs(_spring.acceleration) < 0.00005f)
            {
                // reset to 0 which is neutral
                setVertHorizontalOffset(0f);
                _isRebounding = false;
            }
        }
    }
}

