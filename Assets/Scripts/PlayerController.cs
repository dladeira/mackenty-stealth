using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AnimationCurve speedCurve;
    public float speedMultiplier;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDistance;

    [SerializeField] private PauseMenu pauseMenu;

    private float up;
    private float down;
    private float left;
    private float right;

    private float timeSinceDash;

    private Vector3 dashMove;

    void FixedUpdate()
    {
        Vector3 move = GetAxisMovement() * (pauseMenu.pauseMenuOpen ? 0 : speedMultiplier);
        rb.AddForce((move * Time.fixedDeltaTime) - rb.velocity, ForceMode.VelocityChange);
        timeSinceDash += Time.deltaTime;

        if (Input.GetAxisRaw("Jump") > 0 && timeSinceDash > dashCooldown)
        {
            Dash(move);
        }

        if (timeSinceDash < 0.25f)
        {
            rb.AddForce((dashMove * Time.fixedDeltaTime * (dashDistance * 10)) - rb.velocity, ForceMode.VelocityChange);
        }
    }

    Vector3 GetAxisMovement()
    {
        Vector3 move = new Vector3(0, 0, 0);

        up = CalculateMovementTime(Input.GetAxisRaw("Vertical") > 0, up);
        down = CalculateMovementTime(Input.GetAxisRaw("Vertical") < 0, down);
        right = CalculateMovementTime(Input.GetAxisRaw("Horizontal") > 0, right);
        left = CalculateMovementTime(Input.GetAxisRaw("Horizontal") < 0, left);

        if (up > 0 && down > 0)
        {

        }
        else if (up > 0)
        {
            move.z = speedCurve.Evaluate(up);
        }
        else if (down > 0)
        {
            move.z = -speedCurve.Evaluate(down);
        }

        if (left > 0 && right > 0)
        {

        }
        else if (right > 0)
        {
            move.x = speedCurve.Evaluate(right);
        }
        else if (left > 0)
        {
            move.x = -speedCurve.Evaluate(left);
        }

        if ((up > 0 || down > 0) && (left > 0 || right > 0))
        {
            move /= 1.5f;
        }

        return move;
    }

    float CalculateMovementTime(bool statement, float value)
    {
        if (statement)
        {
            return value += Time.deltaTime;
        }
        return 0;
    }

    void Dash(Vector3 move)
    {
        dashMove = move.normalized;
        timeSinceDash = 0;
    }
}