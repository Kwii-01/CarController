using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using DG.Tweening;

using UnityEngine;

namespace TankController {
    public class Cam : MonoBehaviour {
        public static Cam Instance { get; private set; } = default;

        [field: SerializeField] public Camera Camera { get; private set; } = default;
        private CinemachineBrain _brain;
        private CinemachineVirtualCamera _activeVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _perlinNoise;
        private Tween _shakeTween;

        private void Awake() {
            if (Instance != null) {
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = default;
            }
        }

        private void Start() {
            this._brain = this.Camera.GetComponent<CinemachineBrain>();
        }

        public void DoShake(float duration, float amplitude = 1f, float frequency = 10f) {
            this._shakeTween.Kill(true);
            CinemachineVirtualCamera vcam = this._brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (this._activeVirtualCamera != vcam) {
                this._activeVirtualCamera = vcam;
                this._perlinNoise = this._activeVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            this._perlinNoise.m_AmplitudeGain = amplitude;
            this._perlinNoise.m_FrequencyGain = frequency;
            this._shakeTween = DOVirtual.DelayedCall(duration, () => this._perlinNoise.m_FrequencyGain = 0f);
        }
    }
}