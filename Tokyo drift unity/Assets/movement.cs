using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float pitchthreshold = 10f;
    [SerializeField] private Transform vrcam;
    [SerializeField] private Transform OgPos;
    [SerializeField] private float maxspeed = 100f;
    
    [SerializeField] private Transform car;
    public float rotthreshold = 2f;
    


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

        if (vrcam.position.x > OgPos.position.x)
        {
            Vector3 moveDir = vrcam.forward;
            moveDir.y = 0f;
            moveDir.Normalize();
            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));

        }
        
        /*if (angle > pitchthreshold && angle < 360f - pitchthreshold)
        {
            Vector3 moveDir = vrcam.forward;
            moveDir.y = 0f;
            moveDir.Normalize();

            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
        }

        float rotAngle = vrcam.rotation.y;
        if (rotAngle < 360f - rotAngle)
        {
            car.rotation = Quaternion.Euler(0,vrcam.eulerAngles.y+180f,0);
        }*/
    }
}
