using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonInput : MonoBehaviour
{
    public Vector3 MouseLook { get; private set; }
    public Vector2 MoveInput { get; private set; }
    [HideInInspector]
    public bool Jump;
    public bool Sprint { get; private set; }
        
    [SerializeField] private Camera camera;

    private Vector3 previousMousePosition;
    private bool isPaused;
        
    private void Start()
    {
        previousMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if (isPaused) return;
            
        var mousePosition = Input.mousePosition;
        MouseLook = mousePosition - previousMousePosition;
        previousMousePosition = mousePosition;
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        MoveInput = new Vector3(inputX, inputY);
            
        // clamp for diagonal movement
        if (MoveInput.magnitude > 1f)
        {
            MoveInput.Normalize();
        }

        Sprint = Input.GetKey(KeyCode.LeftShift);
        Jump = Input.GetKeyDown(KeyCode.Space);
    }
}
