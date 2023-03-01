using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.AI.State_Machine;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Baller _ballHeldBy;
    [SerializeField] private bool _isBeingHeld = false;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _dribbleTransform;
    [SerializeField] private Vector3 _floorPosition;
    [SerializeField] private bool _isHittingFloor = false;
    [SerializeField] private float _dribbleTime = 5f;
    private SphereCollider _sphereCollider;
    [SerializeField] private float _dribbleAmplitude = 0.8f;
    void Start()
    {
        _floorPosition = GameObject.FindGameObjectWithTag("Floor").transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(Dribble());
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isBeingHeld) return;
        if (_ballHeldBy == null) return;
        

    }

    public void PickUp(IStateMachineMember baller)
    {
        Drop();
        _ballHeldBy = baller as Baller;
        if (_ballHeldBy == null) return;
        _dribbleTransform = _ballHeldBy.transform.GetChild(0);
        transform.parent = _dribbleTransform;
        _isBeingHeld = true;
        _rigidBody.isKinematic = true;
        
    }

    private void SetDribbleConstrains(bool constrained)
    {
        if (constrained)
        {
            _rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            _rigidBody.constraints = RigidbodyConstraints.None;
        }
    }

    public void Drop()
    {
        transform.parent = null;
        _ballHeldBy = null;
        _isBeingHeld = false;
        _rigidBody.isKinematic = false;
        _dribbleTransform = null;
        return;
    }

    private IEnumerator Dribble()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!_isBeingHeld)
            {
                continue;
            }
            Vector3 landingSpot = new Vector3(_dribbleTransform.position.x, _floorPosition.y + _sphereCollider.radius, _dribbleTransform.position.z);
            _rigidBody.position = landingSpot + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * _dribbleTime)* _dribbleAmplitude);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "floor") return;
        if (_isBeingHeld && _ballHeldBy != null)
        {
            _isHittingFloor = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.tag != "floor") return;
        if (_isBeingHeld && _ballHeldBy != null )
        {
            _isHittingFloor = false;
        }
    }
}
