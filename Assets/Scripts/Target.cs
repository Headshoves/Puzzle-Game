using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Piece
{
    public bool HasTower = false;
    public bool Complete = false;
    public bool RightTower = false;

    protected override void Start() {
        base.Start();
    }
}
