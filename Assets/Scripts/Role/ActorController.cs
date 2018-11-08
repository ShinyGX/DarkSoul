using System;
using DS.Camera;
using DS.Role.FSM;
using UnityEngine;


//-------------------------------------------
//  author: Billy
//  description:  
//-------------------------------------------

namespace DS.Role
{
    [RequireComponent(typeof(PlayerInput))]
    public class ActorController : MonoBehaviour
    {
        [Header("-----player prefab------")]
        public GameObject player;

        [Header("-----player setting-----")]
        public float moveVelocity;
        public float runVelocity;
        public float jumpVelocity;


        [Header("--------physic material-----")]
        public PhysicMaterial fullFriction;
        public PhysicMaterial zeroFriction;

        private CapsuleCollider _capsuleCollider;
        private CameraController _cameraController;

        private Animator _animator;
        private Rigidbody _rigidbody;

        public Vector3 AnimMovePos { set; get; }
        public PlayerInput InputSignal { get; private set; }

        private IMachine _machine;

        private readonly IPlayerState _groundState = new GroundState();
        private readonly IPlayerState _airState = new AirState();
        private readonly IPlayerState _attackState = new AttackState();
        private readonly IPlayerState _hitState = new HitState();
        private readonly IPlayerState _defenseState = new DefenseState();
        private readonly IPlayerState _deathState = new DeathState();
  

        // Use this for initialization
        void Awake()
        {
            var inputList = GetComponents<PlayerInput>();
            foreach (var input in inputList)
            {
                if (!input.enabled) continue;
                InputSignal = input;
                break;
            }

            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _animator = player.GetComponent<Animator>();

            _cameraController = GetComponentInChildren<CameraController>();

            _machine = new PlayerFiniteStateMachine(player, this, _groundState, InputSignal, _animator, _rigidbody);
        }

        // Update is called once per frame
        void Update()
        {
            GroundAnimHandler();
            ActionHandler();


            _machine.Update();
        }

        private void ActionHandler()
        {
            if (InputSignal.Roll || _rigidbody.velocity.magnitude > 8f)
            {
                _animator.SetTrigger(ProjectConstant.AnimatorParameter.ROLL);
            }
            else
            {
                _animator.ResetTrigger(ProjectConstant.AnimatorParameter.ROLL);
            }

            if (InputSignal.Jump)
            {
                _animator.SetTrigger(ProjectConstant.AnimatorParameter.JUMP);
                _machine.TranslateTo(_airState);
            }

            if (InputSignal.Attack && _machine.GetCurrentState() != _airState)
            {
                _animator.SetTrigger(ProjectConstant.AnimatorParameter.ATTACK);
                _machine.TranslateTo(_attackState);
            }

            if (InputSignal.Defense  && _machine.GetCurrentState() == _attackState )
            {
                _animator.SetTrigger(ProjectConstant.AnimatorParameter.DEFENSE);
            }
        }


        private void FixedUpdate()
        {
            _machine.FixUpdate();
        }


        private void GroundAnimHandler()
        {
            float forwardValue = InputSignal.SignalValueMagic * (InputSignal.Run ? 3.0f : 4.0f);
            if (!InputSignal.Run)
                forwardValue = Mathf.Clamp(forwardValue, 0, 1);
            if (!_cameraController.IsLock)
            {
                //在非锁定敌人的状况下无需right参数进行左右移动
                _animator.SetFloat(ProjectConstant.AnimatorParameter.FORWARD, forwardValue);
                _animator.SetFloat(ProjectConstant.AnimatorParameter.RIGHT, 0);
            }
            else
            {
                if (!InputSignal.Run && Math.Abs(_animator.GetFloat(ProjectConstant.AnimatorParameter.FORWARD)) > 0.1f)
                {
                    _animator.SetFloat(ProjectConstant.AnimatorParameter.RIGHT, 0);
                }
                else
                {
                    _animator.SetFloat(ProjectConstant.AnimatorParameter.RIGHT,
                        Mathf.Lerp(_animator.GetFloat(ProjectConstant.AnimatorParameter.RIGHT),
                            (InputSignal.Run ? 2.0f : 1.0f) * InputSignal.RightValue, 0.1f));
                }


                _animator.SetFloat(ProjectConstant.AnimatorParameter.FORWARD,
                    (InputSignal.Run ? 2.0f : 1.0f ) * InputSignal.UpValue);

            }
        }

        private void OnDeathStateEnter()
        {
            _machine.TranslateTo(_deathState);
        }

        private void OnDefenseStateEnter()
        {
            _machine.TranslateTo(_defenseState);
        }

        private void OnGroundEnter()
        {
            _machine.TranslateTo(_groundState);
        }

        private void OnGround()
        {
            _capsuleCollider.material = fullFriction;
            _animator.SetBool(ProjectConstant.AnimatorParameter.ON_GROUND, true);
        }

        private void NotOnGround()
        {
            _capsuleCollider.material = zeroFriction;
            _animator.SetBool(ProjectConstant.AnimatorParameter.ON_GROUND, false);
        }

        private void OnHitUpwordStateEnter()
        {
            _machine.TranslateTo(_hitState);
        }

        private void AnimatorMove(object movePos)
        {
            AnimMovePos += (Vector3) movePos;
        }

        private int GetLayerIndex(string layerName)
        {
            return _animator.GetLayerIndex(layerName);
        }

        public void IssueTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        public string GetCurrentState()
        {
            return _machine.GetCurrentStateName();
        }
    }
}
