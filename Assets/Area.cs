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

	[SerializeField]
	protected UILabel timesLabel;
	protected UILabel timesStrokeLabel;

	protected float maxSeconds = 10.0f;

	protected float tmpSeconds;
	

	public bool canMove{
		get{ return _canMove;}
		set{ _canMove = value;if(value){
				Debug.Log ("canmove true");
				flySeconds = 0.0f;
			}}
	}
	private bool _canMove = true;

	[SerializeField]
	private UITexture timesUpTexture;

	[SerializeField]
	private AudioSource startWhistle;

	private float startCountDownSeconds;

	[SerializeField]
	private AudioSource lostSound;

	[SerializeField]
	private UILabel startCountDownLabel;
	private bool notStart = true;

	[SerializeField]
	public UITexture winYep;

	[SerializeField]
	private AudioSource winSound;
	[SerializeField]
	private AudioSource bgSound;

	private bool isWin = false;

	private float flySeconds = 0.0f;

	private bool movingCamera = false;
	private Vector3 sphereFromPos;
	private Vector3 sphereToPos;
	private float movingCameraTime;
	// Use this for initialization
	protected void Start () {
		Gesture.onDraggingE += OnDragging;
		Gesture.onDraggingEndE += OnDraggingEnd;
		Gesture.onTouchDownE += OnTouchDown;
		Gesture.onMouse1DownE += OnTouchDown;
		tmpSeconds = maxSeconds;

	}

	protected void OnDestroy(){
		Gesture.onDraggingE -= OnDragging;
		Gesture.onDraggingEndE -= OnDraggingEnd;
		Gesture.onTouchDownE -= OnTouchDown;
		Gesture.onMouse1DownE -= OnTouchDown; 
	}
	// Update is called once per frame
	void Update () {
		if (startCountDownSeconds > 0) {
			return;
		} else {
			if(notStart){
				startWhistle.Play();
				notStart = false;
				startCountDownLabel.gameObject.SetActive(false);
				canMove = true;
				countDown ();
			}
		}

		if (!movingCamera) {
			camera.transform.position = getCameraPos (sphere.transform.position);
	
		}

		if ((!canMove && flySeconds > 0.5f && !sphere.GetComponent<Sphere>().inTube) || sphere.GetComponent<Sphere>().crash) {
//			Debug.Log ("inTube "+ sphere.GetComponent<Sphere>().inTube + "flySeconds= "+ flySeconds + " crash= "+sphere.GetComponent<Sphere>().crash);
			sphere.rigidbody.velocity = Vector3.zero;
			sphere.rigidbody.angularVelocity = Vector3.zero;
//			sphere.rigidbody.Sleep();
		
			if(!movingCamera){
				movingCameraTime = 0;
				movingCamera = true;
				sphereFromPos = sphere.transform.position;
				sphereToPos = sphere.GetComponent<Sphere>().jumpPos;
				sphere.transform.position = sphereToPos;
				sphere.SetActive(false);
				Debug.Log("movingCamera false sphereFromPos=" + sphereFromPos + "sphereToPos=" + sphereToPos);
			}else{
				movingCameraTime += Time.deltaTime;
				camera.transform.position = getCameraPos(Vector3.Lerp (sphereFromPos, sphereToPos, movingCameraTime));
				Debug.Log("movingCamera true camera pos=" + camera.transform.position );
				if(camera.transform.position == getCameraPos (sphereToPos)){
					sphere.SetActive(true);
					Debug.Log("movingCamera true camera pos=====");
					movingCamera = false;
					canMove = true;
					sphere.GetComponent<Sphere>().crash = false;

				}
			}
		}


		//camera moveTo
		/*
		if (Mathf.Abs( sphere.transform.position.z ) > 10 && camera.transform.position.z != -11 + Mathf.Round (sphere.transform.position.z)) {
			iTween.MoveTo (camera.gameObject, iTween.Hash (
			"z", -11 + Mathf.Round (sphere.transform.position.z),
			"time", 0.1f, "islocal", true, "easetype", iTween.EaseType.linear));
		}
		*/
		if (!isWin) {
			timesLabel.text = tmpSeconds.ToString ("F1");
			if (tmpSeconds < 10) {
				timesLabel.color = new Color (255, 0, 0);
			}
//		Debug.Log ("this update tmpSeconds="+this+tmpSeconds);
			if (tmpSeconds <= 0) {
				canMove = false;
				sphere.rigidbody.Sleep ();
				//times up
				if (timesUpTexture.transform.position.x < 0) {
					lostSound.Play ();
					timesUpTexture.transform.position = new Vector3 (Mathf.Lerp (timesUpTexture.transform.position.x, 0, Time.time / 10), 0, 0);
				}
			}
		}
	}

	void OnTouchDown(Vector2 pos){
//		Debug.Log ("tmpSeconds="+tmpSeconds);
		if (tmpSeconds <= 0 || isWin) {
			Application.LoadLevel ("menu");
		}
	}

	void OnDragging(DragInfo dragInfo){
		//Debug.Log (dragInfo.pos);
		if (!canMove) {
			return;
		}
		if (lastPos == Vector2.zero) {
			lastPos = dragInfo.pos;
		} else {
			Vector2 curPos = dragInfo.pos;
			Vector2 tmp = curPos - lastPos;
			// add torgue cannot clime up,whether set dynamic friction bigger
			/*
			Vector3 force = new Vector3(tmp.y, 0, -tmp.x);
			force.Normalize();
			force *= 50;
//			sphere.rigidbody.velocity = sphere.rigidbody.velocity * 0.1f;
			sphere.rigidbody.AddTorque(force);
*/
			// add force
			Vector3 force = new Vector3(tmp.x, 0, tmp.y);
			force.Normalize();
			force *= 30 * sphere.rigidbody.mass;
			sphere.rigidbody.velocity = sphere.rigidbody.velocity * 0.5f;
			Vector3 pos =  sphere.transform.position + new Vector3(0.0f, sphere.transform.localScale.x * sphere.GetComponent<SphereCollider>().radius * 1.8f, 0.0f);
			sphere.rigidbody.AddForceAtPosition(force, pos);
			/*
			//mod av
			int times = 150;
			// mod angularVelocity
			Vector3 av = new Vector3(tmp.y, 0, -tmp.x);
			av.Normalize();
			av *= times;

//			Vector3 lastAv = sphere.rigidbody.angularVelocity;
			sphere.rigidbody.angularVelocity = av;
//			Debug.Log("av="+av);

			//mod velocity
			Vector3 yVelocity = new Vector3(0,sphere.rigidbody.velocity.y,0);
			Vector3 v = new Vector3(tmp.x, 0, tmp.y);
			v.Normalize ();
			v *= times / 2;
			sphere.rigidbody.velocity = v * sphere.GetComponent<SphereCollider>().radius * sphere.transform.localScale.x / 6.28f + yVelocity;
*/
		}
	
	}

	void OnDraggingEnd(DragInfo dragInfo){
		lastPos = Vector2.zero;
//		Debug.Log("Drag end");
	}

	void updateCanMove(bool value){
		if (notStart) {// not start yet
			return;
		}
		Debug.Log ("updateCanMove =" + value);
		canMove = value;
	}

	protected void Awake(){
		canMove = false;
		startCountDownSeconds = 3.0f;
		timesLabel.text = maxSeconds.ToString ("F1");
		sphereStartPos = sphere.transform.position;
		cameraStartPos = camera.transform.position;
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

	void FixedUpdate(){
		if (startCountDownSeconds > 0) {//before start
			startCountDownSeconds -= Time.fixedDeltaTime;
			startCountDownLabel.text = startCountDownSeconds.ToString ("F0");
		} else {//after start
			if (!canMove && !sphere.GetComponent<Sphere>().inTube && !isWin) {
				flySeconds += Time.fixedDeltaTime;
			}
		}
	}

	public void win(){
		bgSound.Stop ();
		winSound.Play ();
		isWin = true;
		winYep.GetComponent<TweenPosition> ().enabled = true;
	}

	Vector3 getCameraPos(Vector3 spherePos){
		Vector3 tmp;
		tmp.x = (spherePos.x - sphereStartPos.x) * 0.8f;
		tmp.y = spherePos.y - sphereStartPos.y;
		tmp.z = spherePos.z;
		return cameraStartPos + tmp;
	}

}
