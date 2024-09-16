using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class Tower : Piece
{
    [BoxGroup("General Settings")][SerializeField] private float _timeToRotate = .5f;
    [BoxGroup("General Settings")] public bool Complete = false;

    [BoxGroup("Tower Settings")][SerializeField] private bool _hasSpecificTarget = false;
    [BoxGroup("Tower Settings")][ShowIf("_hasSpecificTarget")][SerializeField] private Target _target;
    [BoxGroup("Tower Settings")] public bool RightTarget = false;

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
        if (_hasLimitMoves) {
            if (_moveCount > 0) {
                if (_canRotate) {
                    _moveCount--;
                    _moveText.text = _moveCount.ToString();
                    RotateTower();
                }
            }
        }
        else {
            if (_canRotate) {
                RotateTower();
            }
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
