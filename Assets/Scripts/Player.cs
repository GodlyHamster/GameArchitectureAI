using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float maxSpeed = 3f;

    [SerializeField]
    private Rigidbody rb;

    void Update()
    {
        //player movement
        Vector3 movementValue = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            movementValue += new Vector3(0, 0, 1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementValue += new Vector3(0, 0, -1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementValue += new Vector3(1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementValue += new Vector3(-1f, 0, 0);
        }

        rb.AddForce(movementValue * speed * Time.deltaTime, ForceMode.Impulse);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
