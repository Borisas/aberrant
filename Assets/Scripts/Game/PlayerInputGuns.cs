using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputGuns : MonoBehaviour {

    [SerializeField] private GameObject _gunLeft = null;
    [SerializeField] private GameObject _gunRight = null;
    [SerializeField] private Projectile _bulletProjectile = null;

    [SerializeField] private float _bulletDamage = 0.0f;
    [SerializeField] private float _fireInterval = 0.15f;
    private float _fireTimer = 0.0f;
    
    void Update() {

        if (Scene.GameController.IsWaveInProgress() == false) return;

        if (_fireTimer < _fireInterval) {
            _fireTimer += Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && _fireTimer >= _fireInterval) {
            FireBulletAtScreenPos(Input.mousePosition);
            _fireTimer -= _fireInterval;
        }
    }

    void FireBulletAtScreenPos(Vector3 screenPos) {
        Vector3 worldPos = Scene.GameCamera.ScreenToWorldPoint(screenPos);

        Fire(_gunRight, worldPos);
        Fire(_gunLeft, worldPos);
    }

    void Fire(GameObject weapon, Vector3 worldPos) {
        var dir = worldPos - weapon.transform.position;
        var rot = (new Vector2(dir.x, dir.y)).AngleDeg();

        weapon.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rot + 90.0f);

        var proj = ObjectPool.Get(_bulletProjectile);
        proj.Setup(null, _bulletDamage, dir);
        proj.transform.position = weapon.transform.position;
    }
}
