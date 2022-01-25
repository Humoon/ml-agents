using UnityEngine;
using Unity.Profiling;
using Crawler.Tools.Patterns;


namespace Unity.MLAgentsExamples
{
    public class PerformanceMonitor : MonoBehaviourSingleton<PerformanceMonitor>
    {
        // Start is called before the first frame update
        ProfilerRecorder totalUsedMemoryRecorder;
        // ProfilerRecorder gcMemoryRecorder;
        ProfilerRecorder gameObjectCountRecorder;
        ProfilerRecorder verticesCountRecorder;
        ProfilerRecorder trianglesCountRecorder;

        float lastFrameCount = 0;
        float lastTimeCount = 0;
        float PFS;

        void Start()
        {
            lastFrameCount = Time.frameCount;
            lastTimeCount = Time.realtimeSinceStartup;
        }


        void OnEnable()
        {
            // Momory Profiler
            totalUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            // gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");

            // Render Profiler
            verticesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
            trianglesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        }

        /// <summary>
        /// Dispose all Recorder
        /// </summary>
        void OnDisable()
        {
            totalUsedMemoryRecorder.Dispose();
            // gcMemoryRecorder.Dispose();
            gameObjectCountRecorder.Dispose();

            verticesCountRecorder.Dispose();
            trianglesCountRecorder.Dispose();
        }

        public float GetTimeScale()
        {
            return Time.timeScale;
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public float GetUnScaledDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }

        public float GetTime()
        {
            return Time.time;
        }

        public float GetUnScaledTime()
        {
            return Time.unscaledTime;
        }

        public float GetFixedTime()
        {
            return Time.fixedTime;
        }

        public float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        public float GetCaptureFrameRate()
        {
            return Time.captureFramerate;
        }

        /// <summary>
        /// Get Current FPS
        /// </summary>
        public float GetFPS()
        {
            return 1 / Time.deltaTime;
        }

        /// <summary>
        ///	Total value of memory that Unity uses and tracks
        /// </summary>
        public float GetTotalUsedMemory()
        {
            return totalUsedMemoryRecorder.LastValue / (1024 * 1024);
        }

        /// <summary>
        /// The number of vertices Unity processed during a frame
        /// </summary>
        public float GetVerticesCount()
        {
            return verticesCountRecorder.LastValue;
        }

        /// <summary>
        /// The number of triangles Unity processed during a frame
        /// </summary>
        public float GetTrianglesCount()
        {
            return trianglesCountRecorder.LastValue;

        }

        /// <summary>
        ///  The total number of GameObject instances in the scene
        /// </summary>
        public float GetgameObjectCount()
        {
            return gameObjectCountRecorder.LastValue;
        }

        public void PrintTimeScale()
        {
            Debug.Log($"Engine Configuration Channel set TimeScale = {Time.timeScale}");
            Time.timeScale = Time.timeScale;
            Debug.Log($"DeltaTime = {Time.deltaTime} | UnScaledDeltaTime = {Time.unscaledDeltaTime}");
        }

        public float ComputeFPS()
        {
            float framePass = Time.frameCount - lastFrameCount;
            float timePass = Time.realtimeSinceStartup - lastTimeCount;
            var m_fps = framePass / timePass;

            lastFrameCount = Time.frameCount;
            lastTimeCount = Time.realtimeSinceStartup;

            Debug.Log($"FramePass: {framePass}, and TimePass: {timePass}, and FPS: {m_fps}");

            return m_fps;
        }
    }
}
