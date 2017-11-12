using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsStartupEvent : MonoBehaviour
{   
    private void Start ( )
    {
        Analytics.CustomEvent ("Startup Event", new Dictionary<string, object>
        {
            {"Platform", Application.platform},
            {"Local Time", System.DateTime.Now}
        });
    }    
}
