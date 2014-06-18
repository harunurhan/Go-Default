using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private float playerSpeed = 5.3f; //player's speed for x axis.
	private bool isStartButtonClicked;
	private int score = 0; // collected golds
	private int health = 5;
	public GUIText scoreLabel;
	void Start () {
		Time.timeScale=0.0f; // time stopped to wait for start click.
		isStartButtonClicked = false;
	}

	void Update () {
		if(!isInView()) gameOver();
		applyInput();
		updateScore();
		transform.Translate(Vector3.right * playerSpeed * Time.deltaTime,Space.World); // go forward
	}

	/* Updates score label's text, 
	 * using player's x axis position and number of gold collected so far to calculate current score.
	 */
	private void updateScore()
	{
		scoreLabel.text = ((int)(transform.position.x + score)).ToString();
	}

	/* Performs given input.
	 * Different jump force for each input (A,S,D). Low,medium,high.
	 */
	private void applyInput()
	{	
		bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.4f); // check if player touches the ground.
		if(Input.GetKeyDown(KeyCode.A)) jump(isGrounded,150);
		else if(Input.GetKeyDown(KeyCode.S)) jump(isGrounded,260);
		else if(Input.GetKeyDown(KeyCode.D)) jump(isGrounded,370);
	}

	/* Performs jump action
	 * Jumps if player is grounded on Y axis.
	 */
	private void jump(bool isGrounded, float jumpPower)
	{
		if(isGrounded)
		{
			rigidbody.velocity = new Vector3(0,0,0);
			rigidbody.AddForce (new Vector3(0,jumpPower,0), ForceMode.Force);
			jumpPower = 0;
		}
	}

	/* Checks if player is on the screen view.
	 * Actually checks does player fall or not.
	 */
	private bool isInView()
	{
		Vector3 port = Camera.main.WorldToViewportPoint(transform.position);
		if((port.x < 1) && (port.x > 0) && (port.y < 1) && (port.y > 0) && port.z > 0)
			return true;
		else
			return false;
	}

	/* Game return the begining state, simply restarts.
	 */
	private void gameOver()
	{
		Application.LoadLevel (Application.loadedLevelName);
		isStartButtonClicked = false;
	}

	/* Creates, Start Button and listens it.
	 */
	void OnGUI()
	{
		if(!isStartButtonClicked)
		{
			if(GUI.Button(new Rect(Screen.width/2-50,Screen.height/2+35,100,30), "Start"))
			{
				Time.timeScale = 1.0f; //start
				isStartButtonClicked = true;
			}
		}
	}

	/* Occurs if player enters any trigger, more simply if player touches any trigger game object.
	 * Trigger objects are collectables(SpeedUp, Gold, SpeedDown) and Block.
	 * It also performs action for each trigger.
	 */
	void OnTriggerEnter(Collider other)
	{
		other.gameObject.SetActive(false); //disables entered game object, not visible anymore.
		if(other.tag == "Gold")
		{
			score += 30;
		}
		else if(other.tag == "SpeedUp")
		{
			playerSpeed+=1f;
		}
		else if(other.tag == "SpeedDown")
		{
			playerSpeed-=1f;
		}
		else if(other.tag == "Block")
		{
			health--;
			if(health <= 0) gameOver();
		}
	}
}
