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
    void Update()
    {
        frameCount++;

        string operationName = "CollectInfo" + frameCount.ToString();

        var transaction = SentrySdk.StartTransaction(
            "UpdateFunc", //transaction_name
            operationName //operation_name
        );
        var span = transaction.StartChild(
            "Updata"//operation_name
        );

        var span1 = span.StartChild("GetAllObjectsInfo");
        Thread.Sleep(20);
        span1.Finish();

        var span2 = span.StartChild("GetRuntimeInfo");
        Thread.Sleep(30);
        span2.Finish();

        span.Finish();
        transaction.Finish();
    }
}