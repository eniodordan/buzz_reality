using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace BuzzReality
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputActionProperty leftPauseButton;
        [SerializeField] private GameObject leftDirectInteractor;
        [SerializeField] private GameObject leftRayInteractor;
        [Space(10)]
        [SerializeField] private InputActionProperty rightPauseButton;
        [SerializeField] private GameObject rightDirectInteractor;
        [SerializeField] private GameObject rightRayInteractor;

        private XRBaseControllerInteractor _leftControllerInteractor;
        private XRBaseControllerInteractor _rightControllerInteractor;
        
        private bool _isLeftActive = false;
        private bool _isDirectActive = false;
        
        public static event Action OnPauseButtonPressed;

        private void Start()
        {
            _leftControllerInteractor = leftDirectInteractor.GetComponent<XRBaseControllerInteractor>();
            _rightControllerInteractor = rightDirectInteractor.GetComponent<XRBaseControllerInteractor>();
        }

        private void OnEnable()
        {
            MenuManager.OnMenuStateChanged += OnMenuStateChanged;
        }

        private void OnDisable()
        {
            MenuManager.OnMenuStateChanged -= OnMenuStateChanged;
        }
        
        private void OnMenuStateChanged(MenuState oldState, MenuState newState)
        {
            _isDirectActive = newState.Equals(MenuState.INGAME_MENU);

            if (_isLeftActive)
            {
                leftDirectInteractor.SetActive(_isDirectActive);
                leftRayInteractor.SetActive(!_isDirectActive);
            }
            else
            {
                rightDirectInteractor.SetActive(_isDirectActive);
                rightRayInteractor.SetActive(!_isDirectActive);
            }
        }

        public void UpdateDominantHand(int index)
        {
            if (index == 0)
            {
                leftDirectInteractor.SetActive(false);
                leftRayInteractor.SetActive(false);
                
                rightDirectInteractor.SetActive(_isDirectActive);
                rightRayInteractor.SetActive(!_isDirectActive);
                
                _isLeftActive = false;
            }
            else if (index == 1)
            {
                rightDirectInteractor.SetActive(false);
                rightRayInteractor.SetActive(false);
                
                leftDirectInteractor.SetActive(_isDirectActive);
                leftRayInteractor.SetActive(!_isDirectActive);
                
                _isLeftActive = true;
            }
        }

        public void SendHapticImpulse()
        {
            if (_isLeftActive)
            {
                _leftControllerInteractor.xrController.SendHapticImpulse(0.75f, 0.25f);
            }
            else
            {
                _rightControllerInteractor.xrController.SendHapticImpulse(0.75f, 0.25f);
            }
        }

        private void Update()
        {
            var state = MenuManager.Instance.CurrentMenuState;
            if (!state.Equals(MenuState.INGAME_MENU) && !state.Equals(MenuState.PAUSE_MENU)) return;
            
            if (leftPauseButton.action.WasPressedThisFrame() || rightPauseButton.action.WasPressedThisFrame())
                OnPauseButtonPressed?.Invoke();
        }
    }
}
