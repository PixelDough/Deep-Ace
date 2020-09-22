﻿using System;
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
    
    private Rigidbody _rigidbody;
    private Vector2 _inputDirection;

    private Player _input;
    private Camera _camera;

    private bool _isDead = false;
    private bool _isOnFlatGround = true;

    private void Start()
    {
        _camera = Camera.main;
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
        if (other.gameObject.CompareTag("ZoneDeath"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;
        _rigidbody.isKinematic = true;
        
        deathBallRenderer.gameObject.SetActive(true);
        ballRenderer.enabled = false;
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

    private void FixedUpdate()
    {
        
        //_rigidbody.AddForce(new Vector3(_inputDirection.x, 0, _inputDirection.y), ForceMode.Force);
    }
}
