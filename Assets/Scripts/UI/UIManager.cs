using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject UI_TrackingPermissionScreen;
    [SerializeField] private GameObject UI_CameraPermissionScreen;
    [SerializeField] private GameObject UI_ErrorScreen;
    [SerializeField] private GameObject UI_TrackingScreen;
    [SerializeField] private GameObject UI_NormalScreen;


    private ErrorState lastState;


    // Start is called before the first frame update
    void Start()
    {
        GeoSpatialManager.Instance.ErrorStateChanged.AddListener(StateUpdate);
    }

    public virtual void StateUpdate(ErrorState errState, string message) {
        Debug.Log(errState + " " + message);
        if (lastState == errState) {
            return;
        }

        UI_CameraPermissionScreen.SetActive(false);
        UI_TrackingPermissionScreen.SetActive(false);
        UI_ErrorScreen.SetActive(false);
        UI_TrackingScreen.SetActive(false);
        UI_NormalScreen.SetActive(false);

        if (errState == ErrorState.Location)
        {
            UI_TrackingPermissionScreen.SetActive(true);
        }
        else if (errState == ErrorState.Camera)
        {
            UI_CameraPermissionScreen.SetActive(true);
        }
        else if (errState == ErrorState.Tracking)
        {
            //UI_TrackingScreen.SetActive(true);
            UI_NormalScreen.SetActive(true);
        }
        else if (errState == ErrorState.Message)
        {
            UI_ErrorScreen.SetActive(true);
        }
        else {
            UI_NormalScreen.SetActive(true);
        }
    
    
    }
}
