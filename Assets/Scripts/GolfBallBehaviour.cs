using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GolfBallBehaviour : MonoBehaviour, IBeginDragHandler ,IDragHandler, IEndDragHandler
{

	public LineRenderer line;
	public float maxLength = 200;
	public float minLength = -200;
	public float force = 100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isDragStart)
		{
			DrawLine();
			CalculateTrajectory ();
		}
	}

	Vector2 startPos = Vector2.zero;
	bool isDragStart = false;
	public void OnBeginDrag (PointerEventData data)
	{
		//Debug.Log ("starting position: " + data.position);
		startPos = data.position;
		isDragStart = true;
	}

	Vector2 endPos = Vector3.zero;
	public void OnDrag (PointerEventData data)
	{
		//Debug.Log ("end drag position" + data.position);
		endPos = data.position;

	}

	public void OnEndDrag (PointerEventData data)
	{
		isDragStart = false;
		line.SetPosition (0, Vector3.zero);
		line.SetPosition (1, Vector3.zero);
		//var calcForce = Vector2.Distance (startPos, endPos);
		trajectory.x = Mathf.Clamp (trajectory.x, minLength, maxLength);
		trajectory.y = Mathf.Clamp (trajectory.y, minLength, maxLength);
		trajectory = trajectory.normalized;
		Vector2 finalForce = trajectory;
		finalForce.x = Mathf.Lerp (-force, force, trajectory.x);
		finalForce.y = Mathf.Lerp (-force, force, trajectory.y);
		Debug.Log ("Trajectory: " + trajectory.x + ", " + trajectory.y);
		Debug.Log (finalForce.x + ", " + finalForce.y);
		rigidbody2D.AddForce (finalForce, ForceMode2D.Impulse);
		//Debug.Log ("end drag");
	}

	float currForce = 0f;
	float angle = 0f;
	Vector2 trajectory;
	private void CalculateTrajectory()
	{
		//float distance = Vector2.Distance (startPos, endPos);
		///Debug.Log ("Distance: " + distance);
		//currForce = Mathf.Clamp (distance, 0, maxForce);
		//float angle = Mathf.Atan2(abVector.y, abVector.x);
		//Debug.Log ("Angle: " + angle * Mathf.Rad2Deg);
		trajectory = endPos - startPos;
	}

	private void DrawLine()
	{
		Vector3 start = startPos;
		start = Camera.main.ScreenToWorldPoint (start);
		start.z = 0f;
		Vector3 end = endPos;
		end = Camera.main.ScreenToWorldPoint (end);
		end.z = 0f;
		if (Vector2.Distance(startPos, endPos) <= maxLength)
		{
			line.SetPosition (0, start);
			line.SetPosition (1, end);
		}


		//Debug.DrawLine (startPos, endPos, Color.red, 1.0f);
	}
}
