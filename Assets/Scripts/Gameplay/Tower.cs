using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Tower : Piece
{
    [BoxGroup("Settings")][SerializeField] private float _timeToRotate = .5f;
    [BoxGroup("Settings")][SerializeField] private bool _hasSpecificTarget = false;
    [BoxGroup("Settings")][ShowIf("_hasSpecificTarget")][SerializeField] private Target _target;
    [BoxGroup("Settings")] public bool Complete = false;
    [BoxGroup("Settings")] public bool RightTarget = false;



    private Laser _laser;

    private bool _canRotate = true;
    public float Angle { private set; get; }


    protected override void Start() {
        base.Start();
        if (_hasSpecificTarget) {
            _target.HasTower = true;
        }

        Angle = transform.rotation.eulerAngles.y;

        _laser = transform.GetComponentInChildren<Laser>();

    }

    private void OnMouseDown() {
        if (_canRotate) {
            RotateTower();
        }
    }

    private void RotateTower() {
        _canRotate = false;

        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y + 60, 0), _timeToRotate).OnComplete(() => {
            Angle = transform.rotation.eulerAngles.y;
            _canRotate = true;
        });
    }

    public void LockRotate(bool lockRotate) {
        _canRotate = !lockRotate;
    }

    public Target GetTarget() { return _target; }

    public bool HasTarget() { return _hasSpecificTarget; }

    public void Launch() {
        _laser.StartTrail();
    }
}
