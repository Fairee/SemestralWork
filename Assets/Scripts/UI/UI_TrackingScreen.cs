using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Google.XR.ARCoreExtensions;

public class UI_TrackingScreen : MonoBehaviour
{
    [SerializeField] TMP_Text accUpdate;
    [SerializeField] TMP_Text accWanted;
    private AREarthManager earth;
    // Start is called before the first frame update
    void Start()
    {
        double head = GeoSpatialManager.Instance.WantedHeadingAccuracy;
        double vert = GeoSpatialManager.Instance.WantedVerticalAccuracy;
        double horiz = GeoSpatialManager.Instance.WantedHorizontalAccuracy;
        accWanted.text = "Wanted-- Heading: " + head + " Vertical: " + vert + " Horizontal: " + horiz;
        earth = GeoSpatialManager.Instance.EarthManager;
    }

    private void Update()
    {
        double head = earth.CameraGeospatialPose.HeadingAccuracy;
        double vert = earth.CameraGeospatialPose.VerticalAccuracy;
        double hor = earth.CameraGeospatialPose.HorizontalAccuracy;
        accUpdate.text = "Current-- Heading: " + head + " Vertical: " + vert + " Horizontal: " + hor;
    }
}


