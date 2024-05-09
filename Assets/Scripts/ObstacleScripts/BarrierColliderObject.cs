using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierColliderObject : MonoBehaviour
{
    public event EventHandler ColliderTouched;

    private void OnCollisionEnter(Collision collision)
    {
        ColliderTouched?.Invoke(this,EventArgs.Empty);
    }
}
