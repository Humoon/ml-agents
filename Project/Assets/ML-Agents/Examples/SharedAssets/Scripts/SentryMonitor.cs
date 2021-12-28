using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Sentry;
using Sentry.Unity;

public class SentryMonitor : MonoBehaviour
{
    private int _counter = 0;
    private int m_TotalVerts;
    private int m_TotalTriangles;
    private float m_FPS;
    private long m_AllocatedMemory;
    private long m_AllMemory;

    private float f_UpdateInterval = 0.5F;
    private float f_LastInterval;

    // private GameObject testObject = null;


    void Start()
    {
        SentryUnity.Init(o =>
        {
            o.Dsn = "https://449798ce6def47ceba65af22e57d30c9@o1039766.ingest.sentry.io/6115983";
            o.EnableLogDebouncing = true;
            o.Debug = true;
            o.Environment = "production";
            o.SampleRate = 1;
        });

        // The time between debug calls of the same type is less than 1s
        // Every following call of the same type gets ignored until 1s has passed

        Debug.Log("Log");              // recorded
        Debug.Log("Log 2");            // not recorded
        Debug.LogWarning("Warning");   // recorded
        Debug.LogWarning("Warning 2"); // not recorded
        Debug.LogError("Error");       // recorded
        Debug.LogError("Error 2");     // not recorded

        f_LastInterval = Time.realtimeSinceStartup;

        // This will throw a Null Reference Exception
        // testObject.GetComponent<Transform>();   // Captured error
    }

    void GetAllObjectsInfo()
    {
        m_TotalVerts = 0;
        m_TotalTriangles = 0;
        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in ob)
        {
            GetAllVertsAndTris(obj);
        }
    }

    void GetRuntimeInfo()
    {
        m_FPS = 1 / Time.unscaledDeltaTime;
        m_AllocatedMemory = Profiler.GetMonoUsedSizeLong() / (1024 * 1024);
        m_AllMemory = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);

        // Object[] textures = Resources.FindObjectsOfTypeAll(typeof(Texture));
        // foreach (Texture t in textures)
        // {
        //     Debug.Log("Texture object " + t.name + " using: " + Profiler.GetRuntimeMemorySizeLong((Texture)t) + "Bytes");
        // }
    }

    void GetAllVertsAndTris(GameObject obj)
    {
        Component[] filters;
        filters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter f in filters)
        {
            m_TotalTriangles += f.sharedMesh.triangles.Length / 3;
            m_TotalVerts += f.sharedMesh.vertexCount;
        }
    }

    void OnGUI()
    {
        string fpsplay = m_FPS.ToString("FPS: #,##0");
        GUILayout.Label(fpsplay);

        string allocatedMemoryplay = m_AllocatedMemory.ToString("Allocated Memory: #,##0 MB");
        GUILayout.Label(allocatedMemoryplay);
        string allMemoryplay = m_AllMemory.ToString("All Memory: #,##0 MB");
        GUILayout.Label(allMemoryplay);

        string vertsdisplay = m_TotalVerts.ToString("Total Verts: #,##0");
        GUILayout.Label(vertsdisplay);
        string trisdisplay = m_TotalTriangles.ToString("Total Triangles: #,##0");
        GUILayout.Label(trisdisplay);
    }

    void Update()
    {
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_LastInterval = Time.realtimeSinceStartup;
            GetAllObjectsInfo();
            GetRuntimeInfo();
        }
        _counter++;
        // if (_counter % 100 == 0)
        // {
        //     // SentrySdk.AddBreadcrumb("Frame number: " + _counter + "| FPS: " + m_FPS + "| Verts: " + m_TotalVerts + "| Triangles: " + m_TotalTriangles + "| UsedMemory: " + m_AllocatedMemory + " MB");
        //     Debug.Log("Captured Log");              // Breadcrumb
        //     Debug.LogWarning("Captured Warning");   // Breadcrumb
        //     Debug.LogError("Captured Error");       // Captured Error

        //     // This will throw a Null Reference Exception
        //     // testObject.GetComponent<Transform>();   // Captured error
        // }
    }
}