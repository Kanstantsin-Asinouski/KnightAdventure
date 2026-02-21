using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _knockBackMovingTimeMax;

    private Rigidbody2D _rb;    

    private float _knockBackMovingTimeTimer;

    public bool IsGettingKnockBack { get; private set; }

    private void Start()
    {
        _knockBackForce = 3f;
        _knockBackMovingTimeMax = 0.3f;
    }

    private void Awake()
    {        
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsGettingKnockBack) 
            return;

        _knockBackMovingTimeTimer -= Time.deltaTime;

        if (_knockBackMovingTimeTimer < 0)
        {
            StopKnockBackMovement();
        }
    }

    public void GetKnockedBack(Transform damageSource)
    {
        IsGettingKnockBack = true;
        _knockBackMovingTimeTimer = _knockBackMovingTimeMax;
        Vector2 difference = (transform.position - damageSource.position).normalized * _knockBackForce;
        _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement()
    {
        _rb.linearVelocity = Vector2.zero;
        IsGettingKnockBack = false;
    }
}