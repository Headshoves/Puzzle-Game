using DG.Tweening;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float _timeToRotate = .5f;

    private bool _canRotate = true;

    private void OnMouseDown() {
        if (_canRotate) {
            RotateTower();
        }
    }

    private void RotateTower() {
        _canRotate = false;

        transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y + 60, 0), _timeToRotate).OnComplete(() => {
            _canRotate = true;
        });
    }

    public void LockRotate(bool lockRotate) {
        _canRotate = !lockRotate;
    }
}
