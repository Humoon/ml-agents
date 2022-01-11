using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using Sentry;
using Sentry.Unity;
using System;
using System.Threading;

public class SentryTest : MonoBehaviour
{
    private int frameCount = 0;
    private float fps = 0f;
    private readonly float m_updateTime = 0.2f;
    private float m_lastUpdateTime = 0f;

    private Rect m_fps, frame_count;
    private GUIStyle m_style = new GUIStyle();

    private void Start()
    {
        SentryUnity.Init(o =>
        {
            // o.Dsn = "https://449798ce6def47ceba65af22e57d30c9@o1039766.ingest.sentry.io/6115983"; //sentry.io
            // o.Dsn = "http://0c853a6b79b14c30a9ab2b56fbc79449@127.0.0.1:9000/9"; //local self-hosted
            o.Dsn = "http://888fe57eba274af794fe857cccdbb379@jssz-ai-newton-cpu-03:9000/5"; //server self-hosted
            o.Debug = true;
            // o.EnableLogDebouncing = true;
            o.TracesSampleRate = 1.0;
            // o.MaxBreadcrumbs = 10; //控制应该捕获breadcrumbs的数量,默认100
        });

        m_fps = new Rect(0, 0, 100, 40);
        frame_count = new Rect(0, 40, 100, 40);
        m_style.fontSize = 30;
        m_style.normal.textColor = Color.red;
    }

    void Awake()
    {
        Application.targetFrameRate = 20;
    }

    void Update()
    {
        frameCount++;
        CreateTransaction();

        if (Time.realtimeSinceStartup - m_lastUpdateTime >= m_updateTime)
        {
            fps = 1 / Time.unscaledDeltaTime;
            m_lastUpdateTime = Time.realtimeSinceStartup;
        }

        Debug.Log($"Current Frame Count: {frameCount}");

        ThrowException();
    }

    void CreateTransaction()
    {
        var transaction = SentrySdk.StartTransaction(
            "SentryMonitorFreq", //transaction_name
            "CollectInfo" //operation_name
        );
        var rootSpan = transaction.StartChild(
            "RootSpan"//operation_name
        );

        var childSpan1 = rootSpan.StartChild("ChildSpan1");
        Thread.Sleep(5);
        childSpan1.Finish();

        var childSpan2 = rootSpan.StartChild("ChildSpan2");
        Thread.Sleep(3);
        childSpan2.Finish();

        rootSpan.Finish();
        transaction.Finish();
    }

    void ThrowException()
    {
        try
        {
            throw new Exception("SentryExceptionTest");
        }
        catch (Exception err)
        {
            SentrySdk.CaptureException(err);
        }
    }

    void OnGUI()
    {
        GUI.Label(m_fps, "FPS: " + fps, m_style);
        GUI.Label(frame_count, "FrameCount: " + frameCount, m_style);
    }
}
