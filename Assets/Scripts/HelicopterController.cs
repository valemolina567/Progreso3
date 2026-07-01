using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [Header("Rotors")]
    public Transform mainRotor;
    public Transform tailRotor;

    [Header("Blade Speed")]
    public float bladeSpeed = 0f;
    public float acceleration = 500f;
    public float deceleration = 250f;
    public float maxBladeSpeed = 1800f;

    [Header("Flight")]
    public float liftThreshold = 400f;
    public float liftSpeed = 3f;
    public float gravity = 2f;

    // Altura inicial (suelo)
    private float groundY;

    void Start()
    {
        groundY = transform.position.y;
    }

    void Update()
    {
        HandleInput();
        RotateRotors();
        HandleLift();
    }

    // -------------------------
    // CONTROL DE VELOCIDAD
    // -------------------------
    void HandleInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            bladeSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            bladeSpeed -= deceleration * Time.deltaTime;
        }

        bladeSpeed = Mathf.Clamp(bladeSpeed, 0f, maxBladeSpeed);
    }

    // -------------------------
    // ROTACIÓN DE ASPAS
    // -------------------------
    void RotateRotors()
    {
        if (mainRotor != null)
            mainRotor.Rotate(0f, bladeSpeed * Time.deltaTime, 0f);

        if (tailRotor != null)
            tailRotor.Rotate(bladeSpeed * Time.deltaTime, 0f, 0f);
    }

    // -------------------------
    // DESPEGUE Y DESCENSO
    // -------------------------
    void HandleLift()
    {
        bool upPressed = Input.GetKey(KeyCode.UpArrow);

        // Despega si supera el umbral
        if (upPressed && bladeSpeed >= liftThreshold)
        {
            transform.position += Vector3.up * liftSpeed * Time.deltaTime;
        }
        // Baja suavemente si está en el aire
        else if (transform.position.y > groundY)
        {
            transform.position += Vector3.down * gravity * Time.deltaTime;

            // Evita atravesar el suelo
            if (transform.position.y < groundY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    groundY,
                    transform.position.z
                );
            }
        }
    }
}