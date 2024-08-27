
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoofusController : MonoBehaviour
{
    private float speed = 3f; // JSON VALUE
    public Transform playerModel;
    public float leanDistance = 15f;

    void FixedUpdate()
    {
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (movement.magnitude > 0.1f)
        {
            Vector3 direction = new(movement.x, 0.0f, movement.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation *= Quaternion.Euler(-movement.z * leanDistance, 0, movement.x * leanDistance);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
        }
        transform.Translate(movement * speed * Time.deltaTime * 3, Space.World);
    }

}
