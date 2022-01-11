using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using System;
using Crawler.Tools.Patterns;

public class PerformanceMonitor : MonoBehaviourSingleton<PerformanceMonitor>
{
    // Start is called before the first frame update
    ProfilerRecorder totalUsedMemoryRecorder;
    ProfilerRecorder systemUsedMemoryRecorder;
    // ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder gameObjectCountRecorder;
    ProfilerRecorder objectCountRecorder;
    ProfilerRecorder verticesCountRecorder;
    ProfilerRecorder trianglesCountRecorder;

    private float fPS = 0;
    private float totalUsedMemory = 0;
    private float systemUsedMemory = 0;
    private float objectCount = 0;
    private float gameObjectCount = 0;
    private float verticesCount = 0;
    private float trianglesCount = 0;

    void OnEnable()
    {

        // Momory Profiler
        totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        // gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        gameObjectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Game Object Count");
        objectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Object Count");

        // Render Profiler
        verticesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        trianglesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
    }

    void OnDisable()
    {
        totalUsedMemoryRecorder.Dispose();
        systemUsedMemoryRecorder.Dispose();
        // gcMemoryRecorder.Dispose();
        gameObjectCountRecorder.Dispose();
        objectCountRecorder.Dispose();

        verticesCountRecorder.Dispose();
        trianglesCountRecorder.Dispose();
    }
    public float GetFPS()
    {
        return 1 / Time.unscaledDeltaTime;
    }

    public float GetTotalUsedMemory()
    {
        return totalUsedMemoryRecorder.LastValue / (1024 * 1024);
    }

    public float GetVerticesCount()
    {
        return verticesCountRecorder.LastValue;
    }

    public float GetTrianglesCount()
    {
        return trianglesCountRecorder.LastValue;

    }

    void GetPerformance()
    {
        fPS = 1 / Time.unscaledDeltaTime;
        totalUsedMemory = totalUsedMemoryRecorder.LastValue / (1034 * 1024);
        systemUsedMemory = systemUsedMemoryRecorder.LastValue / (1024 * 1024);
        gameObjectCount = gameObjectCountRecorder.LastValue;
        objectCount = objectCountRecorder.LastValue;

        verticesCount = verticesCountRecorder.LastValue;
        trianglesCount = trianglesCountRecorder.LastValue;
    }
}
