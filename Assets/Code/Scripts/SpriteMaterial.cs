using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using UnityEngine;

namespace BobaStop {
    public class SpriteMaterial : MonoBehaviour {
        private static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        private static readonly int OutlineThickness = Shader.PropertyToID("_OutlineThickness");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int OutlineActive = Shader.PropertyToID("_OutlineActive");

        [ColorUsage(true, true)] [SerializeField]
        private Color _flashColor = Color.white;

        [SerializeField] private float _flashTime = 0.25f;
        [SerializeField] private AnimationCurve _flashSpeedCurve;

        private SpriteRenderer[] _spriteRenderers;
        private Material[] _materials;

        private Coroutine _damageFlashCoroutine;

        private void Awake() {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

            Init();
        }

        private void Init() {
            _materials = new Material[_spriteRenderers.Length];

            // assign sprite renderer materials to _materials
            for (int i = 0; i < _spriteRenderers.Length; i++) {
                _materials[i] = _spriteRenderers[i].material;
            }
        }

        public void CallDamageFlash() {
            _damageFlashCoroutine = StartCoroutine(DamageFlasher());
        }

        private IEnumerator DamageFlasher() {
            SetFlashColor();

            float currentFlashAmount = 0f;
            float elapsedTime = 0f;
            while (elapsedTime < _flashTime) {
                elapsedTime += Time.deltaTime;

                currentFlashAmount = Mathf.Lerp(1f, _flashSpeedCurve.Evaluate(elapsedTime), elapsedTime / _flashTime);
                SetFlashAmount(currentFlashAmount);

                yield return null;
            }
        }

        private void SetFlashColor() {
            foreach (var t in _materials) {
                t.SetColor(FlashColor, _flashColor);
            }
        }

        private void SetFlashAmount(float amount) {
            foreach (var t in _materials) {
                t.SetFloat(FlashAmount, amount);
            }
        }
        
        public void SetOutlineThickness(float outlineThickness) {
            foreach (var t in _materials) {
                t.SetFloat(OutlineThickness, outlineThickness);
            }
        }

        public void SetOutlineColor(Color color) {
            foreach (var t in _materials) {
                t.SetColor(OutlineColor, color);
            }
        }
        
        public void TurnOutlineOn() {
            foreach (var t in _materials) {
                t.SetFloat(OutlineActive, 1f);
            }
        }
        
        public void TurnOutlineOff() {
            foreach (var t in _materials) {
                t.SetFloat(OutlineActive, 0f);
            }
        }
    }
}