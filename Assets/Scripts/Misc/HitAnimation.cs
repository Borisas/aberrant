using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class HitAnimation {
    
    private List<PrimeTween.Tween> _blinkTween = new List<Tween>();
    private PrimeTween.Tween _scaleAnim;
    private int _shaderBlinkRatio;
    private float _duration = 0.0f;
    private MaterialPropertyBlock _block = new MaterialPropertyBlock();
    private List<Renderer> _rend = null;
    private Transform _rendTransform = null;
    private Vector3 _originalScale = Vector3.one;

    public HitAnimation(string name, float duration, Renderer rend, Transform scaleTarget) {
        _duration = duration;
        _shaderBlinkRatio = Shader.PropertyToID(name);
        _rend = new List<Renderer>();
        _rend.Add(rend);
        
        _rendTransform = scaleTarget;
        _originalScale = _rendTransform.localScale;
    }

    public HitAnimation(string name, float duration, SpriteRenderer[] rend, Transform scaleTarget) {

        _duration = duration;
        _shaderBlinkRatio = Shader.PropertyToID(name);
        _rend = new List<Renderer>();
        _rend.AddRange(rend);
        
        _rendTransform = scaleTarget;
        _originalScale = _rendTransform.localScale;
    }

    public void Play() {
        Kill();

        _blinkTween.Clear();
        
        foreach (var r in _rend) {
            var bt = Tween.Custom(1.0f, 0.0f, 0.2f, (x) => {
                r.GetPropertyBlock(_block);
                _block.SetFloat(_shaderBlinkRatio, x);
                r.SetPropertyBlock(_block);
            }, Ease.OutQuad);
            _blinkTween.Add(bt);
        }

        _rendTransform.localScale = _originalScale * 0.85f;
        _scaleAnim = Tween.Scale(_rendTransform,_originalScale,0.25f);

        Scene.Effects.SpawnHitParticles(_rend[Random.Range(0,_rend.Count)], _rendTransform);
    }

    public void Kill() {
        foreach (var b in _blinkTween) {
            b.Stop();
        }
        _scaleAnim.Stop();
        _rendTransform.localScale = _originalScale;
    }
}
