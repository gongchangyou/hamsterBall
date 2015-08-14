using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Threading;
public static class TimeSpanToolV2
{
	public static TimeSpan AddSeconds(this TimeSpan ts, int seconds)
	{
		return ts.Add(new TimeSpan(0, 0, seconds));
	}
	public static TimeSpan AddMinutes(this TimeSpan ts, int minutes)
	{
		return ts.Add(new TimeSpan(0, minutes, 0));
	}
	public static TimeSpan AddHours(this TimeSpan ts, int hours)
	{
		return ts.Add(new TimeSpan(hours, 0, 0));
	}
	public static TimeSpan AddMilliSeconds(this TimeSpan ts, int milliSeconds)
	{
		return ts.Add(new TimeSpan(ts.Days, 0, 0, 0, milliSeconds));
	}
}

public class Area : MonoBehaviour {
	[SerializeField]
	protected GameObject sphere;
	[SerializeField]
	protected Camera camera;
	Vector2 lastPos = Vector2.zero;

	protected Vector3 sphereStartPos;
	protected Vector3 cameraStartPos;
	protected float endY = -20;

	[SerializeField]
	protected Text text;
	
	protected float maxSeconds = 10.0f;

	private float tmpSeconds;

	public bool canMove{
		get{ return _canMove;}
		set{ _canMove = value;}
	}
	private bool _canMove = true;

	// Use this for initialization
	protected void Start () {
		Gesture.onDraggingE += OnDragging;
		Gesture.onDraggingEndE += OnDraggingEnd;
		tmpSeconds = maxSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		if (sphere.transform.position.y < endY || sphere.GetComponent<Sphere>().crash) {
			_canMove = true;
			sphere.transform.position = sphere.GetComponent<Sphere>().jumpPos;
			sphere.rigidbody.velocity = Vector3.zero;
			sphere.rigidbody.angularVelocity = Vector3.zero;
//			sphere.rigidbody.Sleep();
			sphere.GetComponent<Sphere>().crash = false;
		}

		Vector3 tmp = Vector3.zero;
		tmp.x = (sphere.transform.position.x - sphereStartPos.x) * 0.8f;
		tmp.y = sphere.transform.position.y - sphereStartPos.y;
		tmp.z = sphere.transform.position.z;
		camera.transform.position = cameraStartPos + tmp;
		//camera moveTo
		/*
		if (Mathf.Abs( sphere.transform.position.z ) > 10 && camera.transform.position.z != -11 + Mathf.Round (sphere.transform.position.z)) {
			iTween.MoveTo (camera.gameObject, iTween.Hash (
			"z", -11 + Mathf.Round (sphere.transform.position.z),
			"time", 0.1f, "islocal", true, "easetype", iTween.EaseType.linear));
		}
		*/

		text.text = tmpSeconds.ToString ("F1");
		if (tmpSeconds <= 0) {
			//times up

		}
	}
	
	void OnDragging(DragInfo dragInfo){
		//Debug.Log (dragInfo.pos);
		if (!_canMove) {
			return;
		}
		if (lastPos == Vector2.zero) {
			lastPos = dragInfo.pos;
		} else {
			Vector2 curPos = dragInfo.pos;
			Vector2 tmp = curPos - lastPos;
			/* add force
			Vector3 force = new Vector3(tmp.x, 0, tmp.y);
			force.Normalize();
			force *= 50;
			sphere.rigidbody.velocity = sphere.rigidbody.velocity * 0.1f;
			sphere.rigidbody.AddForce(force);
			*/

			int times = 100;
			// mod angularVelocity
			Vector3 av = new Vector3(tmp.y, 0, -tmp.x);
			av.Normalize();
			av *= times;
//			Vector3 lastAv = sphere.rigidbody.angularVelocity;
			sphere.rigidbody.angularVelocity = av;
//			Debug.Log("av="+av);

			//mod velocity
			Vector3 v = new Vector3(tmp.x, 0, tmp.y);
			v.Normalize ();
			v *= times / 2;
			sphere.rigidbody.velocity = v * sphere.GetComponent<SphereCollider>().radius * sphere.transform.localScale.x / 6.28f;

		}
	
	}

	void OnDraggingEnd(DragInfo dragInfo){
		lastPos = Vector2.zero;
//		Debug.Log("Drag end");
	}

	void updateCanMove(bool canMove){
		_canMove = canMove;
	}

	protected void Awake(){
		countDown ();
	}

	void countDown(){
		try
		{
			DateTime _timeEnd = DateTime.Now.AddSeconds(maxSeconds);
			ThreadPool.QueueUserWorkItem((arg) =>
			                             {
				TimeSpan _ts = _timeEnd - DateTime.Now;
				while (true)
				{
					Thread.Sleep(100);
					if (_ts.TotalSeconds >= 0)
					{
//						Debug.Log("totalSeconds="+_ts.TotalSeconds);

//						Console.WriteLine("还剩余{0}分钟{1}秒", _ts.Minutes, _ts.Seconds);
						_ts = _ts.AddMilliSeconds(-100);
						tmpSeconds = _ts.Minutes * 60 + _ts.Seconds + _ts.Milliseconds / 1000.0f;
					}
				}
			});
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		finally
		{
			Console.ReadLine();
		}
	}

}
