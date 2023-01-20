using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ErrorScreen : MonoBehaviour
{

    [SerializeField] TMP_Text messageText;
    // Start is called before the first frame update
    void Start()
    {
        GeoSpatialManager.Instance.ErrorStateChanged.AddListener(StateUpdate);
    }

    void StateUpdate(ErrorState errState, string message) {
        if (message != null) {
            messageText.text = message;
        }
    
    
    }
}
