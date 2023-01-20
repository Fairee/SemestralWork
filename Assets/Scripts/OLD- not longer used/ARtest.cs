using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARCore;
using TMPro;
using System;

public class ARtest : MonoBehaviour
{
    bool canBeRun = false;
    bool startLocationAcquired = false;
    public ARSessionOrigin arSessionOrigin;
    public AREarthManager arEarthManager;
    public TextMeshProUGUI textm1;
    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public TextMeshProUGUI text5;
    public TextMeshProUGUI text6;
    public Camera cam;
    VpsAvailabilityPromise promise;
    public Vector3D originPos;
    public List<GroupHead> head;
    krovakConverter conv = new krovakConverter(); 

    private void Awake()
    {
        if (arSessionOrigin == null) {
            Debug.LogError("Couldn't find ARSessionOrigin");
        }

        if (arEarthManager == null) {
            Debug.LogError("Couldnt find AREarthManager");
        }
    }


    public void setLocations() {

        GeospatialPose p = arEarthManager.CameraGeospatialPose;
        Vector3D pos = conv.transform(p.Latitude, p.Longitude, p.Altitude);
        originPos = pos;

        cam.transform.position = new Vector3(0, 0, 0);
        cam.transform.rotation = new Quaternion(0,0,0,0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        textm1.text = arEarthManager.EarthTrackingState + " " + arEarthManager.EarthState;
        if (arEarthManager.EarthTrackingState == TrackingState.Tracking)
        {
            GeospatialPose p = arEarthManager.CameraGeospatialPose;
            Vector3D pos = conv.transform(p.Latitude, p.Longitude, p.Altitude);
            if (startLocationAcquired == false)
            {
                promise = AREarthManager.CheckVpsAvailability(50.159778, 14.746013);
                originPos = pos;

                startLocationAcquired = true;
            }
            else
            {
                //this.transform.position = (pos - originPos).ToFloatWithSwap();
                //cam.transform.position = (pos - originPos).ToFloatWithSwap();
                Vector3D otherpos = originPos + new Vector3D(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
                text.text = "Current position from converter: " + pos.x + " " + pos.y + " " + pos.z;
                text2.text = "Current position from origin + cam transform " + otherpos.x + " " + otherpos.y + " " + otherpos.z;
                text3.text = "Origin position: " + originPos.x + " " + originPos.y + " " + originPos.z;
                text4.text = "Camera position: " + cam.transform.position.x + " " + cam.transform.position.y + " " + cam.transform.position.z;
                text5.text = "VPS STATE: " + promise.State;
                text6.text = "VPS RESULT: " + promise.Result;
                //text4.text = "My rotation " + cam.transform.rotation.x + " " + cam.transform.rotation.y + " " + cam.transform.rotation.z;
            }

           /* if (promise.State == PromiseState.Done)
            {
                promise = AREarthManager.CheckVpsAvailability(p.Latitude, p.Longitude);
            }*/
        }






        /*bool d = arEarthManager.isActiveAndEnabled;
         text3.text = Time.realtimeSinceStartup.ToString();
         var a = arEarthManager.EarthState;
         text.text = a.ToString();
         text2.text = arEarthManager.EarthTrackingState.ToString();
         if (arEarthManager.EarthTrackingState == TrackingState.Tracking)
         {
             GeospatialPose pose = new GeospatialPose();
             pose.Longitude = 14.745861;
             pose.Latitude = 50.160316;
             Vector2 position = conv.forward(pose);
             text.text = pose.Latitude.ToString() + " " + pose.Longitude.ToString();
             text2.text = position.x + " " + position.y;
         }*/
    }
}






