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
    private int m_frames = 0;
    private float m_lastUpdateShowTime = 0f;
    private readonly float m_updateTime = 0.05f;

    private float m_frameDeltaTime = 0;
    private float m_FPS = 0;
    private Rect m_fps, m_dtime, m_fps_;
    private GUIStyle m_style = new GUIStyle();

    private void Start()
    {
        SentryUnity.Init(o =>
        {
            o.Dsn = "https://449798ce6def47ceba65af22e57d30c9@o1039766.ingest.sentry.io/6115983"; //sentry.io
            // o.Dsn = "http://0c853a6b79b14c30a9ab2b56fbc79449@127.0.0.1:9000/9"; //local self-hosted
            // o.Dsn = "http://888fe57eba274af794fe857cccdbb379@jssz-ai-newton-cpu-03:9000/5"; //server self-hosted
            o.Debug = true;
            // o.EnableLogDebouncing = true;
            o.Environment = "production";
            o.TracesSampleRate = 1.0;
            // o.MaxBreadcrumbs = 10; //控制应该捕获breadcrumbs的数量,默认100
        });

        m_lastUpdateShowTime = Time.realtimeSinceStartup;
        m_fps = new Rect(0, 0, 100, 40);
        m_dtime = new Rect(0, 40, 100, 40);
        m_fps_ = new Rect(0, 80, 100, 40);
        m_style.fontSize = 30;
        m_style.normal.textColor = Color.red;
    }
    void Update()
    {
        frameCount++;
        var transaction = SentrySdk.StartTransaction(
            "SentryMonitorFrequence", //transaction_name
            "CollectInfo" //operation_name
        );
        Thread.Sleep(10);
        // var span = transaction.StartChild(
        //     "Update"//operation_name
        // );

        // var span1 = span.StartChild("GetAllObjectsInfo");
        // span1.Finish();

        // var span2 = span.StartChild("GetRuntimeInfo");
        // span2.Finish();

        // span.Finish();
        transaction.Finish();

        m_frames++;
        if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
        {
            m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
            m_frameDeltaTime = (Time.realtimeSinceStartup - m_lastUpdateShowTime) / m_frames;
            // m_FPS_ = 1 / Time.unscaledDeltaTime;
            m_frames = 0;
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
        }
        Debug.Log($"Current Frame Count: {frameCount}");
    }
    void OnGUI()
    {
        GUI.Label(m_fps, "FPS: " + m_FPS, m_style);
        GUI.Label(m_dtime, "间隔: " + m_frameDeltaTime, m_style);
        GUI.Label(m_fps_, "FrameCount: " + frameCount, m_style);
        // GUI.TextArea(new Rect(10, 30, 250, 20), statsText);
    }
}