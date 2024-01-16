using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

namespace Vehicles {
    public class CarVisual : MonoBehaviour {
        [SerializeField] private MeshRenderer _renderer;
        private Tween _backLightTween;


        public void UpdateBackLight(bool value) {
            this._renderer.material.SetColor("_EmissionColor", value ? Color.white : Color.black);
        }

        public void LightBackLightFor(float second) {
            this.UpdateBackLight(true);
            this._backLightTween = DOVirtual.DelayedCall(second, () => this.UpdateBackLight(false));
        }

        public void CancelBackLight() {
            this._backLightTween.Kill(true);
        }
    }
}