#region License

#endregion

using System.Collections;
using UnityEngine;
using SocketIO;

public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;

	private Animator animator;

	public float originalMoveSpeed = 10f;
	public float moveSpeed = 10f;

	public float maxDiveSpeed = 25f;

	public float turnSpeed = 8f;
	public float upSpeed = 2f;
	public float bankSpeed = 20f;
	public float evenSpeed = 20f;

	public float rollSpeed = 50f;

	public int flyingAnimation = 0;
	public int rightAnimation = 1;
	public int leftAnimation = 2;
	public int upAnimation = 3;
	public int downAnimation = 4;
	public int rollRightAnimation = 5;
	public int rollLeftAnimation = 6;
	public int evenDownAnimation = 7;
	public int evenUpAnimation = 8;
	public int evenLeftAnimation = 9;
	public int evenRightAnimation = 10;

	private Vector3 euler; 

	private bool upCoroutineStarted = false;
	private bool downCoroutineStarted = false;
	private bool leftCoroutineStarted = false;
	private bool rightCoroutineStarted = false;
	private bool rollCoroutineStarted = false;

	public bool isFlying = false; //change this to false when cardboard is active

	AudioSource flapping;

	public void Start() 
	{
		euler = transform.eulerAngles;
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		animator = GetComponent<Animator> ();
		flapping = GetComponent<AudioSource> ();


		////////// MOVEMENT USING EULER AND COROUTINES  //////////
		socket.On ("left", MoveLeftEuler);
		socket.On ("right", MoveRightEuler);
		socket.On ("up", MoveUpEuler);
		socket.On ("down", MoveDownEuler);
		socket.On ("evenup", EvenUpEuler);
		socket.On ("evendown", EvenDownEuler);
		socket.On ("evenleft", EvenLeftEuler);
		socket.On ("evenright", EvenRightEuler);
		socket.On ("roll", RollEuler);
		socket.On ("reset", ResetGame);
		socket.On ("move", MoveDragon);

	}



	// LEFTTTTT
	public void MoveLeftEuler(SocketIOEvent e){
		
		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("LEFT");

			//Trigger Left Animation
			animator.SetInteger("DragonState", leftAnimation);

			StartCoroutine ("LeftEuler");
		} else {
			Debug.Log ("ROLL IN PROGRESS - NO LEFT");
		}
	}
	
	private IEnumerator LeftEuler(){
		flapping.mute = true;
		//if left button is pressed while dragon is evening out, cancel the evening out and turn more left
		if (leftCoroutineStarted == true) {
			Debug.Log ("CANCEL LEFT EVENING");
			leftCoroutineStarted = false;
			StopCoroutine("EvenLeftEulerCoroutine");
		}
			
		float currentTime = Time.time;
		while (Time.time <= currentTime + 1f) {
			//turning angle
			euler.y = (euler.y - turnSpeed * Time.deltaTime);
			//banking angle
			if(euler.z < 40){
				euler.z = (euler.z + bankSpeed * Time.deltaTime);
			}
			transform.eulerAngles = euler;
			yield return null;
		}
	}

	public void EvenLeftEuler(SocketIOEvent e){
		
		// if roll in progress block other movements

		if (rollCoroutineStarted == false){
			Debug.Log ("EVEN LEFT");

			animator.SetInteger ("DragonState", evenLeftAnimation);

			StartCoroutine ("EvenLeftEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN LEFT");
		}
	}

	private IEnumerator EvenLeftEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN LEFT");
		leftCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.z > 0) {
			euler.z = (euler.z - bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
		transform.eulerAngles = euler;
		leftCoroutineStarted = false;

		//Trigger Default Flying Animation
		animator.SetInteger("DragonState", flyingAnimation);
		flapping.mute = false;
	}

	/// RIGHTTTT
	public void MoveRightEuler(SocketIOEvent e){
		


		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("RIGHT");

			//Trigger Right Animation
			animator.SetInteger("DragonState", rightAnimation);

			StartCoroutine ("RightEuler");
		} else {
			Debug.Log ("ROLL IN PROGRESS - NO RIGHT");
		}
	}
	
	private IEnumerator RightEuler(){
		flapping.mute = true;
		if (rightCoroutineStarted == true) {
			Debug.Log ("CANCEL RIGHT EVENING");
			rightCoroutineStarted = false;
			StopCoroutine("EvenRightEulerCoroutine");
		}
			
		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			euler.y = (euler.y + turnSpeed * Time.deltaTime);
			if(euler.z > -40){
				euler.z = (euler.z - bankSpeed * Time.deltaTime);
			}
			transform.eulerAngles = euler;
			yield return null;
		}
			
	}

	public void EvenRightEuler(SocketIOEvent e){
		
		// if roll in progress block other movements
		if (rollCoroutineStarted == false){
			Debug.Log ("EVEN RIGHT");
			animator.SetInteger ("DragonState", evenRightAnimation);
			StartCoroutine ("EvenRightEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN RIGHT");
		}
	}

	private IEnumerator EvenRightEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN RIGHT");

		rightCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.z < 0) {
			euler.z = (euler.z + bankSpeed * Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}
			
		transform.eulerAngles = euler;
		rightCoroutineStarted = false;

		animator.SetInteger("DragonState", flyingAnimation);
		flapping.mute = false;
	}

	//// UPPPPPP
	public void MoveUpEuler(SocketIOEvent e){

		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("UP");
			animator.SetInteger ("DragonState", upAnimation);
			StartCoroutine ("UpEuler");
		} else {
			Debug.Log ("ROLL IN PROGRESS - NO UP");
		}
	}
	
	private IEnumerator UpEuler(){
		flapping.mute = true;
		// if the dragon is leveling out and asked to rise again, cancel the leveling out
		if (upCoroutineStarted == true) {
			Debug.Log ("CANCEL UP COROUTINE");
			upCoroutineStarted = false;
			StopCoroutine("EvenUpEulerCoroutine");
		}


		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x > -70){
				euler.x = (euler.x-upSpeed*Time.deltaTime);
				transform.eulerAngles = euler;
			}
			yield return null;
		}

	}

	public void EvenUpEuler(SocketIOEvent e){
		
		// if roll in progress block other movements
		if (rollCoroutineStarted == false){// && downCoroutineStarted == false) {
			Debug.Log ("EVEN UP");

			StartCoroutine ("EvenUpEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN UP");
		}
	}

	
	private IEnumerator EvenUpEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN UP");
		animator.SetInteger ("DragonState", evenUpAnimation);
		//change bool so we know if to cancel it
		upCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.x < 0) {
			
			euler.x = (euler.x + evenSpeed*Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}

		transform.eulerAngles = euler;
		//set bool to false once over
		upCoroutineStarted = false;

		animator.SetInteger ("DragonState", flyingAnimation);
		flapping.mute = false;
	}

	///// DOWNNNN
	public void MoveDownEuler(SocketIOEvent e){

		// if roll in progress block other movements
		if (rollCoroutineStarted == false) {
			Debug.Log ("DOWN");

			animator.SetInteger ("DragonState", downAnimation);

			StartCoroutine ("DownEuler");
		} else {
			Debug.Log ("ROLL IN PROGRESS - NO DOWN");
		}
	}
	
	private IEnumerator DownEuler(){
		flapping.mute = true;
		if (downCoroutineStarted == true) {
			Debug.Log ("DOWN COROUTINE CANCELLED");
			downCoroutineStarted = false;
			StopCoroutine("EvenDownEulerCoroutine");
		}


		float currentTime = Time.time;
		
		while (Time.time <= currentTime + 1f) {
			if(euler.x < 70){
				if(moveSpeed < maxDiveSpeed){
					moveSpeed += 0.08f;
				}

				euler.x = (euler.x+turnSpeed*Time.deltaTime);
				transform.eulerAngles = euler;
			}
			yield return null;
		}

	}

	public void EvenDownEuler(SocketIOEvent e){

		// if roll in progress block other movements
		if (rollCoroutineStarted == false){
			Debug.Log ("EVEN DOWN");

			StartCoroutine ("EvenDownEulerCoroutine");
		} else {
			Debug.Log ("NO EVEN DOWN");
		}

	}

	private IEnumerator EvenDownEulerCoroutine(){
		Debug.Log ("COROUTINE - EVEN DOWN");
		animator.SetInteger ("DragonState", evenDownAnimation);
		downCoroutineStarted = true;
		yield return new WaitForSeconds (0.2f);
		while (euler.x > 0) {
			if(moveSpeed>originalMoveSpeed){
				moveSpeed -= 0.1f;
			}

			euler.x = (euler.x - evenSpeed*Time.deltaTime);
			transform.eulerAngles = euler;
			yield return null;
		}


		transform.eulerAngles = euler;
		moveSpeed = originalMoveSpeed;

		downCoroutineStarted = false;

		animator.SetInteger ("DragonState", flyingAnimation);
		flapping.mute = false;
	}

	public void RollEuler(SocketIOEvent e){

		//only let second barrel roll start once one roll is over

		if (rollCoroutineStarted == false) {
			Debug.Log ("ROLL");
			StartCoroutine ("RollEulerCoroutine");
		} else {
			Debug.Log ("NO ROLL");
		}
	}

	public void ResetGame(SocketIOEvent e){
		Application.LoadLevel (0);
	}
		
	private IEnumerator RollEulerCoroutine(){
		Debug.Log ("COROUTINE - ROLL");
		flapping.mute = true;

		// can check for and cancel even up or even down coroutine if you want here

		rollCoroutineStarted = true;

		// if turning left or moving straight do barrel roll to the left
		if (euler.z > 0 || euler.z == 0) {
			animator.SetInteger ("DragonState", rollLeftAnimation);
			while (euler.z <= 360f) {
				euler.z = (euler.z + rollSpeed * Time.deltaTime);

				transform.eulerAngles = euler;
				yield return null;
			}
		} else if (euler.z < 0) {  // if turning right z will be less than 0 and do barrel roll to theright
			animator.SetInteger ("DragonState", rollRightAnimation);
			while (euler.z >= -360f) {
				euler.z = (euler.z - rollSpeed * Time.deltaTime);

				transform.eulerAngles = euler;
				yield return null;
			}
		}
		euler.z = 0;
		transform.eulerAngles = euler;

		// if dragon was rising or diving while rolling, when roll stops reorient to x = 0
		if (euler.x > 0) {
			Debug.Log ("EVEN DOWN MAN");
			StartCoroutine ("EvenDownEulerCoroutine");

		} else if (euler.x < 0) {
			Debug.Log ("EVEN UP MAN");
			StartCoroutine ("EvenUpEulerCoroutine");
		} else {
			animator.SetInteger ("DragonState", flyingAnimation);
		}

		rollCoroutineStarted = false;
		flapping.mute = false;
	}

	public void MoveDragon(SocketIOEvent e){
		isFlying = !isFlying;
	}
	
	//// END OF USING EULER ANGLES ////

	public void Update(){
		// start/stop flying on button click

		// done this so you can project another comp on a big screen
		if (Input.GetMouseButtonDown (0)) {
			//isFlying = !isFlying;
			socket.Emit ("fly");
		}

		if (isFlying) {
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}
			

	}
	

}
