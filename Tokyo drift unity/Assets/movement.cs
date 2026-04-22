using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class movement : MonoBehaviour
{
    [SerializeField] private Transform vrcam;
    [SerializeField] private Transform OgPos;
    
    [SerializeField] private Transform car;

    public float turnRThreshold = 0.5f;
    public float turnLThreshold = -0.5f;

    private float verticalvel;
    public float gravity = -9.81f;

    public float accel = 0.01f;
    
    public float rotationSpeed = 60f;
    public float rotationThreshold = 15f;   // degrees required before turning

    private float neutralYaw;
    
    [SerializeField] private TextMeshProUGUI speed;

    [SerializeField] private CarAudio carAudio;

    [SerializeField] private CharacterController characterController;

    public ParticleSystem speedlines;
    private int maxspeedline = 75;
    private int linespeed;

    public int CheckPoint;

    public float currentlaptime;
    public float bestlaptime;
    public float lastlaptime;
    

    [SerializeField] private TextMeshProUGUI currentlaptext;
    [SerializeField] private TextMeshProUGUI bestlaptext;
    [SerializeField] private TextMeshProUGUI lastlaptext;

    public bool isDrifting;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vrcam = Camera.main.transform;
        
        characterController = GetComponent<CharacterController>();
        Mathf.Clamp(linespeed, 0f,75f);

    }

    // Update is called once per frame
    void Update()
    {            
        float mov = accel * vrcam.localPosition.z;

        carAudio.SetSpeed(Mathf.Abs(mov) * 100f);

        float lean = vrcam.localPosition.z - OgPos.localPosition.z;
        float leanX = vrcam.localPosition.x - OgPos.localPosition.x;
        
        isDrifting = Mathf.Abs(leanX) > turnRThreshold && lean > 0f;
        float driftMultiplier = isDrifting ? 2f : 1f;

        

        int movText = (int) mov;
        
        

        if (characterController.isGrounded)
        {
            verticalvel = -2f;
        }
        else
        {
            verticalvel += gravity * Time.deltaTime;
        }
        Vector3 movedir = Vector3.zero;
        
        movedir += car.forward * (lean * accel * driftMultiplier);
        

        //right left 
        if (vrcam.localPosition.x > OgPos.localPosition.x+turnRThreshold)
        {
            movedir += car.right * (leanX * accel);        
        }
        else if (vrcam.localPosition.x < OgPos.localPosition.x+turnLThreshold)
        {
            movedir += car.right * (leanX * accel);
            
        }
        

        movedir.y = verticalvel;
        characterController.Move(movedir*Time.deltaTime);
        
        var speedlinesEmission = speedlines.emission;
        
        float speedFactor = Mathf.Abs(vrcam.localPosition.z - OgPos.localPosition.z);        
        float emissionRate = Mathf.Clamp(speedFactor * 75, 0f, maxspeedline);
        speedlinesEmission.rateOverTime = emissionRate;

        updatelaptext();

        if (CheckPoint == 2)
        {
            lastlaptime = currentlaptime;
            if (bestlaptime == 0 ||currentlaptime < bestlaptime)
            {
                bestlaptime = currentlaptime;
            }
            currentlaptime = 0f;
            CheckPoint = 0;
        }

        if (CheckPoint > 0)
        {
            updatelaptime();
        }
        
        if (isDrifting)
        {
            movText = (int)mov + 50;
            speed.text = movText * 10 + "\nkm/h";
            Debug.Log("move int"+mov+"drift multi" + driftMultiplier+"movedirection vector3" + movedir);
            
        }
        else
        {
            speed.text = movText * 10 + "\nkm/h";
            Debug.Log("move int"+mov+"drift multi" + driftMultiplier+"movedirection vector3" + movedir);

        }

        HandleRotation();

        

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

    void updatelaptime()
    {
        currentlaptime += Time.deltaTime;
    }

    void updatelaptext()
    {
        currentlaptext.text = "Current lap " + FormatTime(currentlaptime);
        lastlaptext.text = "Last lap " + FormatTime(lastlaptime);
        bestlaptext.text = "Best lap " + FormatTime(bestlaptime);
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    } 

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            CheckPoint++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            CheckPoint = 1;
        }
    }

    
}
