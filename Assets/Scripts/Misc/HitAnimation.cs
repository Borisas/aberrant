using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class HitAnimation {
    
    private PrimeTween.Tween _blinkTween;
    private PrimeTween.Tween _scaleAnim;
    private int _shaderBlinkRatio;
    private float _duration = 0.0f;
    private MaterialPropertyBlock _block = new MaterialPropertyBlock();
    private Renderer _rend = null;
    private Transform _rendTransform = null;
    private Vector3 _originalScale = Vector3.one;

    public HitAnimation(string name, float duration, Renderer rend) {
        _duration = duration;
        _shaderBlinkRatio = Shader.PropertyToID(name);
        _rend = rend;
        
        _rendTransform = _rend.transform;
        _originalScale = _rendTransform.localScale;
    }

    public void Play() {
        Kill();
        
        _blinkTween = Tween.Custom(1.0f, 0.0f, 0.2f, (x) => {
            _rend.GetPropertyBlock(_block);
            _block.SetFloat(_shaderBlinkRatio, x);
            _rend.SetPropertyBlock(_block);
        }, Ease.OutQuad);

        _rendTransform.localScale = _originalScale * 0.85f;
        _scaleAnim = Tween.Scale(_rendTransform,_originalScale,0.25f);

        Scene.Effects.SpawnHitParticles(_rend, _rendTransform);
    }

    public void Kill() {
        _blinkTween.Stop();
        _scaleAnim.Stop();
        _rendTransform.localScale = _originalScale;
    }
}
