using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Group1{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera cam;
        [SerializeField] private WorldUtilityManager utility;

        public Camera cameraObject;
        public PlayerManager player;
        [SerializeField] Transform cameraPivotTransform;

        [Header("Camera Settings")]
        [SerializeField] private float cameraSmoothSpeed = 10f;
        [SerializeField] float leftAndRightRotationSpeed = 220f;
        [SerializeField] float upAndDownRotationSpeed = 220f;
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
        private List<CharacterManager> avaliableTarget = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;

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
                //RotateTargetTransform();
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
            if (player.isLockedOn)
            {
                Debug.Log("Player Locked On");
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();

                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            else
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
            
            float shortDistance = Mathf.Infinity;
            float shortDistanceOfRightTarget = Mathf.Infinity;
            float shortDistanceOfLeftTarget = -Mathf.Infinity;
            
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, utility.Instance.GetCharacterLayers());

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
                
                if(lockOnTarget != null)
                {
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.position);

                    if(lockOnTarget.isDead) return;

                    if(lockOnTarget.transform.root == player.transform.root) continue;

                    if(distanceFromTarget > maximumLockOnDistance) continue;

                    if(viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;
                        if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, utility.Instance.GetEnviroLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            Debug.Log("WE MADE IT");
                            avaliableTarget.Add(lockOnTarget);
                        }
                    }
                }
                //Debug.Log("lockOnTarget is null");
            }

            for(int k = 0; k < avaliableTarget.Count; k++)
            {
                if(avaliableTarget[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, avaliableTarget[k].transform.position);

                    if(distanceFromTarget < shortDistance)
                    {
                        shortDistance = distanceFromTarget;
                        nearestLockOnTarget = avaliableTarget[k];
                    }
                }
                else
                {
                    ClearLockOnTarget();
                    player.isLockedOn = false;
                }
            }
        }

        public void ClearLockOnTarget()
        {
            Debug.Log("ClearLockOnTarget");
            nearestLockOnTarget = null;
            avaliableTarget.Clear();
        }
    }
}
