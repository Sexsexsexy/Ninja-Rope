using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //Movement vars
	public float runSpeed;
	public float runAcceleration;
	public float jumpForce;
	public float dashForce;
	public float slideForce;
	public float fallBoost;

    //Wall vars
    private int direction;
    private bool wallRunning;
    private bool wallSliding;
    private bool canWallRun;
    private int wallRunTimer;
    private int ledgeTimer;

    public float ledgePullForce;
    public float wallRunForce;
    public float wallSlideForce;
    public int maxWallRunFrames;
    public int maxLedgeGrabFrames;
    public int ledgeCooldown;

    public Vector2 topReach;
    public Vector2 botReach;

	public Transform groundCheck;
	public float groundRadius;
	public LayerMask whatIsGround;

	public bool holdRope;
	public bool onRope;

	public BoxCollider2D topCollider;
	public BoxCollider2D botCollider;

	private RopeHandler ropeHandler;
	private Animator animator;
	private bool grounded;
	private bool sliding;
	private bool jump;
	private bool slide;
	private Vector2 dash;

	void Start()
	{
		ropeHandler = GetComponent<RopeHandler>();
        animator = transform.FindChild("SpriteHandler").GetComponent<Animator>();

        wallRunTimer = 0;
        direction = 1;
        ledgeTimer = maxLedgeGrabFrames;
	}
	
	void Update()
	{
	}

	void FixedUpdate()
	{
		CheckGround();
		CheckWallRun();

        if (jump)
        {
            Jump();
            //jump = false;
        }

		if (grounded) {

            canWallRun = true;
            direction = 1;

            wallRunTimer = 0;

            if (ledgeTimer > maxLedgeGrabFrames)
                ledgeTimer--;

			if (slide) {
				Slide();
				//slide = false;
			}
			if (sliding) {
				if (rigidbody2D.velocity.x < 1)
					sliding = false;
			} else
				Run();
		} else {
			if (dash != Vector2.zero) {
				Dash();
			}
		}

        if (rigidbody2D.velocity.y < 0)
            canWallRun = false;

        if (rigidbody2D.velocity.x > 0)
            ResetDirection();

		animator.SetBool("Swinging", onRope);
		animator.SetBool("Jumping", !grounded);
		animator.SetBool("Sliding", sliding);
        animator.SetBool("WallRunning", wallRunning);
        animator.SetBool("WallSliding", wallSliding);

	}

    void apa(int apaapa)
    {

    }

	private void CheckGround()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
	}

	private void CheckWallRun()
	{
        Vector2 topPoint = new Vector2(transform.position.x + topReach.x * direction, transform.position.y + topReach.y);
        Collider2D topcol = Physics2D.OverlapPoint(topPoint);

        Vector2 botPoint = new Vector2(transform.position.x + botReach.x * direction, transform.position.y + botReach.y);
        Collider2D botcol = Physics2D.OverlapPoint(botPoint);

        bool wasrunning = wallRunning;
        wallRunning = false;
        wallSliding = false;

        if (botcol == null)
        {
            Debug.Log("Bot null");
            return;
        }

        if (topcol == null)
        {
            //Ledgegrab
            int maxvel = 10;
            if (ledgeTimer <= maxLedgeGrabFrames && rigidbody2D.velocity.y < maxvel)
            {
                if (ledgeTimer <= 0)
                    ledgeTimer = ledgeCooldown;

                rigidbody2D.AddForce(new Vector2(ledgePullForce / 4, ledgePullForce), ForceMode2D.Impulse);
            }
        }
        else
        {
            wallRunning = true;

            //Wallrun
            if (rigidbody2D.velocity.y > 0 && canWallRun)
            {
                rigidbody2D.AddForce(new Vector2(0, wallRunForce));
                wallRunTimer++;

                if (wallRunTimer >= maxWallRunFrames)
                {
                    wallRunning = false;
                    canWallRun = false;
                    wallSliding = false;
                }
            }
            else
            {
                wallSliding = true;
                rigidbody2D.AddForce(new Vector2(0, Mathf.Clamp(wallSlideForce - rigidbody2D.velocity.y * 2,0,1) * wallSlideForce));
            }
        }
	}

	private void Run()
	{
		rigidbody2D.AddForce(Mathf.Clamp(runSpeed - rigidbody2D.velocity.x, -1, 1) * runAcceleration * Vector2.right);
	}

	private void Jump()
	{
        if ((wallRunning || wallSliding) && rigidbody2D.velocity.y != 0)
        {
            canWallRun = true;

            direction = -direction;

            transform.FindChild("SpriteHandler").localScale = new Vector3(direction, 1, 1);

            float yfactor = 0.60f;
            float xfactor = 0.70f;

            rigidbody2D.AddForce(jumpForce * new Vector2(xfactor * direction, yfactor), ForceMode2D.Impulse);

            jump = false;
            wallRunTimer = maxWallRunFrames / 2;
           
        }
        else if( grounded)
        {
            rigidbody2D.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            jump = false;
        }
	}

    private void ResetDirection()
    {
        if (rigidbody2D.velocity.x > 0)
            direction = 1;

        transform.FindChild("SpriteHandler").localScale = new Vector3(direction, 1, 1);
    }

	private void Dash()
	{
		rigidbody2D.AddForce(dashForce * dash, ForceMode2D.Impulse);
		dash = Vector2.zero;
		animator.SetTrigger("Dash");
	}

	private void Slide()
	{
		rigidbody2D.AddForce(slideForce * Vector2.right, ForceMode2D.Impulse);
		slide = false;
		sliding = true;
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			//if (onRope) {
			ropeHandler.ReleaseRope();
			//}
		} else if (col.transform.CompareTag("Deadly")) {

		}
	}

	// This section handles the input
	public void Pressed()
	{
		if (grounded || wallRunning) {
			jump = true;
		} else {
			if (!onRope)
				ropeHandler.ShootRope();
			else if (!holdRope)
				ropeHandler.ReleaseRope();
		}
	}

	public void Released()
	{
		if (onRope && holdRope) {
			ropeHandler.ReleaseRope();
		}
	}

	public void Swipe(Vector2 dir)
	{
		if (grounded) {
			slide = true;
		} else {
			if (!onRope)
				dash = dir;
		}
	}

}
