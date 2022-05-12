using UnityEngine;

namespace Code.Player
{
    public class EnemyColorChanger : MonoBehaviour, IAimable
    {
        [SerializeField] private Color _aimColor;
        [SerializeField] private Renderer _meshRenderer;
        [SerializeField] private string _colorPropertyName;

        private Color _initialColor;
        private bool _aiming;
        private bool _wasAiming;
        
        public void Start()
        {
            _initialColor = _meshRenderer.material.GetColor(_colorPropertyName);
        }

        public void Aim()
        {
            _aiming = true;
        }

        private void HighlightObject()
        {
            _meshRenderer.material.SetColor(_colorPropertyName, _aimColor);
        }
        
        private void DisableHighlightOnObject()
        {
            _meshRenderer.material.SetColor(_colorPropertyName, _initialColor);
        }

        public void LateUpdate()
        {
            if (_aiming != _wasAiming)
            {
                if (_aiming)
                {
                    HighlightObject();
                }
                else
                {
                    DisableHighlightOnObject();
                }
            }

            _wasAiming = _aiming;
            _aiming = false;
        }
    }
}