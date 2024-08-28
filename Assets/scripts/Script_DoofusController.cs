
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoofusController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f; // JSON VALUE

    [Header("References")]
    [SerializeField] public Transform playerModel;

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (movement.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(movement.x, 0.0f, movement.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
        }
        transform.Translate(movement * speed * Time.deltaTime * 3, Space.World);
    }
}
