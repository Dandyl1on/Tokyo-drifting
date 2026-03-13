using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float pitchthreshold = 20f;
    [SerializeField] private Transform vrcam;

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
        float angle = vrcam.localEulerAngles.x;

        if (angle > pitchthreshold && angle < 360f - pitchthreshold)
        {
            Vector3 moveDir = vrcam.forward;
            moveDir.y = 0f;
            moveDir.Normalize();

            characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
        }
    }
}
