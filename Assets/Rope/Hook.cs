using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hook : MonoBehaviour
{
    public float maxDistance;
    public RopeHandler shooter;
    public Transform player;
    public Vector2 anchorPoint;
    public float offset = -0.05f; //should be nagative!! (it is the length by which the middle of the rope is moved away from object it is bending around
    private bool drawLine;
    private LineRenderer line;
    private List<Vector2> linePoints;
    private bool hooked;
    private LayerMask groundMask;
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start()
    {
        line = transform.parent.gameObject.GetComponentInChildren<LineRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        linePoints = new List<Vector2>();
        groundMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (drawLine)
        {
            UpdateLine();
            if (!hooked && Vector2.Distance(player.position, transform.position) > maxDistance)
            {
                Release();//this may need rethinking since we have a springjoint now...
            }
            DrawLine();
        }
    }

    private void UpdateLine()
    {

        // Remove the player position since it has changed
        linePoints.RemoveAt(linePoints.Count - 1);
        Vector2 playerPoint = player.position;
        Vector2 currentJoint = linePoints[linePoints.Count - 1];

        // if we are not yet hooked we want to move the line and the collider with the hook
        if (!hooked)
        {
            linePoints[0] = transform.TransformPoint(anchorPoint);
        }
        else
        {
            var hit = SliceCast(playerPoint, playerPoint + Time.deltaTime * rigidBody.velocity,
                                currentJoint - 0.1f * (currentJoint - playerPoint), 10000, groundMask);
            // Check if we have a hit
            if (hit.collider != null)
            {
                float dist = -Vector2.Distance(currentJoint, hit.point);        
                linePoints.Add(hit.point);
                shooter.MoveJoint(hit.point, dist);     
            }
            // Otherwise check if we should unwind
            else if (linePoints.Count > 1)
            {
                //new solution
                Vector2 from = playerPoint - currentJoint;
                Vector2 to = linePoints[linePoints.Count - 2] - currentJoint;
                float angle = CheckAngle(from, to);
                //Debug.Log(angle);

                if (angle > 180)
                {
                    Debug.Log("Time to unwind!");
                    Vector2 oldJoint = linePoints[linePoints.Count - 2];
                    float dist = Vector2.Distance(oldJoint, currentJoint);
                    linePoints.RemoveAt(linePoints.Count - 1);
                    shooter.MoveJoint(oldJoint, dist);
                }
            }
        }
        //now we add player position as the end point for the line
        linePoints.Add(playerPoint);
    }

    private RaycastHit2D SliceCast(Vector2 arcStart, Vector2 arcEnd, Vector2 center, int resolution, LayerMask mask)
    {
        Vector2 direction = arcEnd - arcStart;

        RaycastHit2D hit = new RaycastHit2D();
        for (int i = 0; i < resolution; i++)
        {
            hit = Physics2D.Linecast(arcStart + direction/resolution * i , center, mask);
            if (hit.collider != null)
            {
                return hit;
            }
        }
        return hit;
    }

    //Returns the counterclockwise angle between two vectors (from first to second) 
    private float CheckAngle(Vector2 from, Vector2 toVec)
    {
        float ang = Vector2.Angle(from, toVec);
        Vector3 cross = Vector3.Cross(from, toVec);

        if (cross.z < 0)
            ang = 360 - ang;
        return ang;
    }

    // This may need optimization
    private void DrawLine()
    {
        line.positionCount = linePoints.Count;
        for (int i = 0; i < linePoints.Count; i++)
        {
            line.SetPosition(i, linePoints[i]);
        }
    }


    public void Shoot(Vector2 start, Vector2 force)
    {
        transform.position = start;
        transform.up = force;
        drawLine = true;
        linePoints.Clear();
        linePoints.Add(transform.position);
        linePoints.Add(player.position);
        DrawLine();
        line.enabled = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.isKinematic = false;
        rigidBody.AddForce(force);
    }

    public void Release()
    {
        hooked = false;
        drawLine = false;
        line.enabled = false;
        linePoints.Clear();
        line.positionCount = 0;
        transform.position = new Vector3(-100, 0, 0);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.transform.CompareTag("Player"))
        {
            rigidBody.isKinematic = true;
            rigidBody.velocity = Vector2.zero;

            UpdateLine();
            hooked = true;
            shooter.CreateJoint(transform.TransformPoint(anchorPoint));
        }
    }
}
