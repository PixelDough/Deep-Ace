using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class GolfGameController : MonoBehaviour
{

    [SerializeField] private GolfBallController golfBall;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera freelookVirtualCamera;
    
    [Header("Hit Mode Selector")]
    [SerializeField] private Image hitModeSelectionImage;
    [SerializeField] private Texture2D hitModeSelectionSprites;

    private List<CinemachineVirtualCamera> _virtualCameras;
    private CinemachineVirtualCamera _currentVirtualCamera;

    public enum HitModes
    {
        Ground,
        Air,
        COUNT
    }
    private HitModes _hitMode;
    public HitModes HitMode
    {
        get => _hitMode;
        private set
        {
            _hitMode = value;
            Sprite sprite = Sprite.Create(hitModeSelectionSprites, new Rect((int) HitMode * 32, 0, 32, 32), new Vector2(16, 16));
            hitModeSelectionImage.sprite = sprite;
        }
    }

    public enum GameStates
    {
        Intro,
        Aim,
        Power,
        Roll,
        Freelook,
    }

    private GameStates _gameState = GameStates.Aim;

    // private GameState _currentGameState;

    private Player _input;

    private void Start()
    {
        _input = ReInput.players.GetPlayer(0);
        HitMode = HitModes.Air;

        _virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
    }

    private void Update()
    {
        UpdateState();
        // _currentGameState?.Update();
    }

    private void UpdateState()
    {
        switch (_gameState)
        {
            case GameStates.Aim:
                if (_currentVirtualCamera != aimVirtualCamera) ChangeCamera(aimVirtualCamera);
                
                aimVirtualCamera.transform.Rotate(Vector3.up, _input.GetAxis(RewiredConsts.Action.Golf_RotateY) / 10, Space.World);
                Vector3 direction = aimVirtualCamera.transform.forward;
                direction.y = 0;
                direction = direction.normalized;
                if (HitMode == HitModes.Air)
                {
                    direction.y = 1f;
                }
                if (_input.GetButtonDown(RewiredConsts.Action.Golf_Hit))
                {
                    golfBall.Hit(direction.normalized, 8);
                }
                
                //golfBall.DrawPath(direction.normalized * 8);

                if (_input.GetButtonDown(RewiredConsts.Action.Golf_ChangeHitMode))
                {
                    if (HitMode + 1 == HitModes.COUNT)
                        HitMode = 0;
                    else
                        HitMode++;
                }

                if (_input.GetButtonDown(RewiredConsts.Action.Golf_ToggleFreecam))
                {
                    freelookVirtualCamera.transform.position = aimVirtualCamera.transform.position;
                    freelookVirtualCamera.transform.forward = aimVirtualCamera.transform.forward;
                    _gameState = GameStates.Freelook;
                }
                
                break;
            case GameStates.Freelook:
                if (_currentVirtualCamera != freelookVirtualCamera) ChangeCamera(freelookVirtualCamera);
                if (_input.GetButtonDown(RewiredConsts.Action.Golf_ToggleFreecam))
                {
                    _gameState = GameStates.Aim;
                }
                break;
        }
    }

    private void ChangeCamera(CinemachineVirtualCamera virtualCamera)
    {
        foreach (var cam in _virtualCameras)
        {
            cam.enabled = (cam == virtualCamera);
        }

        _currentVirtualCamera = virtualCamera;
    }

}
