using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private Transform cameraArm;

    public float speed;
    public float jump;
    public float Foxspeed;
    public float Foxtime;
    
    float hAxis;
    float vAxis;
    public float turnspeed;

    bool isJump;
    bool isFox;

    bool jDown;
    bool fDown;

    Rigidbody rigid;

    Vector3 FoxVec;
    Vector3 moveVec;
    Vector3 move;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        LookAround();
        Move();
        GetInput();
        Jump();
        Fox();
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButton("Jump");
        fDown = Input.GetButtonDown("Fox");
    }



    void Move()
    {
        if(isFox) {
            moveVec = FoxVec;
        }
        if(!isFox) {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isMove = moveInput.magnitude != 0;

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            
            move = moveDir * Time.deltaTime * speed;
        }

        transform.position += move;
    }
    //3인칭 카메라
    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        
        cameraArm.rotation = Quaternion.Euler(camAngle.x - mouseDelta.y, camAngle.y + mouseDelta.x * turnspeed, camAngle.z);

    }
    void Jump()
    {
        if(jDown && !isJump) {
            rigid.AddForce(Vector3.up * jump,ForceMode.Impulse);
            isJump = true;
        }
    }
    void Fox()
    {
        if(fDown && !isFox && !isJump && move != Vector3.zero) {
            speed *= Foxspeed;
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            bool isMove = moveInput.magnitude != 0;

            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            
            move = moveDir * Time.deltaTime * speed;
            
            isFox = true;
            Debug.Log("adsad");

            Invoke("FoxOut", Foxtime);
        }
        if (isFox) {
            FoxVec = move;
        }
    }
    void FoxOut()
    {
        speed /= Foxspeed;
        isFox = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor") {
            isJump = false;
        }
    }
}