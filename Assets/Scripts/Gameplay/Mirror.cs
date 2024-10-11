using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : Piece {
    [SerializeField] private float _timeToRotate = .5f;

    private bool _canRotate = true;

    public float RotationAngle { private set; get; }

    protected override void Start() {
        base.Start();

        RotationAngle = (int)_initialAngle;
    }

    private void OnMouseDown() {
        if(CanPlay) {
            if (_canRotate) {
                RotateTower();
            }
        }
    }

    private void RotateTower() {
        _canRotate = false;

        float y = transform.rotation.eulerAngles.y + 60;
        RotationAngle = RotationAngle + 60 < 360 ? y : 30;

        transform.DORotate(new Vector3(0, y, 0), _timeToRotate).OnComplete(() => {
            _canRotate = true;
        });
    }

    public void LockRotate(bool lockRotate) {
        _canRotate = !lockRotate;
    }
}
