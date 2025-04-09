using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float jumpForce = 10.0f;

    [Header("Looking Around")]
    [SerializeField] private float sensitivity = 1.0f;

    [Header("Ground Detection")]
    [SerializeField] private bool grounded;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;

    private Rigidbody rb;
    private Vector3 direction;
    private Vector2 mouseRotation;

    void Start() {
        
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update() {

        if (!Player.instance.gameStarted || Player.instance.gamePaused) return;
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vertical + transform.right * horizontal;

        mouseRotation.y += Input.GetAxis("Mouse X") * sensitivity;
        mouseRotation.x -= Input.GetAxis("Mouse Y") * sensitivity;
        mouseRotation.x  = Mathf.Clamp(mouseRotation.x, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(0.0f, mouseRotation.y, 0.0f);
        Player.instance.head.rotation = Quaternion.Euler(mouseRotation.x, mouseRotation.y, 0.0f);

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(grounded && Input.GetKeyDown(KeyCode.Space)) {

            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

    }

    void FixedUpdate() {

        if (!Player.instance.gameStarted || Player.instance.gamePaused) return;
        
        rb.AddForce(direction.normalized * speed * Time.deltaTime * 1000.0f);

    }

}