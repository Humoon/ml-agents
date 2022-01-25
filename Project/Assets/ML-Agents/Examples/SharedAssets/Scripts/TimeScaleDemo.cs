using UnityEngine;
using System.Collections;

public class TimeScaleDemo : MonoBehaviour
{
	private bool moveDirection = true;
	private bool scaleDirection = true;
	private Renderer rend;

    float fixedUpdateCount = 0;

	void Start()
	{
		rend = this.GetComponent<Renderer> ();
		StartCoroutine (ChangeColor ());
	}

	void Update ()
	{
		this.transform.position += Vector3.right * 0.01f * (moveDirection == true ? 1.0f : -1.0f);
		if (this.transform.position.x > 4.0f || this.transform.position.x < 0.0f) {
			moveDirection = !moveDirection;
		}

		this.transform.localScale += Vector3.one * Time.deltaTime * (scaleDirection == true ? 1.0f : -1.0f);
		if (this.transform.localScale.x > 2.0f || this.transform.localScale.x < 1.0f) {
			scaleDirection = !scaleDirection;
		}
        Debug.Log($"TimeScale: Update -> TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime} ｜ fixedDeltaTime: {Time.fixedDeltaTime} \n FrameCount: {Time.frameCount} | TimePass: {Time.realtimeSinceStartup} | FixedUpdateCount: {fixedUpdateCount}");

        fixedUpdateCount = 0;
	}

	void FixedUpdate ()
	{
        fixedUpdateCount += 1;

		this.transform.rotation = Quaternion.Euler (Vector3.one * 45.0f * Time.fixedDeltaTime) * this.transform.rotation;

        // Debug.Log($"FixedUpdate -> TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime} | fixedDeltaTime: {Time.fixedDeltaTime} \n FrameCount: {Time.frameCount} | TimePass: {Time.realtimeSinceStartup}");
	}

	IEnumerator ChangeColor()
	{
		while (true) {
			rend.material.color = Random.ColorHSV ();
			yield return 0;
		}
	}

	void OnGUI ()
	{
		GUI.color = Color.white;

		GUIStyle buttonStyle = new GUIStyle (GUI.skin.button);
		buttonStyle.fontSize = 15;

		GUIStyle labelStyle = new GUIStyle (GUI.skin.label);
		labelStyle.fontSize = 15;

		GUILayoutOption[] options = new GUILayoutOption[]{ GUILayout.Width (200), GUILayout.Height (100) };

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("TimeScale = 0", buttonStyle, options) == true) {
			Time.timeScale = 0;
            Debug.Log($"TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime}");
		}
		if (GUILayout.Button ("TimeScale = 1", buttonStyle, options) == true) {
			Time.timeScale = 1;
            Debug.Log($"TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime}");

		}
		if (GUILayout.Button ("TimeScale = 2", buttonStyle, options) == true) {
			Time.timeScale = 2;
            Debug.Log($"TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime}");

		}
		if (GUILayout.Button ("TimeScale = 8", buttonStyle, options) == true) {
			Time.timeScale = 8f;
            Debug.Log($"TimeScale: {Time.timeScale} | DeltaTime: {Time.deltaTime} | unScaledDeltaTime: {Time.unscaledDeltaTime}");

		}
		GUILayout.EndHorizontal ();

		GUILayout.Space (20);
		GUILayout.Label ("Time.timeScale : " + Time.timeScale, labelStyle);
		GUILayout.Space (20);
		GUILayout.Label ("Time.realtimeSinceStartup : " + Time.realtimeSinceStartup, labelStyle);
        GUILayout.Space (20);
		GUILayout.Label ("Time.timeSinceLevelLoad : " + Time.timeSinceLevelLoad, labelStyle);
		GUILayout.Space (10);
        GUILayout.Label ("Time.FrameCount : " + Time.frameCount, labelStyle);
        GUILayout.Space (20);
		GUILayout.Label ("Time.time : " + Time.time, labelStyle);
		GUILayout.Label ("Time.unscaledTime : " + Time.unscaledTime, labelStyle);
		GUILayout.Space (20);
		GUILayout.Label ("Time.deltaTime : " + Time.deltaTime, labelStyle);
		GUILayout.Label ("Time.unscaledDeltaTime : " + Time.unscaledDeltaTime, labelStyle);
		GUILayout.Space (20);
		GUILayout.Label ("Time.fixedTime : " + Time.fixedTime, labelStyle);
		GUILayout.Label ("Time.fixedUnscaledTime : " + Time.fixedUnscaledTime, labelStyle);
		GUILayout.Space (20);
		GUILayout.Label ("Time.fixedDeltaTime : " + Time.fixedDeltaTime, labelStyle);
		GUILayout.Label ("Time.fixedUnscaledDeltaTime : " + Time.fixedUnscaledDeltaTime, labelStyle);
	}
}
