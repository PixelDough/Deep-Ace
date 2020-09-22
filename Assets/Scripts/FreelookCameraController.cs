using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Rewired;
using Rewired.ComponentControls.Effects;
using UnityEngine;

public class FreelookCameraController : MonoBehaviour
{
    private Player _input;
    private Vector3 _rotation;
    private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        _input = ReInput.players.GetPlayer(0);
        _rotation = transform.rotation.eulerAngles;
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        _rotation = transform.rotation.eulerAngles;
        if (!_virtualCamera.isActiveAndEnabled) return;
        _rotation.y += _input.GetAxis(RewiredConsts.Action.Freecam_LookY) / 5;
        _rotation.x -= _input.GetAxis(RewiredConsts.Action.Freecam_LookX) / 5;

        if (_rotation.x > 180)
            _rotation.x = Mathf.Max(_rotation.x, 280);
        else
            _rotation.x = Mathf.Min(_rotation.x, 80);
        //_rotation.x = Mathf.Clamp(_rotation.x, -80, 80);
        
        transform.rotation = Quaternion.Euler(_rotation);

        bool doBoost = _input.GetButton(RewiredConsts.Action.Freecam_Boost);
        Vector3 movement = Vector3.zero;
        movement.x = _input.GetAxis(RewiredConsts.Action.Freecam_MoveX);
        movement.y = _input.GetAxis(RewiredConsts.Action.Freecam_MoveY);
        movement.z = _input.GetAxis(RewiredConsts.Action.Freecam_MoveZ);
        transform.Translate(movement.normalized / 100 * (doBoost ? 2 : 1), Space.Self);
    }
}
