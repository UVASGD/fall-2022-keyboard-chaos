using UnityEngine;

namespace ElectorchStrauss.ProceduralTerrainSystem.Scripts
{
    public class CameraMove : MonoBehaviour
    {
        private Camera mainCamera;
        public GameObject gameController;
        private GameController _gameController;
        public GameObject objectFocused;
        private Vector3 rotations;
        [SerializeField]private Vector3 previousPosition;
        [SerializeField] [Range(0, 360)] private int maxRotationInOneSwipe = 360;
        [SerializeField] private float distanceToTarget = -900;
        //change speed according to distanceToTarget
        [SerializeField] private float speed = 500;
        // Start is called before the first frame update
        void Start()
        {
            mainCamera = GetComponent<Camera>();
            _gameController = gameController.transform.GetComponent<GameController>();
        }
        // Update is called once per frame
        void Update()
        {
            if (_gameController.rotateAroundAction.triggered || _gameController.moveInOut.y != 0)
            {
                previousPosition = mainCamera.ScreenToViewportPoint(_gameController.mousePos);
            }
            if (_gameController.moveShift > 0 && _gameController.rotateAround > 0 || _gameController.moveInOut.y != 0)
            {
                Vector3 newPosition = mainCamera.ScreenToViewportPoint(_gameController.mousePos);
                Vector3 direction = previousPosition - newPosition;
                mainCamera.transform.Translate( direction.y * speed * Vector3.up);
                mainCamera.transform.Translate( direction.x * speed * Vector3.right);
                previousPosition = newPosition;
            }
            if (_gameController.rotateAround > 0 && _gameController.moveShift == 0 || _gameController.moveInOut.y != 0)
            {
                Vector3 newPosition = mainCamera.ScreenToViewportPoint(_gameController.mousePos);
                Vector3 direction = previousPosition - newPosition;
                mainCamera.transform.position = objectFocused.transform.position;
                mainCamera.transform.Rotate(Vector3.right, direction.y * maxRotationInOneSwipe);
                mainCamera.transform.Rotate(Vector3.up, -direction.x * maxRotationInOneSwipe, Space.World);
                mainCamera.transform.Translate(new Vector3(0,0,distanceToTarget));
                previousPosition = newPosition;
            }
            if (_gameController.moveInOut.y > 0)
            {
                distanceToTarget += 10f;
            }
            if (_gameController.moveInOut.y < 0)
            {
                distanceToTarget -= 10f;
            }
        }
    }
}