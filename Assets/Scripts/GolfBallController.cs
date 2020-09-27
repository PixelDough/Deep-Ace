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

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem spawnParticles;
    
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
        _rigidbody.maxAngularVelocity = 1000;
        // _rigidbody.AddForce(direction * 5, ForceMode.Impulse);
        // _rigidbody.AddForce(Vector3.up * 7, ForceMode.Impulse);
        transform.rotation = Quaternion.LookRotation(direction);
        _rigidbody.AddForce(direction * force, ForceMode.Impulse);
        //_rigidbody.angularVelocity = new Vector3(400, 0, 0);
        _rigidbody.AddRelativeTorque(new Vector3(force, 0, 0), ForceMode.VelocityChange);
    }

    public void Teleport(Vector3 position)
    {
        _rigidbody.isKinematic = true;
        transform.position = position;
        _rigidbody.isKinematic = false;
        
        trailRenderer.Clear();
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

    public void Respawn(GolfPoint golfPoint)
    {
        isDead = false;
        Teleport(golfPoint.position);
        _rigidbody.isKinematic = false;
        ballRenderer.enabled = true;
        
        Color col = deathBallRenderer.material.GetColor("_MainColor");
        col.a = 1;
        deathBallRenderer.material.SetColor("_MainColor", col);
        deathBallRenderer.enabled = false;

        spawnParticles.transform.position = transform.position;
        spawnParticles.Play();

    }

    /*private IEnumerator RespawnAnimation()
    {
        ballRenderer.enabled = false;
        _rigidbody.isKinematic = true;
        deathBallRenderer.enabled = true;
        
        while (deathBallRenderer.material.GetColor("_MainColor").a < 1f)
        {
            LeanTween.value(deathBallRenderer.gameObject, 0f, 1f, .5f).setOnUpdate((float val) =>
            {
                Color col = deathBallRenderer.material.GetColor("_MainColor");
                col.a = val;
                deathBallRenderer.material.SetColor("_MainColor", col);
            });
            yield return null;
        }

        ballRenderer.enabled = true;
        _rigidbody.isKinematic = false;
        deathBallRenderer.enabled = false;
        
        yield return null;
    }*/

    private void Die(bool instant = false)
    {
        if (isDead) return;
        isDead = true;
        _rigidbody.isKinematic = true;

        ballRenderer.enabled = false;
        if (instant) return;

        deathBallRenderer.enabled = true;
        
        //deathBallRenderer.transform.LeanAlpha(0, 1f);
        //LeanTween.alpha(deathBallRenderer.gameObject, 0, 1f);
        
        LeanTween.value(deathBallRenderer.gameObject, 1f, 0, .5f).setOnUpdate((float val)=>
        {
            Color col = deathBallRenderer.material.GetColor("_MainColor");
            col.a = val;
            deathBallRenderer.material.SetColor("_MainColor", col);
        }).setDelay(0.5f);
    }

    /*private void OnCollisionEnter(Collision other)
    {
        if (_rigidbody.angularVelocity.sqrMagnitude > 1)
            _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity,Vector3.zero, 20 * Time.deltaTime);
        if (_rigidbody.velocity.sqrMagnitude > 1)
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, new Vector3(0, _rigidbody.velocity.y, 0), 10 * Time.deltaTime);
    }*/

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
