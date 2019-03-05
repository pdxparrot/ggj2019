using System.Collections;

using pdxpartyparrot.Core.DebugMenu;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Game.World;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Profiling;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class ClimbingControllerComponent3D : CharacterActorControllerComponent3D
    {
#region Actions
        public class GrabAction : CharacterActorControllerAction
        {
            public static GrabAction Default = new GrabAction();
        }

        public class ReleaseAction : CharacterActorControllerAction
        {
            public static ReleaseAction Default = new ReleaseAction();
        }
#endregion

        private enum ClimbMode
        {
            None,
            Climbing,
            Hanging
        }

        [SerializeField]
        [ReadOnly]
        private ClimbMode _climbMode = ClimbMode.None;

        public bool IsClimbing => _climbMode != ClimbMode.None;

// TODO: the number of constant raycasts could probably be reduced even more
// TODO: we probably also don't need to hold onto the hit results, just update and handle them one at a time

        [Space(10)]

#region Hands
        [Header("Hands")]

        [SerializeField]
        private Transform _leftHandTransform;

        private RaycastHit? _leftHandHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didLeftHandRaycast;

        [SerializeField]
        [ReadOnly]
        private bool _didWrapLeftRaycast;

        [SerializeField]
        [ReadOnly]
        private bool _didRotateLeftRaycast;

        private RaycastHit? _leftHandHangHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didLeftHandHangRaycast;

        private bool CanGrabLeft => null != _leftHandHitResult;

        private bool CanHangLeft => null != _leftHandHangHitResult;

        [SerializeField]
        private Transform _rightHandTransform;

        private RaycastHit? _rightHandHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didRightHandRaycast;

        [SerializeField]
        [ReadOnly]
        private bool _didWrapRightRaycast;

        [SerializeField]
        [ReadOnly]
        private bool _didRotateRightRaycast;

        private RaycastHit? _rightHandHangHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didRightHandHangRaycast;

        private bool CanGrabRight => null != _rightHandHitResult;

        private bool CanHangRight => null != _rightHandHangHitResult;
#endregion

        [SerializeField]
        private Transform _hangTransform;

        [Space(10)]

#region Head
        [Header("Head")]

        [SerializeField]
        private Transform _headTransform;

        private RaycastHit? _headHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didHeadRaycast;

        [SerializeField]
        [ReadOnly]
        private bool _didClimbUpRaycast;
#endregion

        [Space(10)]

#region Chest
        [Header("Chest")]

        [SerializeField]
        private Transform _chestTransform;

        private RaycastHit? _chestHitResult;

        [SerializeField]
        [ReadOnly]
        private bool _didChestRaycast;
#endregion

        private bool CanClimbUp => IsClimbing && (null == _headHitResult && null != _chestHitResult);

        [Space(10)]

#region Debug
        [Header("Debug")]

        [SerializeField]
        [Tooltip("Debug break when grabbing fails")]
        private bool _breakOnFall;

        private DebugMenuNode _debugMenuNode;
#endregion

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(Controller.ActorAnimator, "ClimbingControllerComponent requires an actor animator");
            Assert.IsTrue(Mathf.Approximately(_leftHandTransform.position.y, _rightHandTransform.position.y), "Character hands are at different heights!");
            Assert.IsTrue(_headTransform.position.y > _leftHandTransform.position.y, "Character head should be above player hands!");
            Assert.IsTrue(_chestTransform.position.y < _leftHandTransform.position.y, "Character chest should be below player hands!");

            StartCoroutine(RaycastRoutine());

            InitDebugMenu();
        }

        protected override void OnDestroy()
        {
            DestroyDebugMenu();

            base.OnDestroy();
        }

        private void OnDrawGizmos()
        {
            if(!Application.isPlaying) {
                return;
            }

// TODO: encapsulate the math here so we a) don't duplicate it in the raycast methods and b) guarantee we always match the math done in the raycast methods

            // left hand
            if(_didLeftHandRaycast) {
                Gizmos.color = null != _leftHandHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + transform.forward * Controller.ControllerData.ArmRayLength);
            }

            if(_didLeftHandHangRaycast) {
                Gizmos.color = null != _leftHandHangHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (transform.up) * Controller.ControllerData.HangRayLength);
            }

            if(_didWrapLeftRaycast) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 2.0f);
            }

            if(_didRotateLeftRaycast) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_leftHandTransform.position, _leftHandTransform.position + (Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 0.5f);
            }

            // right hand
            if(_didRightHandRaycast) {
                Gizmos.color = null != _rightHandHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + transform.forward * Controller.ControllerData.ArmRayLength);
            }

            if(_didRightHandHangRaycast) {
                Gizmos.color = null != _rightHandHangHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (transform.up) * Controller.ControllerData.HangRayLength);
            }

            if(_didWrapRightRaycast) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 2.0f);
            }

            if(_didRotateRightRaycast) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_rightHandTransform.position, _rightHandTransform.position + (Quaternion.AngleAxis(90.0f, transform.up) * transform.forward) * Controller.ControllerData.ArmRayLength * 0.5f);
            }

            // head
            if(_didHeadRaycast) {
                Gizmos.color = null != _headHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_headTransform.position, _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength);
            }

            if(_didClimbUpRaycast) {
                Gizmos.color = Color.white;
                Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength;
                Vector3 end = start + Controller.Owner.Height * -Vector3.up;
                Gizmos.DrawLine(start, end);
            }

            // chest
            if(_didChestRaycast) {
                Gizmos.color = null != _chestHitResult ? Color.red : Color.yellow;
                Gizmos.DrawLine(_chestTransform.position, _chestTransform.position + transform.forward * Controller.ControllerData.ChestRayLength);
            }
        }
#endregion

        public override bool OnAnimationMove(Vector3 axes, float dt)
        {
            switch(_climbMode)
            {
            case ClimbMode.None:
                return false;
            case ClimbMode.Climbing:
                break;
            case ClimbMode.Hanging:
                Controller.DefaultAnimationMove(axes, dt);
                break;
            }

#if !USE_SPINE
            if(null != Controller.Animator) {
                Controller.Animator.SetFloat(Controller.ControllerData.MoveXAxisParam, Controller.CanMove ? Mathf.Abs(Controller.LastMoveAxes.x) : 0.0f);
                Controller.Animator.SetFloat(Controller.ControllerData.MoveZAxisParam, Controller.CanMove ? Mathf.Abs(Controller.LastMoveAxes.y) : 0.0f);
            }
#endif
            return true;
        }

        public override bool OnPhysicsMove(Vector3 axes, float speed, float dt)
        {
            if(!IsClimbing) {
                return false;
            }

            switch(_climbMode)
            {
            case ClimbMode.Climbing:
                Vector3 velocity = Controller.Rotation3D * (axes * Controller.ControllerData.ClimbSpeed);
                if(Controller.DidGroundCheckCollide && velocity.y < 0.0f) {
                    velocity.y = 0.0f;
                }
                Controller.MovePosition(Controller.Position + velocity * dt);
                break;
            case ClimbMode.Hanging:
                Controller.DefaultPhysicsMove(axes, Controller.ControllerData.HangSpeed, dt);
                break;
            }

            return true;
        }

        public override bool OnPerformed(CharacterActorControllerAction action)
        {
            if(action is GrabAction) {
                if(IsClimbing) {
                    return true;
                }

                if(CanGrabLeft && CanGrabRight) {
                    StartClimbing();

                    if(null != _leftHandHitResult) {
                        AttachToSurface(_leftHandHitResult.Value, false);
                    } else if(null != _rightHandHitResult) {
                        AttachToSurface(_rightHandHitResult.Value, false);
                    }
                } else if(CanHangLeft && CanHangRight) {
                    StartHanging();

                    if(null != _leftHandHangHitResult) {
                        AttachToSurface(_leftHandHangHitResult.Value, true);
                    } else if(null != _rightHandHangHitResult) {
                        AttachToSurface(_rightHandHangHitResult.Value, true);
                    }
                } else {
                    return false;
                }

                return true;
            }

            if(action is ReleaseAction) {
                if(IsClimbing) {
                    StopClimbing();
                }

                return true;
            }

            return false;
        }

        private void StartClimbing()
        {
            _climbMode = ClimbMode.Climbing;
            Controller.UseGravity = false;

#if !USE_SPINE
            if(null != Controller.Animator) {
                Controller.Animator.SetBool(Controller.ControllerData.ClimbingParam, true);
                Controller.Animator.SetBool(Controller.ControllerData.HangingParam, false);
            }
#endif
        }

        private void StartHanging()
        {
            _climbMode = ClimbMode.Hanging;
            Controller.UseGravity = false;

#if !USE_SPINE
            if(null != Controller.Animator) {
                Controller.Animator.SetBool(Controller.ControllerData.ClimbingParam, false);
                Controller.Animator.SetBool(Controller.ControllerData.HangingParam, true);
            }
#endif
        }


        public void StopClimbing()
        {
            _climbMode = ClimbMode.None;
            Controller.UseGravity = true;

#if !USE_SPINE
            if(null != Controller.Animator) {
                Controller.Animator.SetBool(Controller.ControllerData.ClimbingParam, false);
                Controller.Animator.SetBool(Controller.ControllerData.HangingParam, false);
            }
#endif

            // fix our orientation, just in case
            Vector3 rotation = transform.eulerAngles;
            rotation.x = rotation.z = 0.0f;
            transform.eulerAngles = rotation;
        }

        private IEnumerator RaycastRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(Controller.RaycastRoutineRate);
            while(true) {
                UpdateRaycasts();

                HandleRaycasts();

                yield return wait;
            }
        }

        private void UpdateRaycasts()
        {
            ResetRaycastDebug();

            if(Controller.ActorAnimator.IsAnimating) {
                return;
            }

            Profiler.BeginSample("PlayerController.UpdateRaycasts");
            try {
                UpdateHandRaycasts();
                UpdateHeadRaycasts();
                UpdateChestRaycasts();
            } finally {
                Profiler.EndSample();
            }
        }

        private void ResetRaycastDebug()
        {
            // left hand
            _didLeftHandRaycast = false;
            _didWrapLeftRaycast = false;
            _didRotateLeftRaycast = false;
            _didLeftHandHangRaycast = false;

            // right hand
            _didRightHandRaycast = false;
            _didRotateRightRaycast = false;
            _didWrapRightRaycast = false;
            _didRightHandHangRaycast = false;

            // head
            _didHeadRaycast = false;
            _didClimbUpRaycast = false;

            // chest
            _didChestRaycast = false;
        }

#region Hand Raycasts
        private void UpdateHandRaycasts()
        {
            UpdateLeftHandRaycasts();
            UpdateRightHandRaycasts();
        }

        private void UpdateLeftHandRaycasts()
        {
            _didLeftHandRaycast = true;

            _leftHandHitResult = null;

            RaycastHit hit;
            if(Physics.Raycast(_leftHandTransform.position, transform.forward, out hit, Controller.ControllerData.ArmRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHitResult = hit;
                }
            }

            _didLeftHandHangRaycast = true;

            _leftHandHangHitResult = null;

            if(Physics.Raycast(_leftHandTransform.position, transform.up, out hit, Controller.ControllerData.HangRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _leftHandHangHitResult = hit;
                }
            }
        }

        private void UpdateRightHandRaycasts()
        {
            _didRightHandRaycast = true;

            _rightHandHitResult = null;

            if(Physics.Raycast(_rightHandTransform.position, transform.forward, out var hit, Controller.ControllerData.ArmRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHitResult = hit;
                }
            }

            _didRightHandHangRaycast = true;

            _rightHandHangHitResult = null;

            if(Physics.Raycast(_rightHandTransform.position, transform.up, out hit, Controller.ControllerData.HangRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _rightHandHangHitResult = hit;
                }
            }
        }
#endregion

#region Head Raycasts
        private void UpdateHeadRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _didHeadRaycast = true;

            _headHitResult = null;

            if(Physics.Raycast(_headTransform.position, Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward, out var hit, Controller.ControllerData.HeadRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _headHitResult = hit;
                }
            }
        }
#endregion

#region Chest Raycasts
        private void UpdateChestRaycasts()
        {
            if(!IsClimbing) {
                return;
            }

            _didChestRaycast = true;

            _chestHitResult = null;

            if(Physics.Raycast(_chestTransform.position, transform.forward, out var hit, Controller.ControllerData.ChestRayLength, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
                if(null != grabbable) {
                    _chestHitResult = hit;
                }
            }
        }
#endregion

        private void HandleRaycasts()
        {
            Profiler.BeginSample("PlayerController.HandleRaycasts");
            try {
                if(IsClimbing) {
                    HandleClimbingRaycasts();
                }
            } finally {
                Profiler.EndSample();
            }
        }

        private void HandleClimbingRaycasts()
        {
            if(Controller.ActorAnimator.IsAnimating) {
                return;
            }

            switch(_climbMode)
            {
            case ClimbMode.None:
                break;
            case ClimbMode.Climbing:
                if(CanGrabLeft && CanGrabRight) {
                    if(!CheckHang()) {
                        if(!CheckRotateLeft()) {
                            CheckRotateRight();
                        }
                    }
                } else if(!CanGrabLeft && CanGrabRight) {
                    CheckWrapLeft();
                } else if(CanGrabLeft && !CanGrabRight) {
                    CheckWrapRight();
                } else /*if(!CanGrabLeft && !CanGrabRight)*/ {
                    if(CanClimbUp) {
                        CheckClimbUp();
                    } else {
                        Debug.LogWarning("Unexpectedly fell off!");
                        StopClimbing();

                        if(_breakOnFall) {
                            Debug.Break();
                        }
                    }
                }
                break;
            case ClimbMode.Hanging:
                if(CanHangLeft && CanHangRight) {
                    CheckClimb();
                } else if(!CanHangLeft && CanHangRight) {
                    // TODO: climb up?
                } else if(CanHangLeft && !CanHangRight) {
                    // TODO: climb up?
                } else /*if(!CanHangLeft && !CanHangRight)*/ {
                    // TODO: climb up?

                    Debug.LogWarning("Unexpectedly fell off!");
                    StopClimbing();

                    if(_breakOnFall) {
                        Debug.Break();
                    }
                }
                break;
            }
        }

#region Auto-Rotate/Climb
// TODO: encapsulate the common code from these

        private bool CheckWrapLeft()
        {
            if(null == _rightHandHitResult || Controller.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            _didWrapLeftRaycast = true;

            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward, out var hit, Controller.ControllerData.ArmRayLength * 2.0f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckRotateLeft()
        {
            if(null == _rightHandHitResult || Controller.LastMoveAxes.x >= 0.0f) {
                return false;
            }

            _didRotateLeftRaycast = true;

            if(!Physics.Raycast(_leftHandTransform.position, Quaternion.AngleAxis(-90.0f, transform.up) * transform.forward, out var hit, Controller.ControllerData.ArmRayLength * 0.5f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _rightHandHitResult.Value.normal) {
                return false;
            }

            _leftHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;
        }

        private bool CheckWrapRight()
        {
            if(null == _leftHandHitResult || Controller.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            _didWrapRightRaycast = true;

            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(-Controller.ControllerData.WrapAroundAngle, transform.up) * transform.forward, out var hit, Controller.ControllerData.ArmRayLength * 2.0f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) + offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckRotateRight()
        {
            if(null == _leftHandHitResult || Controller.LastMoveAxes.x <= 0.0f) {
                return false;
            }

            _didRotateRightRaycast = true;

            if(!Physics.Raycast(_rightHandTransform.position, Quaternion.AngleAxis(90.0f, transform.up) * transform.forward, out var hit, Controller.ControllerData.ArmRayLength * 0.5f, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            IGrabbable grabbable = hit.transform.GetComponent<IGrabbable>();
            if(null == grabbable) {
                return false;
            }

            if(hit.normal == _leftHandHitResult.Value.normal) {
                return false;
            }

            _rightHandHitResult = hit;

            Vector3 offset = (Controller.Owner.Radius * 2.0f) * transform.forward;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) - offset, Quaternion.LookRotation(-hit.normal), Controller.ControllerData.WrapTimeSeconds);

            return true;

        }

        private bool CheckClimbUp()
        {
            if(Controller.LastMoveAxes.y <= 0.0f) {
                return false;
            }

            _didClimbUpRaycast = true;

            // cast a ray from the end of our rotated head check straight down to see if we can stand here
            Vector3 start = _headTransform.position + (Quaternion.AngleAxis(-Controller.ControllerData.HeadRayAngle, transform.right) * transform.forward) * Controller.ControllerData.HeadRayLength;
            float length = Controller.Owner.Height;

            if(!Physics.Raycast(start, -Vector3.up, out var hit, length, Controller.ControllerData.CollisionCheckLayerMask, QueryTriggerInteraction.Ignore)) {
                return false;
            }

            Vector3 offset = Controller.Owner.Radius * 2.0f * transform.forward;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, offset), Controller.Rotation3D, Controller.ControllerData.ClimbUpTimeSeconds, () => {
                StopClimbing();
            });

            return true;
        }

        private bool CheckHang()
        {
            if((!CanHangLeft && !CanHangRight) || Controller.LastMoveAxes.y <= 0.0f) {
                return false;
            }

            RaycastHit hit = (_leftHandHangHitResult ?? _rightHandHangHitResult).Value;

            Vector3 offset = -_hangTransform.localPosition;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, offset), Controller.Rotation3D, Controller.ControllerData.HangTimeSeconds, () => {
                StartHanging();
            });

            return true;
        }

        private bool CheckClimb()
        {
            if((!CanGrabLeft && !CanGrabRight) || !Controller.IsMoving) {
                return false;
            }

            RaycastHit hit = (_leftHandHitResult ?? _rightHandHitResult).Value;

            Vector3 offset = Controller.Owner.Radius * transform.up;
            Controller.ActorAnimator.StartAnimation3D(Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal) - offset, Controller.Rotation3D, Controller.ControllerData.ClimbDownTimeSeconds, () => {
                StartClimbing();
            });

            return true;
        }
#endregion

        private Vector3 GetSurfaceAttachmentPosition(RaycastHit hit, Vector3 offset)
        {
            // keep a set distance away from the surface
            Vector3 targetPoint = hit.point + (hit.normal * Controller.ControllerData.AttachDistance);
            Vector3 a = targetPoint - Controller.Position;
            Vector3 p = Vector3.Project(a, hit.normal);

            return p + offset;
        }

        private void AttachToSurface(RaycastHit hit, bool isHang)
        {
            Debug.Log($"Attach to surface {hit.transform.name}");

            // TODO: we can probably infer this using the dot product
            if(isHang) {
                Controller.Position = Controller.Position + GetSurfaceAttachmentPosition(hit, -_hangTransform.localPosition);
            } else {
                transform.forward = -hit.normal;
                Controller.Position = Controller.Position + GetSurfaceAttachmentPosition(hit, Controller.Owner.Radius * hit.normal);
            }
        }

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => $"Game.Player {Controller.Owner.Id} Climbing Component");
            _debugMenuNode.RenderContentsAction = () => {
                _breakOnFall = GUILayout.Toggle(_breakOnFall, "Break on fall");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
