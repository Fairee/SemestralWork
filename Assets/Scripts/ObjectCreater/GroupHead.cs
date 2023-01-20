using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupHead : MonoBehaviour
{

    /// <summary>
    /// BASED ON INDEX ANCHOR THE OBJECT ON A SPECIFIC PLACE
    /// </summary>
    [SerializeField]
    public Vector3D originalPosition;
    public int index;
    public ARGeospatialAnchor anchor;

    public void Start()
    {
        if (index == 0)
        {
            GeospatialPose geoPose = new GeospatialPose();
            geoPose.Longitude = 14.7446369;
            geoPose.Latitude = 50.1610154;
            geoPose.Altitude = 187;
            anchor = GeoSpatialManager.Instance.RequestGeospatialAnchor(geoPose);
            transform.parent = anchor.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else if (index == 1)
        {
            GeospatialPose geoPose = new GeospatialPose();
            geoPose.Longitude = 14.74478;
            geoPose.Latitude = 50.1610329;
            geoPose.Altitude = 187;
            anchor = GeoSpatialManager.Instance.RequestGeospatialAnchor(geoPose);
            transform.parent = anchor.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
}
