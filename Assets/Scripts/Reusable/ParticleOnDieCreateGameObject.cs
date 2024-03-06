using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleOnDieCreateGameObject : MonoBehaviour {


    struct TrackedParticles {
        public uint Seed;
        public Vector3 Position;
        public bool Updated;
    }

    [SerializeField] private GameObject[] _gameObjects = null;
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle[] _particles;
    private List<TrackedParticles> _trackedParticles = new List<TrackedParticles>();

    void Awake() {
        _particleSystem = GetComponent<ParticleSystem>();

        var psm = _particleSystem.main;
        int c = psm.maxParticles;
        _particles = new ParticleSystem.Particle[c];
    }

    private void OnEnable() {
        _trackedParticles.Clear();
    }

    void LateUpdate() {
        DirtyParticles();
        
        int numParticlesAlive = _particleSystem.GetParticles(_particles);
        for (int i = 0; i < numParticlesAlive; i++) {
            UpdateParticle(_particles[i].randomSeed, _particles[i].position);
        }
        
        CheckParticles();
    }

    void DirtyParticles() {
        for (int i = 0; i < _trackedParticles.Count; i++) {
            var tp = _trackedParticles[i];
            tp.Updated = false;
            _trackedParticles[i] = tp;
        }
    }

    void UpdateParticle(uint seed, Vector3 p) {
        for (int i = 0; i < _trackedParticles.Count; i++) {
            if (_trackedParticles[i].Seed == seed) {
                var tp = _trackedParticles[i];
                tp.Position = p;
                tp.Updated = true;
                _trackedParticles[i] = tp;
                return;
            }
        }
        
        _trackedParticles.Add(new TrackedParticles{
            Seed    = seed,
            Position = p,
            Updated = true
        });
    }

    void CheckParticles() {
        for (int i = 0; i < _trackedParticles.Count; i++) {
            if (_trackedParticles[i].Updated == false) {
                SpawnGameObject(_trackedParticles[i].Position);
                _trackedParticles.RemoveAt(i);
                i--;
            }
        }
    }

    void SpawnGameObject(Vector3 at) {
        var rndGo = _gameObjects.Random();
        var go = ObjectPool.Get(rndGo);
        go.transform.position = transform.localToWorldMatrix.MultiplyPoint(at);
        go.transform.localScale = new Vector3(
            Random.Range(0.5f,1.5f),
            Random.Range(0.5f,1.5f),
            1.0f
        );
    }
}
