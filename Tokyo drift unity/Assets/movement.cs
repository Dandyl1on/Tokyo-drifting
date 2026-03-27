using TMPro;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 10f;
    [SerializeField] private Transform vrcam;
    [SerializeField] private Transform OgPos;
    
    [SerializeField] private Transform car;

    public float turnRThreshold = 0.5f;
    public float turnLThreshold = -0.5f;

    private float verticalvel = 0f;
    public float gravity = -9.81f;

    public float accel = 0.01f;
    
    public float rotationSpeed = 60f;
    public float rotationThreshold = 15f;   // degrees required before turning

    private float neutralYaw;
    
    [SerializeField] private TextMeshProUGUI speed;

    

    [SerializeField] private CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vrcam = Camera.main.transform;
        
        characterController = GetComponent<CharacterController>();
        
     
    }

    // Update is called once per frame
    void Update()
    {            
        float mov = accel * vrcam.localPosition.z;

        speed.text = "Speed: "+ mov;
        if (characterController.isGrounded)
        {
            verticalvel = -2f;
        }
        else
        {
            verticalvel += gravity * Time.deltaTime;
        }

        characterController.Move(new Vector3(0, verticalvel * Time.deltaTime, 0));

        if (vrcam.localPosition.z > OgPos.localPosition.z)
        {
            characterController.Move(car.forward * (mov * Time.deltaTime));
        }
        else if (vrcam.localPosition.z < OgPos.localPosition.z)
        {
            characterController.Move(-car.forward * (moveSpeed * Time.deltaTime)); 
        }

        
        if (vrcam.localPosition.x > OgPos.localPosition.x+turnRThreshold)
        {
            characterController.Move(car.right * (moveSpeed * Time.deltaTime));
        }
        else if (vrcam.localPosition.x < OgPos.localPosition.x+turnLThreshold)
        {
            characterController.Move(-car.right * (moveSpeed * Time.deltaTime));   
        }
        
        // headset steering
        float yawDelta = Vector3.SignedAngle(car.forward, vrcam.forward, Vector3.up);

        if (Mathf.Abs(yawDelta) > rotationThreshold)
        {
            float turn = Mathf.Sign(yawDelta);
            car.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);
        }

    }
    void HandleRotation()
    {
        Vector3 headForward = vrcam.forward;
        headForward.y = 0f;
        headForward.Normalize();

        Vector3 carForward = car.forward;
        carForward.y = 0f;
        carForward.Normalize();

        float yawDelta = Vector3.SignedAngle(carForward, headForward, Vector3.up);

        if (yawDelta > rotationThreshold)
        {
            car.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (yawDelta < -rotationThreshold)
        {
            car.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}
