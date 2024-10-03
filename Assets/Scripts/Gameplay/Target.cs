using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Piece
{
    public bool HasTower = false;
    public bool Complete = false;
    public bool RightTower = false;

    public void SetTower(Color towerColor) {
        HasTower = true;
        _hasColor = true;
        SetColor(towerColor);
    }


    protected override void Start() {
        base.Start();
    }
}
