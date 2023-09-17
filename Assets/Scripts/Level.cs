using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

namespace BuzzReality
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private MeshRenderer wireMeshRenderer;
        [SerializeField] private XRBaseInteractable wireInteractable;
        [SerializeField] private List<XRBaseInteractable> obstaclesInteractable;
        [SerializeField] public List<XRBaseInteractable> invalidWiresInteractable;
        [SerializeField] private LevelDifficulty levelDifficulty;
        
        private LevelState _levelState = LevelState.UNATTENDED;

        public LevelDifficulty LevelDifficulty => levelDifficulty;
        public LevelState LevelState => _levelState;

        public event Action OnInteractableEntered;
        public event Action OnInvalidWireEntered;
        public event Action<LevelState> OnStateChanged;
        
        private void OnEnable()
        {
            wireInteractable.hoverEntered.AddListener(OnEnterInteractable);
            foreach (var obstacle in obstaclesInteractable)
            {
                obstacle.hoverEntered.AddListener(OnEnterInteractable);
            }

            foreach (var invalidWire in invalidWiresInteractable)
            {
                invalidWire.hoverEntered.AddListener(OnEnterInvalidWire);
            }
        }

        private void OnDisable()
        {
            wireInteractable.hoverEntered.RemoveListener(OnEnterInteractable);
            foreach (var obstacle in obstaclesInteractable)
            {
                obstacle.hoverEntered.RemoveListener(OnEnterInteractable);
            }
            
            foreach (var invalidWire in invalidWiresInteractable)
            {
                invalidWire.hoverEntered.RemoveListener(OnEnterInvalidWire);
            }
        }
        
        private void OnEnterInteractable(HoverEnterEventArgs _) => OnInteractableEntered?.Invoke();

        private void OnEnterInvalidWire(HoverEnterEventArgs _) => OnInvalidWireEntered?.Invoke();
        
        public void UpdateWireMaterial(Material material) => wireMeshRenderer.material = material;

        public void ChangeLevelState(LevelState levelState)
        {
            _levelState = levelState;
            OnStateChanged?.Invoke(_levelState);
        }

    }
}
