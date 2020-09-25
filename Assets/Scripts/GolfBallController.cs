using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(Rigidbody))]
public class GolfBallController : MonoBehaviour
{
    [SerializeField] private Renderer ballRenderer;
    [SerializeField] private Renderer deathBallRenderer;
    
    [Header("Ball Path")]
    [SerializeField] private Mesh ballPathMesh;
    [SerializeField] private Material ballPathMaterial;
    
    // TODO: GET RID OF THIS BEING HERE PUT THIS SOMEWHERE ELSE!!!
    [SerializeField] private ParticleSystem ___winPartile;
    
    private Rigidbody _rigidbody;
    private Vector2 _inputDirection;

    private Player _input;

    public bool isDead = false;
    private bool _isOnFlatGround = true;

    public bool isAsleep
    {
        get { return _rigidbody.IsSleeping(); }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = ReInput.players.GetPlayer(0);

        _rigidbody.sleepThreshold = 2;
    }

    private void Update()
    {
        _rigidbody.sleepThreshold = _isOnFlatGround ? 3 : 0;
    }

    public void DrawPath(Vector3 velocity)
    {
        float t;
        t = (-1f * velocity.y) / Physics.gravity.y;
        t = 2f * t;
        
        for (int i = 1; i < 20; i++)
        {
            float time = t * i / 20;
            Vector3 trajectoryPoint = transform.position + velocity * time + Physics.gravity * (0.5f * time * time);
            Graphics.DrawMesh(ballPathMesh, trajectoryPoint, Quaternion.identity, ballPathMaterial, 0);
        }
    }

    public void Hit(Vector3 direction, float force)
    {
        if (_input.GetButtonDown(RewiredConsts.Action.Golf_Hit))
        {
            // _rigidbody.AddForce(direction * 5, ForceMode.Impulse);
            // _rigidbody.AddForce(Vector3.up * 7, ForceMode.Impulse);
            _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        
        if (other.gameObject.CompareTag("ZoneDeath"))
        {
            Die();
        }

        if (other.gameObject.CompareTag("EndPortal"))
        {
            Die(true);
            ___winPartile.Play();
        }
    }

    private void Die(bool instant = false)
    {
        if (isDead) return;
        isDead = true;
        _rigidbody.isKinematic = true;

        ballRenderer.enabled = false;
        if (instant) return;

        deathBallRenderer.gameObject.SetActive(true);
        
        //deathBallRenderer.transform.LeanAlpha(0, 1f);
        //LeanTween.alpha(deathBallRenderer.gameObject, 0, 1f);
        
        LeanTween.value(deathBallRenderer.gameObject, 1f, 0, .5f).setOnUpdate((float val)=>
        {
            Color col = deathBallRenderer.material.GetColor("_MainColor");
            col.a = val;
            deathBallRenderer.material.SetColor("_MainColor", col);
        }).setDelay(0.5f);
    }

    private void OnCollisionStay(Collision other)
    {
        foreach (var contact in other.contacts)
        {
            _isOnFlatGround = Vector3.Angle(contact.normal, Vector3.up) < 15;
        }
    }
    
    public struct GolfPoint
    {
        public Vector3 position;
        public GolfPoint(Vector3 position)
        {
            this.position = position;
        }
    }

    private void FixedUpdate()
    {
        
        //_rigidbody.AddForce(new Vector3(_inputDirection.x, 0, _inputDirection.y), ForceMode.Force);
    }
}
