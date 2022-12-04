using UnityEngine;
using UnityEngine.InputSystem;

namespace ElectorchStrauss.ProceduralTerrainSystem.Scripts
{
    public class GameController : MonoBehaviour
    {
        PlayerInput playerInput;

        [SerializeField, HideInInspector] public InputAction rotateAroundAction,
            focusSelectedAction,
            moveShiftAction,
            moveInOutAction,
            mousePosAction,
            leftClickAction;

        [Header("Rotate around : press middle mouse button")]
        public float rotateAround;

        [Header("Focus selected object : numpad . key")]
        public float focusSelected;

        [Header("Move left right up and down : shift key")]
        public float moveShift;

        [Header("Move forward backward : mouse scroll wheel up and down")]
        public Vector2 moveInOut;

        public Vector2 mousePos;
        public float leftClick;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            rotateAroundAction = playerInput.actions["RotateAround"];
            focusSelectedAction = playerInput.actions["FocusSelected"];
            moveShiftAction = playerInput.actions["MoveShift"];
            moveInOutAction = playerInput.actions["MoveInOut"];
            mousePosAction = playerInput.actions["MousePos"];
            leftClickAction = playerInput.actions["LeftClick"];
        }

        void Update()
        {
            InputActionReadValue();
        }

        private void InputActionReadValue()
        {
            rotateAround = rotateAroundAction.ReadValue<float>();
            focusSelected = focusSelectedAction.ReadValue<float>();
            moveShift = moveShiftAction.ReadValue<float>();
            moveInOut = moveInOutAction.ReadValue<Vector2>();
            mousePos = mousePosAction.ReadValue<Vector2>();
            leftClick = leftClickAction.ReadValue<float>();
        }
    }
}