using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] float setCameraHeightSpeed = 0.75f;
        [SerializeField] float minimumPivot = -30f;
        [SerializeField] float maximumPivot = 60f;
        [SerializeField] float cameraCollisionOffset = 0.2f;    //radius
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjPos;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

        [Header("Lock On")]
        [SerializeField] private float lockOnRadius = 20f;
        [SerializeField] private float minimumViewableAngle = -50f;
        [SerializeField] private float maximumViewableAngle = 50f;
        [SerializeField] private float maximumLockOnDistance = 20f;
        [SerializeField] float lockOnTargetFollowSpeed = 5f;
        [SerializeField] float unlockedCameraHeight = 1.65f;
        [SerializeField] float lockedCameraHeight = 2f;
        private Coroutine cameraLockOnHeightCoroutine;
        [SerializeField] private List<CharacterManager> avaliableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockOnTarget;
        public CharacterManager rightLockOnTarget;


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
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleCameraActions()
        {
            if(player != null)
            {
                FollowTarget();
                HandleRotation();
                HandleCollisions();
                RotateTargetTransform();
            }
        }

        //ADDED BY ME, COULD BRICK EVERYTHING
        private void RotateTargetTransform()
        {
            player.playerCombatManager.lockOnTransform.transform.rotation = cameraPivotTransform.localRotation;
        }

        private void FollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotation()
        {
            // If we are locked on and have a target, handle lock-on logic
            if (player.isLockedOn && player.playerCombatManager.currentTarget != null)
            {
                // If the target died while locked on, break lock-on and sync to current view
                if (player.playerCombatManager.currentTarget.isDead)
                {
                    player.isLockedOn = false;

                    // Clear target on combat side (adjust to your API)
                    player.playerCombatManager.SetTarget(null);
                    ClearLockOnTarget();

                    // Sync free look angles to the current camera orientation (from pivot)
                    Vector3 flatForward = cameraPivotTransform.forward;
                    flatForward.y = 0;
                    flatForward.Normalize();

                    float yaw = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
                    leftAndRightLookAngle = yaw;
                    upAndDownLookAngle = cameraPivotTransform.localEulerAngles.x;

                    // Fall through to free-look below
                }
                else
                {
                    // Normal lock on rotation
                    Vector3 direction = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;

                    direction.y = 0;
                    direction.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    cameraPivotTransform.rotation = Quaternion.Slerp(
                        cameraPivotTransform.rotation,
                        targetRotation,
                        lockOnTargetFollowSpeed * Time.deltaTime
                    );

                    // Keep free-look angles in sync with what the camera is actually doing
                    Vector3 flatForward = cameraPivotTransform.forward;
                    flatForward.y = 0;
                    flatForward.Normalize();

                    float yaw = Mathf.Atan2(flatForward.x, flatForward.z) * Mathf.Rad2Deg;
                    leftAndRightLookAngle = yaw;
                    upAndDownLookAngle = cameraPivotTransform.localEulerAngles.x;

                    return; // done for this frame
                }
            }

            // FREE-LOOK (runs when not locked on, or after death handling above)
            leftAndRightLookAngle += PlayerInputManager.inputs.horizontalCameraInput * leftAndRightRotationSpeed * Time.deltaTime;
            upAndDownLookAngle -= PlayerInputManager.inputs.verticalCameraInput * upAndDownRotationSpeed * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            transform.rotation = Quaternion.Euler(0f, leftAndRightLookAngle, 0f);
            cameraPivotTransform.localRotation = Quaternion.Euler(upAndDownLookAngle, 0f, 0f);
        }


        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;

            if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisionOffset, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetCameraZPosition) < cameraCollisionOffset)
            {
                targetCameraZPosition = -cameraCollisionOffset;
            }
            cameraObjPos.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjPos;
        }

        public void HandleLocatingLockOnTargets()
        {
            Debug.Log("HandleLocatingLockOnTargets");
            avaliableTargets.Clear();
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;

            float shortestDistance = Mathf.Infinity;
            float leftScore = -Mathf.Infinity;
            float rightScore = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(
                player.transform.position,
                lockOnRadius,
                WorldUtilityManager.Instance.GetCharacterLayers()
            );

            foreach (var col in colliders)
            {
                CharacterManager target = col.GetComponent<CharacterManager>();
                if (target == null) continue;
                if (target.isDead) continue;
                if (target.transform.root == player.transform.root) continue;

                float distance = Vector3.Distance(player.transform.position, target.transform.position);
                if (distance > maximumLockOnDistance) continue;

                Vector3 direction = target.transform.position - cameraObject.transform.position;
                float signedAngle = Vector3.SignedAngle(cameraObject.transform.forward, direction, Vector3.up);

                if (signedAngle < minimumViewableAngle || signedAngle > maximumViewableAngle)
                    continue;


                if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, target.characterCombatManager.lockOnTransform.position, WorldUtilityManager.Instance.GetEnviroLayers()))
                    continue;

                avaliableTargets.Add(target);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestLockOnTarget = target;
                }
            }

            if (!player.isLockedOn) return;

            foreach (var target in avaliableTargets)
            {
                if (target == player.playerCombatManager.currentTarget) continue;

                Vector3 relativePos = player.transform.InverseTransformPoint(target.transform.position);

                if (relativePos.x <= 0 && relativePos.x > leftScore)
                {
                    leftScore = relativePos.x;
                    leftLockOnTarget = target;
                }
                else if (relativePos.x >= 0 && relativePos.x < rightScore)
                {
                    rightScore = relativePos.x;
                    rightLockOnTarget = target;
                }
            }
        }

        public void SetLockCameraHeight()
        {
            if(cameraLockOnHeightCoroutine != null) StopCoroutine(cameraLockOnHeightCoroutine);

            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        public void ClearLockOnTarget()
        {
            player.isLockedOn = false;
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            avaliableTargets.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while(player.isPerformingAction) yield return null;

            ClearLockOnTarget();
            HandleLocatingLockOnTargets();

            if(nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.isLockedOn = true;
            }

            yield return null;
        }

        private IEnumerator SetCameraHeight()
        {
            
            float duration = 1f;
            float timer = 0f;
            Vector3 velocity = Vector3.zero;

            Vector3 lockedHeight = new Vector3(cameraPivotTransform.localPosition.x, lockedCameraHeight, cameraPivotTransform.localPosition.z);

            Vector3 unlockedHeight = new Vector3(cameraPivotTransform.localPosition.x, unlockedCameraHeight, cameraPivotTransform.localPosition.z);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.localPosition = Vector3.SmoothDamp(
                        cameraPivotTransform.localPosition,
                        lockedHeight,
                        ref velocity,
                        setCameraHeightSpeed
                    );

                    cameraPivotTransform.localRotation = Quaternion.Slerp(
                        cameraPivotTransform.localRotation,
                        Quaternion.Euler(0, 0, 0),
                        lockOnTargetFollowSpeed * Time.deltaTime
                    );
                }
                else
                {
                    cameraPivotTransform.localPosition = Vector3.SmoothDamp(
                        cameraPivotTransform.localPosition,
                        unlockedHeight,
                        ref velocity,
                        setCameraHeightSpeed
                    );
                }

                yield return null;
            }
        }

    }
}
