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
        [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] float unlockedCameraHeight = 1.65f;
        [SerializeField] float lockedCameraHeight = 2f;
        private Coroutine cameraLockOnHeightCoroutine;
        private List<CharacterManager> avaliableTargets = new List<CharacterManager>();
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
            
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

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
                        if(Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers()))
                        {
                            continue;
                        }
                        else
                        {
                            Debug.Log("WE MADE IT");
                            avaliableTargets.Add(lockOnTarget);
                        }
                    }
                }
                //Debug.Log("lockOnTarget is null");
            }

            for(int k = 0; k < avaliableTargets.Count; k++)
            {
                if(avaliableTargets[k] != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, avaliableTargets[k].transform.position);

                    if(distanceFromTarget < shortDistance)
                    {
                        shortDistance = distanceFromTarget;
                        nearestLockOnTarget = avaliableTargets[k];
                    }

                    if (player.isLockedOn)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(avaliableTargets[k].transform.position);

                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if(avaliableTargets[k] == player.playerCombatManager.currentTarget) continue;

                        if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortDistanceOfLeftTarget)
                        {
                            shortDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = avaliableTargets[k];
                        }
                        else if(relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortDistanceOfRightTarget)
                        {
                            shortDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = avaliableTargets[k];
                        }
                    }
                }
                else
                {
                    ClearLockOnTarget();
                    player.isLockedOn = false;
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
            Debug.Log("ClearLockOnTarget");
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
            
            float duration = 1;
            float timer = 0;
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight); 
            Vector3 newUnlockedCameraHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);
            while (timer < duration)
            {
                timer += Time.deltaTime;
                if (player != null)
                {
                    if (player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed);
                        cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);
                    }
                    else
                    {
                        cameraPivotTransform.transform.localPosition =
                        Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }

                    yield return null;
                }
            }

            if (player != null)
            {
                if(player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                    cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                }
            }
        }

    }
}
