using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter _otherTeleporter;
    [SerializeField] private int _teleportPower;

    public event EventHandler<EventArgs> Teleported;

    private void OnTriggerStay(Collider other)
    {
        float zPos = transform.worldToLocalMatrix.MultiplyPoint3x4(other.transform.position).z;
        if (zPos > 0 && other.TryGetComponent(out Vehicle _))
        {
            Teleport(other.transform);
        }
    }

    // ABSTRACTION
    private void Teleport(Transform obj)
    {
        //position
        Vector3 localPos = transform.worldToLocalMatrix.MultiplyPoint3x4(obj.position);
        localPos = new Vector3(-localPos.x,localPos.y,-localPos.z);
        obj.position = _otherTeleporter.transform.localToWorldMatrix.MultiplyPoint3x4(localPos)+ Vector3.up*0.5f;

        //rotation
        Quaternion difference = _otherTeleporter.transform.rotation * Quaternion.Inverse(transform.rotation * Quaternion.Euler(0, 180, 0));
        obj.rotation = difference ;

        //Force
        Rigidbody objRigid = obj.GetComponent<Rigidbody>();
        objRigid.AddForce(_teleportPower * (obj.transform.localRotation *Vector3.forward), ForceMode.Acceleration);

        Teleported?.Invoke(this,EventArgs.Empty);
        gameObject.SetActive(false);
    }
}
