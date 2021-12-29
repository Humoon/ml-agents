using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using Sentry;
using Sentry.Unity;
using System;

public class ExampleScript : MonoBehaviour
{
    // private float f_UpdateInterval = 0.5F;
    // private float f_LastInterval;

    private int frameCount = 0;

    string statsText;

    ProfilerRecorder totalUsedRecorder;
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;
    ProfilerRecorder mainThreadTimeRecorder;
    //场景中游戏对象的总数
    ProfilerRecorder gameObjectCountRecorder;
    //所有资源的总数。如果此数值随时间推移而上升，表示应用程序创建了一些永不销毁或上载的游戏对象或其他资源
    ProfilerRecorder objectCountRecorder;
    //Unity 在一帧内处理的顶点数
    ProfilerRecorder verticesRecorder;
    //Unity 在一帧内处理的三角形数
    ProfilerRecorder trianglesRecorder;

    private void Start()
    {
        SentryUnity.Init(o =>
        {
            o.Dsn = "http://0c853a6b79b14c30a9ab2b56fbc79449@127.0.0.1:9000/9";
            o.Debug = true;
            o.EnableLogDebouncing = true;
            o.Environment = "production";
            o.TracesSampleRate = 1.0;
            // o.MaxBreadcrumbs = 10; //控制应该捕获breadcrumbs的数量,默认100
        });

        // f_LastInterval = Time.realtimeSinceStartup;
    }


    static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        unsafe
        {
            var samples = stackalloc ProfilerRecorderSample[samplesCount];
            recorder.CopyTo(samples, samplesCount);
            for (var i = 0; i < samplesCount; ++i)
                r += samples[i].Value;
            r /= samplesCount;
        }

        return r;
    }

    void OnEnable()
    {
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);

        // Momory Profiler
        totalUsedRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");

        gameObjectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Game Object Count");
        objectCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Object Count");

        // Render Profiler
        verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
    }

    void OnDisable()
    {
        totalUsedRecorder.Dispose();
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        mainThreadTimeRecorder.Dispose();
        gameObjectCountRecorder.Dispose();
        objectCountRecorder.Dispose();

        verticesRecorder.Dispose();
        trianglesRecorder.Dispose();
    }

    void Update()
    {
        frameCount++;
        // if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        // {
        // f_LastInterval = Time.realtimeSinceStartup;

        var sb = new StringBuilder(1000);
        sb.AppendLine($"Frame Count: {frameCount} - {Convert.ToInt32(1 / Time.unscaledDeltaTime)} fps");
        sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"Total Used Memory: {totalUsedRecorder.LastValue / (1024 * 1024)} MB");

        sb.AppendLine($"Game Object Count: {gameObjectCountRecorder.LastValue}");
        sb.AppendLine($"Object Count: {objectCountRecorder.LastValue}");
        sb.AppendLine($"Vertices: {verticesRecorder.LastValue}");
        sb.AppendLine($"Triangles: {trianglesRecorder.LastValue}");

        statsText = sb.ToString();

        SentrySdk.AddBreadcrumb(statsText);
        // }
    }

    void OnGUI()
    {
        GUI.TextArea(new Rect(10, 30, 250, 150), statsText);
    }
}