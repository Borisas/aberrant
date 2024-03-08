using UnityEngine;


public class DamagingZoneProjectile : DamagingZone, ISetupHitterWithDirection {

    private Rigidbody2D _body = null;
    private TrailRenderer _trail = null;
    [SerializeField] private float _speed;
    [SerializeField] private bool _rotateToDirection = true;
    private Vector2 _direction;
    bool _firstFrame = false;

    protected override void Awake() {
        base.Awake();
        _body = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
    }

    public void Setup(Actor owner, float damage, Vector2 direction) {

        _owner = owner;
        _damage = damage;
        _direction = direction;
        _firstFrame = true;

        if (_trail != null) {
            _trail.enabled = false;
        }

        if (_rotateToDirection) {
            var r = _direction.AngleDeg();
            var rot = transform.localRotation.eulerAngles;
            rot.z = r;
            transform.localRotation = Quaternion.Euler(rot);
        }
    }


    private void FixedUpdate() {

        var p = _body.position + _direction * (_speed * Time.fixedDeltaTime);
        _body.MovePosition(p);

        if (Mathf.Abs(p.x) > 20.0f || Mathf.Abs(p.y) > 20.0f) {
            DestroySelf();
        }
    }


    protected override void Update() {
        if (_firstFrame && _trail != null) {
            _trail.enabled = true;
            _trail.Clear();
            _firstFrame = false;
        }

        base.Update();
    }
}