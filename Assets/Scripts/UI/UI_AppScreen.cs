using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Google.XR.ARCoreExtensions;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UI_AppScreen : MonoBehaviour
{
    double minhead = 15, minvert = 1.5, minhor = 10;
    double midhead = 6, midvert = 1, midhor = 5;
    double goodhead = 2, goodvert = 0.5, goodhor = 1;
    double besthead = 0.5, bestvert = 0.1, besthor = 0.5;


    [SerializeField] TMP_Text positionText;
    [SerializeField] TMP_Text rotationText;
    [SerializeField] TMP_Text accText;
    [SerializeField] Image image;

    private AREarthManager earth;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
        earth = GeoSpatialManager.Instance.EarthManager;
    }

    // Update is called once per frame
    void Update()
    {
        double lat = earth.CameraGeospatialPose.Latitude;
        double lon = earth.CameraGeospatialPose.Longitude;
        double alt = earth.CameraGeospatialPose.Altitude;
        double heading = earth.CameraGeospatialPose.Heading;
        positionText.text = "Current position lat: " + lat + " lon: " + lon + " alt: " + alt;
        rotationText.text = "Heading: " + heading;

        double head = earth.CameraGeospatialPose.HeadingAccuracy;
        double vert = earth.CameraGeospatialPose.VerticalAccuracy;
        double hor = earth.CameraGeospatialPose.HorizontalAccuracy;
        image.color = Color.red;
        if (head < minhead && vert < minvert && hor < minhor) {
            image.color = Color.blue;
            if (head < midhead && vert < midvert && hor < midhor) {
                image.color = Color.yellow;
                if (head < goodhead && vert < goodvert && hor < goodhor) {
                    image.color = Color.green;
                    if (head < besthead && vert < bestvert && hor < besthor)
                    {
                        image.color = Color.white;
                    }
                }
            }
        }

        accText.text = "Current accuracy: heading: " + head + " horizontal: " + hor + " vertical: " + vert;
    }
}
