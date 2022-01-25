using UnityEngine;
using System.Collections;

// Capture frames as a screenshot sequence. Images are
// stored as PNG files in a folder - these can be combined into
// a movie using image utility software (eg, QuickTime Pro).

public class ExampleClass : MonoBehaviour
{

    private float _lastUpdate = 0;
    private float _lastFixedUpdate = 0;
    private float _lastLateUpdate = 0;

    private int _updateCount = 0;
    private int _fixedUpdateCount = 0;
    private int _lateUpdateCount = 0;

    private float _accumulatedUpdateTime = 0;
    private float _accumulatedFixedUpdateTime = 0;
    private float _accumulatedLateUpdateTime = 0;

    public int frameRate = 10;

    void Start()
    {
        Time.captureDeltaTime = 1.0f / frameRate;
    }

    void FixedUpdate()
    {
        _accumulatedFixedUpdateTime += Time.realtimeSinceStartup - _lastFixedUpdate;
        // Debug.Log("[FixedUpdate] interval=" + (Time.realtimeSinceStartup - _lastFixedUpdate));
        _lastFixedUpdate = Time.realtimeSinceStartup;
        _fixedUpdateCount++;
    }

    void Update()
    {
        _accumulatedUpdateTime += Time.realtimeSinceStartup - _lastUpdate;
        _lastUpdate = Time.realtimeSinceStartup;
        _updateCount++;

        if (_accumulatedUpdateTime >= 1)
        {
            Debug.Log($"_accumulatedUpdateTime: {_accumulatedUpdateTime}, frameCount: {_updateCount}, deltaTime: {Time.deltaTime}");
            _accumulatedUpdateTime = 0;
            _lastUpdate = Time.realtimeSinceStartup;
            _updateCount = 0;
        }
        // Debug.Log("[Update] interval=" + (Time.realtimeSinceStartup - _lastUpdate));


    }

    void LateUpdate()
    {
        _accumulatedLateUpdateTime += Time.realtimeSinceStartup - _lastLateUpdate;
        // Debug.Log("[LateUpdate] interval=" + (Time.realtimeSinceStartup - _lastLateUpdate));
        _lastLateUpdate = Time.realtimeSinceStartup;
        _lateUpdateCount++;
    }

    void OnApplicationQuit()
    {
        Debug.LogError("[TimeScale] " + Time.timeScale);
        Debug.LogError("[FixedUpdate/Update]/LateUpdate] count = " + _fixedUpdateCount + " / " + _updateCount + " / " + _lateUpdateCount);
        Debug.LogError("[FixedUpdate/Update]/LateUpdate] avg time = " + _accumulatedFixedUpdateTime / _fixedUpdateCount
                                                                                                                        + " / " + _accumulatedUpdateTime / _updateCount
                                                                                                                        + " / " + _accumulatedLateUpdateTime / _lateUpdateCount);
        Debug.LogError("[FixedDeltaTime/DeltaTime] " + Time.fixedDeltaTime + " / " + Time.deltaTime);
    }
}
