using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 10f;
    [SerializeField] private Transform vrcam;
    [SerializeField] private Transform OgPos;
    
    [SerializeField] private Transform car;

    public float turnRThreshold = 0.5f;
    public float turnLThreshold = -0.5f;

    public float forThreshold = 0.2f;
    public float backThreshold = -0.2f;


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
        float angle = vrcam.position.x;

        if (vrcam.position.z > OgPos.position.z+forThreshold)
        {
            Vector3 moveDir = car.forward;
            moveDir.y = 0f;
            moveDir.Normalize();
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
        }
        else if (vrcam.position.z < OgPos.position.z+backThreshold)
        {
            Vector3 moveDir = -car.forward;
            moveDir.y = 0f;
            moveDir.Normalize();
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime)); 
        }

        
        if (vrcam.position.x > OgPos.position.x+turnRThreshold)
        {
            Vector3 moveDir = car.right;
            moveDir.y = 0f;
            moveDir.Normalize();
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
        }
        else if (vrcam.position.x < OgPos.position.x+turnLThreshold)
        {
            Vector3 moveDir = -car.right;
            moveDir.y = 0f;
            moveDir.Normalize();
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));   
        }

        float rotAngle = vrcam.rotation.y;
        if (rotAngle < 360f)
        {
            car.rotation = Quaternion.Euler(0,vrcam.eulerAngles.y,0);
        }
    }
}
