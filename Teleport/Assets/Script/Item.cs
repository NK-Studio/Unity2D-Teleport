﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody2D rig;

    void Update()
    {
        var xx = Input.GetAxis("Vertical");
        rig.velocity = new Vector2(xx * 300f * Time.deltaTime,rig.velocity.y);
    }
}
