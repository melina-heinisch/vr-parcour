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

    public void setCurvedRay()
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.BezierCurve;
    }

    public void setShortStraightRay()
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        _xrRayInteractor.maxRaycastDistance = 0.75f;
    }

    public void setLongStraightRay()
    {
        _xrRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        _xrRayInteractor.maxRaycastDistance = 30f;
    }
}
