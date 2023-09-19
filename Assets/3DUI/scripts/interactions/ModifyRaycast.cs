using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ModifyRaycast : MonoBehaviour
{
    private XRRayInteractor _xrRayInteractor;
    void Start()
    {
        _xrRayInteractor = gameObject.GetComponent<XRRayInteractor>();
    }

    public void setCurvedRay() // for teleportation gun
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.BezierCurve;
    }

    public void setShortStraightRay() // to pick up teleportation gun
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        _xrRayInteractor.maxRaycastDistance = 0.75f;
    }

    public void setLongStraightRay() // for keyboard 
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        _xrRayInteractor.maxRaycastDistance = 30f;
    }
}
