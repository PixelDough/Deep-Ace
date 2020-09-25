using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

    [Header("Windows")] 
    [SerializeField] private UIWindowController powerSelectorWindow;

    private List<UIWindowController> _allWindows;

    private List<CinemachineVirtualCamera> _virtualCameras;
    private CinemachineVirtualCamera _currentVirtualCamera;

    private float _aimCameraAngle = 0;
    
    private List<GolfBallController.GolfPoint> _golfPoints = new List<GolfBallController.GolfPoint>();
    
    /*public enum HitModes
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
    }*/

    public enum GameStates
    {
        Intro,
        Aim,
        Power,
        Roll,
        Dead,
        Freelook,
    }

    private GameStates _gameState = GameStates.Aim;

    // private GameState _currentGameState;

    private Player _input;

    private void Awake()
    {
        _input = ReInput.players.GetPlayer(0);
        //HitMode = HitModes.Air;

        _virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
        _allWindows = GetComponentsInChildren<UIWindowController>(true).ToList();
        
        EnterState();
    }

    private void Update()
    {
        UpdateState();
        // _currentGameState?.Update();
    }

    private void UpdateAimCamera()
    {
        aimVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value = _aimCameraAngle;

        if (Input.GetAxis("Mouse X") != 0)
        {
            _aimCameraAngle += _input.GetAxis(RewiredConsts.Action.Golf_RotateY) / 5;
        }
        else
        {
            _aimCameraAngle += _input.GetAxis(RewiredConsts.Action.Golf_RotateY) * Time.deltaTime * 75;
        }
    }

    public void ChangeStates(GameStates state)
    {
        ExitState();
        _gameState = state;
        EnterState();
    }

    private void EnterState()
    {
        switch (_gameState)
        {
            case GameStates.Aim:
                if (_currentVirtualCamera != aimVirtualCamera) ChangeCamera(aimVirtualCamera);
                EnsureWindowsAreOpen(true);
                break;
            case GameStates.Power:
                EnsureWindowsAreOpen(true, powerSelectorWindow);
                break;
            case GameStates.Roll:
                EnsureWindowsAreOpen(true);
                break;
            case GameStates.Freelook:
                EnsureWindowsAreOpen(true);
                Time.timeScale = 0;
                break;
        }
    }
    
    private void UpdateState()
    {
        switch (_gameState)
        {
            case GameStates.Aim:
                UpdateAimCamera();
                
                if (_input.GetButtonDown(RewiredConsts.Action.Golf_Hit))
                {
                    ChangeStates(GameStates.Power);
                }
                
                //golfBall.DrawPath(new Vector2(5, 7).normalized * 8);

                /*if (_input.GetButtonDown(RewiredConsts.Action.Golf_ChangeHitMode))
                {
                    if (HitMode + 1 == HitModes.COUNT)
                        HitMode = 0;
                    else
                        HitMode++;
                }*/

                if (_input.GetButtonDown(RewiredConsts.Action.Golf_ToggleFreecam))
                {
                    freelookVirtualCamera.transform.position = aimVirtualCamera.transform.position;
                    freelookVirtualCamera.transform.forward = aimVirtualCamera.transform.forward;
                    ChangeStates(GameStates.Freelook);
                }
                
                break;
            case GameStates.Power:
                Vector3 direction = aimVirtualCamera.transform.forward;
                direction.y = 0;
                direction = direction.normalized;
                if (true)
                {
                    direction.y = 0f;
                }
                //golfBall.DrawPath(direction.normalized * 3);
                if (_input.GetButtonDown(RewiredConsts.Action.Golf_Hit))
                {
                    // Max of 13!!!
                    // Drag = .75
                    // Angular Drag = 1
                    golfBall.Hit(direction.normalized, 13);
                    ChangeStates(GameStates.Roll);
                }
                break;
            case GameStates.Roll:
                UpdateAimCamera();
                if (golfBall.isDead)
                    ChangeStates(GameStates.Dead);
                else if (golfBall.isAsleep)
                    ChangeStates(GameStates.Aim);
                break;
            case GameStates.Dead:

                break;
            case GameStates.Freelook:
                if (_currentVirtualCamera != freelookVirtualCamera) ChangeCamera(freelookVirtualCamera);
                if (_input.GetButtonDown(RewiredConsts.Action.Golf_ToggleFreecam))
                {
                    ChangeStates(GameStates.Aim);
                }
                break;
        }
    }

    public void ExitState()
    {
        switch (_gameState)
        {
            case GameStates.Freelook:
                Time.timeScale = 1;
                break;
        }
    }

    /*
    #region Game States

    private class GameStateClass
    {
        protected GolfGameController game;
        private bool _isActive = false;

        public GameStateClass(GolfGameController game)
        {
            this.game = game;
        }

        public void StartOnce()
        {
            if (_isActive) return;
            Start();
            _isActive = true;
        }
        
        protected virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void End()
        {
            _isActive = false;
        }

    }
    
    private class StateAim : GameStateClass
    {
        public StateAim(GolfGameController game) : base(game)
        {
        }
        
        protected override void Start()
        {
            base.Start();
            
            if (game._currentVirtualCamera != game.aimVirtualCamera) game.ChangeCamera(game.aimVirtualCamera);
            game.EnsureWindowsAreOpen(true, game.hitModeSelectionWindow);
        }
    }
    
    #endregion
    */

    private void EnsureWindowIsOpen(UIWindowController window)
    {
        window.gameObject.SetActive(true);
        window.enabled = true;
    }

    private void EnsureWindowsAreOpen(bool doClosingAnimations, params UIWindowController[] windows)
    {
        foreach (var window in _allWindows)
        {
            if (windows.Contains(window))
                EnsureWindowIsOpen(window);
            else
            {
                if (window.enabled)
                {
                    window.Disable(doClosingAnimations);
                }
            }
        }
    }

    public void ChangeCamera(CinemachineVirtualCamera virtualCamera)
    {
        foreach (var cam in _virtualCameras)
        {
            cam.enabled = (cam == virtualCamera);
        }

        _currentVirtualCamera = virtualCamera;
    }

}
