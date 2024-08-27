using UnityEngine;

public class DoofusController : MonoBehaviour
{
    private float speed = 10.0f;
    public Transform playerModel;
    public float tiltAngle = 15f;

    void Start()
    {
        
    }
    private void Update()
    {
        if (transform.position.y < -1)
        {
            GameOver();
          
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (movement.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(movement.x, 0.0f, movement.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation *= Quaternion.Euler(-movement.z * tiltAngle, 0, movement.x * tiltAngle);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
        }
        transform.Translate(movement * speed * Time.deltaTime * 3, Space.World);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }
}


