using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour {

    public void SpawnHitParticles(Renderer rend, Transform t) {

        // Vector3 hitOffset = new Vector3(
        //     Random.Range(0.2f,0.8f),
        //     Random.Range(-0.2f,0.2f),
        //     0.0f
        // );

        var bounds = rend.bounds;
        float ex = bounds.extents.x * 0.9f;
        float ey = bounds.extents.y * 0.9f;

        var p = bounds.center +
            new Vector3(
                Random.Range(-ex, ex),
                Random.Range(-ey, ey),
                0.0f
            );
        
        var go = ObjectPool.Get(Database.GetInstance().Effects.HitAnim, transform);
        go.transform.position = p;
    }

    public void SpawnBloodDrop(Transform t, int count = 1) {

        var p = t.position;
        
        for (int i = 0; i < count; i++) {
            var go = ObjectPool.Get(Database.GetInstance().Effects.Blood, transform);
            p.x += UnityEngine.Random.Range(-0.15f, 0.15f);
            p.y += UnityEngine.Random.Range(-0.15f, 0.15f);
            go.transform.position = p;
        }

    }
}
