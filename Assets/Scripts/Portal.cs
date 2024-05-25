using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Portal : MonoBehaviour
{
    
    [SerializeField] private Portal _otherPortal;
    [SerializeField] private Camera _portalView;

    private void Start()
    {
        _otherPortal._portalView.targetTexture = new RenderTexture(Screen.width, Screen.height,24);
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = _otherPortal._portalView.targetTexture;
    }

    private void Update()
    {
        PortalsVisualUpdate();
    }


    private void PortalsVisualUpdate()
    {
        //Position
        Vector3 lookerPosition = _otherPortal.transform.worldToLocalMatrix.MultiplyPoint3x4(Camera.main.transform.position);
        lookerPosition = new Vector3(-lookerPosition.x, Camera.main.transform.position.y, -lookerPosition.z);
        _portalView.transform.localPosition = lookerPosition;

        //Rotation
        Quaternion difference = transform.rotation * Quaternion.Inverse(_otherPortal.transform.rotation * Quaternion.Euler(0, 180, 0));
        _portalView.transform.rotation = difference * Camera.main.transform.rotation;

        //Clipping
        _portalView.nearClipPlane = lookerPosition.magnitude;
    }


}


