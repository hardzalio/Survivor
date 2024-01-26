using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour {

    CharacterController controller;
    public new Camera camera;
    public CinemachineVirtualCamera virtualCamera;
    public PlayerControls controls;
    public PlayerInput playerInput;
    public PlayerColorInitializer colorInitializer;
    public PlayerScore score;
    public PlayerDeath playerDeath;


    public float playerSpeed = 5;
    public float gravity = 9.81f;

    private Vector2 playerMovement = Vector2.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 lookTowards = Vector3.forward;

    public PlayerScore playerScore;
    public UnityEvent updateScoreDisplay;
    public TextMeshProUGUI scoreDisplay;

    private EnergyManager energy;
    void Awake() {
        controls ??= new PlayerControls();
    }
    void Start() {
        controller = GetComponent<CharacterController>();
        energy = GetComponent<EnergyManager>();
        playerInput = GetComponent<PlayerInput>();
        controls.devices = playerInput.devices;
        updateScoreDisplay ??= new UnityEvent();

        updateScoreDisplay.AddListener(OnScoreChange);
        playerDeath.onPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath() {
        controls.Disable();
    }

    private void OnEnable() {
        controls.Movement.Movement.performed += OnMovement;
        controls.Movement.Movement.canceled += OnMovement;
        controls.Movement.Rotation.performed += OnRotate;
        controls.Movement.Enable();
    }
    private void OnDisable() {
        controls.Movement.Movement.performed -= OnMovement;
        controls.Movement.Movement.canceled -= OnMovement;
        controls.Movement.Rotation.performed -= OnRotate;
        controls.Movement.Disable();
    }

    // Update is called once per frame
    void Update() {
        var movement = Time.deltaTime * playerSpeed * new Vector3(playerMovement.x, 0, playerMovement.y);
        controller.Move(movement);
        if (controller.isGrounded) {
            playerVelocity.y = 0;
        }
        else {
            playerVelocity.y -= gravity * Time.deltaTime;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        if (controller.isGrounded)
            transform.LookAt(lookTowards);

        if (Input.GetKeyDown(KeyCode.J)) {
            Debug.Log(energy.UseEnergy(10));
        }
    }
    public void OnRotate(InputAction.CallbackContext context) {
        var name = context.action.activeControl.device.name;
        if (name is "Keyboard" or "Mouse") {
            var mousePosition = context.ReadValue<Vector2>();
            var mouseWorldPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane) + (Vector3.forward * (camera.transform.position.y - transform.position.y)));
            mouseWorldPosition.y = transform.position.y;
            lookTowards = mouseWorldPosition;
            Debug.DrawLine(transform.position, mouseWorldPosition, Color.red);
        }
        else {
            var gamepadPosition = context.ReadValue<Vector2>();
            var gamepadWorldPosition = new Vector3(gamepadPosition.x, 0, gamepadPosition.y) + transform.position;
            lookTowards = gamepadWorldPosition;
        }
    }

    public void OnMovement(InputAction.CallbackContext context) {
        playerMovement = context.ReadValue<Vector2>();
    }

    public void OnScoreChange() {
        scoreDisplay.text = playerScore.score.ToString();
    }
}
