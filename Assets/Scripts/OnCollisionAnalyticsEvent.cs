using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class OnCollisionAnalyticsEvent : MonoBehaviour
{
    private float _time = 5.0f;

    private IEnumerator Timer()
    {
        while(_time >= 0.0f)
        {
            _time -= Time.deltaTime;
            yield return 0;
        }
        _time = 5.0f;
    }

    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if(_time == 5.0f)
        {
            Analytics.CustomEvent ("Collider Hit");
        }

        StartCoroutine (Timer ());
    }
}
