using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using Sentry;
using Sentry.Unity;
using System;

public class SentryMonitor : MonoBehaviour
{
    private int frameCount = 0;

    string statsText;

    ProfilerRecorder totalUsedRecorder;

    private float totalUsedMemory = 0;
    ProfilerRecorder systemUsedMemoryRecorder;
    // ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder gameObjectCountRecorder;
    ProfilerRecorder objectCountRecorder;
    ProfilerRecorder verticesRecorder;
    ProfilerRecorder trianglesRecorder;

    private void Start()
    {
        SentryUnity.Init(o =>
        {
            o.Dsn = "https://449798ce6def47ceba65af22e57d30c9@o1039766.ingest.sentry.io/6115983"; //sentry.io
            // o.Dsn = "http://0c853a6b79b14c30a9ab2b56fbc79449@127.0.0.1:9000/9"; //local self-hosted
            // o.Dsn = "http://888fe57eba274af794fe857cccdbb379@jssz-ai-newton-cpu-03:9000/5"; //server self-hosted
            o.Debug = true;
            o.EnableLogDebouncing = true;
            o.Environment = "production";
            o.TracesSampleRate = 1.0;
            // o.MaxBreadcrumbs = 10; //控制应该捕获breadcrumbs的数量,默认100
        });
    }

    void OnEnable()
    {

        // Momory Profiler
        totalUsedRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        // gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        gameObjectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Game Object Count");
        objectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Object Count");

        // Render Profiler
        verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
    }

    void OnDisable()
    {
        totalUsedRecorder.Dispose();
        systemUsedMemoryRecorder.Dispose();
        // gcMemoryRecorder.Dispose();
        gameObjectCountRecorder.Dispose();
        objectCountRecorder.Dispose();

        verticesRecorder.Dispose();
        trianglesRecorder.Dispose();
    }
    void Update()
    {
        frameCount++;

        var sb = new StringBuilder(500);
        sb.AppendLine($"Frame Count: {frameCount} - {Convert.ToInt32(1 / Time.unscaledDeltaTime)} fps");
        // sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Used Memory: {systemUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");

        totalUsedMemory = totalUsedRecorder.LastValue / (1024 * 1024);
        sb.AppendLine($"Total Used Memory: {totalUsedMemory} MB");

        sb.AppendLine($"Game Object Count: {gameObjectCountRecorder.LastValue}");
        sb.AppendLine($"Object Count: {objectCountRecorder.LastValue}");

        sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
        sb.AppendLine($"Triangles: {trianglesRecorder.LastValue}");

        statsText = sb.ToString();

        SentrySdk.AddBreadcrumb(statsText);

        if (frameCount % 100 == 0)
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.SetTag("TotalUsedMemory", totalUsedMemory.ToString());

                var transaction = SentrySdk.StartTransaction(
                    "SentryMonitorFrequence", //transaction_name
                    "OperationName" //operation_name
                );
                transaction.Finish();

            });
        }
    }

    // void OnGUI()
    // {
    //     GUI.TextArea(new Rect(10, 30, 250, 120), statsText);
    // }
}