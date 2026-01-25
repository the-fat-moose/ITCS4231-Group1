using UnityEngine;

namespace Group1{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera cam;

        public Camera cameraObject;
        public PlayerManager player;
        [SerializeField] Transform cameraPivotTransform;

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 10f;
        [SerializeField] float leftAndRightRotationSpeed = 220f;
        [SerializeField] float upAndDownRotationSpeed = 220f;
        [SerializeField] float minimumPivot = -30f;
        [SerializeField] float maximumPivot = 60f;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;


        private void Awake()
        {
            if(cam == null)
            {
                cam = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void HandleCameraActions()
        {
            if(player != null)
            {
                FollowTarget();
                HandleRotation();
                HandleCollisions();
            }
        }

        private void FollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            leftAndRightLookAngle += (PlayerInputManager.inputs.horizontalCameraInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (PlayerInputManager.inputs.verticalCameraInput * upAndDownRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCollisions()
        {
            
        }
    }
}
