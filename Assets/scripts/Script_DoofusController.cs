using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DoofusController : MonoBehaviour
{
    private float speed;
    public Transform playerModel;
    public float leanDistance = 15f;

    void Start()
    {
        string filePath = Application.dataPath + "/DoofusDiary.json";
        if (File.Exists(filePath))
        {
            speed = JsonUtility.FromJson<PlayerData>(File.ReadAllText(filePath)).player_data.speed;
        }
        else
        {
            Debug.LogError("The file is missing!");
        }
    }

    private void Update()
    {
       
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        if (movement.magnitude > 0.1f)
        {
            Vector3 direction = new Vector3(movement.x, 0.0f, movement.z);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation *= Quaternion.Euler(-movement.z * leanDistance, 0, movement.x * leanDistance);
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f);
        }
        transform.Translate(movement * speed * Time.deltaTime * 3, Space.World);
    }

    public void goToMainMenu()
    {
        if (transform.position.y < 5.0f)
        {
            SceneManager.LoadSceneAsync("GameScene");
        }
    }

   
}

[System.Serializable]
public class PlayerData
{
    public Player player_data;
}

[System.Serializable]
public class Player
{
    public float speed;
}