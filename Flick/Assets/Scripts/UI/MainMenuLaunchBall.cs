using UnityEngine;

public class MainMenuLaunchBall : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidBody = null;

    void Start()
    {
        _rigidBody.AddForce(new Vector3(1.0f, 1.0f, 0.0f) * 350.0f);
    }

    void Update()
    {
        
    }
}
