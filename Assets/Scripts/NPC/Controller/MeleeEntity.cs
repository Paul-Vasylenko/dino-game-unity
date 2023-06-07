using System;
using System.Collections;
using Core.Enums;
using Core.Services.Updater;
using NPC.Behaviour;
using Pathfinding;
using StatsSystem;
using StatsSystem.Enum;
using UnityEngine;

namespace NPC.Controller
{
    public class MeleeEntity : Entity
    {
        private readonly Seeker _seeker;
        private readonly MeleeEntityBehaviour _meleeEntityBehaviour;
        private readonly Vector2 _moveDelta;

        private bool _isAttacking;

        private Coroutine _searchCoroutine;
        private Collider2D _target;
        private Vector3 _previousTargetPosition;
        private Vector3 _destination;
        private float _stoppingDistance;
        private Path _currentPath;
        private int _currentWayPoint;

        public MeleeEntity(MeleeEntityBehaviour meleeEntityBehaviour, StatsController statsController) :
            base(meleeEntityBehaviour, statsController)
        {
            _seeker = meleeEntityBehaviour.GetComponent<Seeker>();
            _meleeEntityBehaviour = meleeEntityBehaviour;
            _meleeEntityBehaviour.AttackSequenceEnded += OnAttackEnded;
            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdateCalled;
            var speedDelta = StatsController.GetStatValue(StatType.Speed) * Time.fixedDeltaTime;
            _moveDelta = new Vector2(speedDelta, speedDelta / 2);
        }

        private IEnumerator SearchCoroutine()
        {
            while (!_isAttacking)
            {
                if (!TryGetTarget(out _target))
                {
                    ResetMovement();
                }
                else if (_target.transform.position != _previousTargetPosition)
                {
                    Vector2 position = _target.transform.position;
                    _previousTargetPosition = position;
                    _stoppingDistance = (_target.bounds.size.x + _meleeEntityBehaviour.Size.x) / 2;
                    var delta = position.x < _meleeEntityBehaviour.transform.position.x ? 1 : -1;
                    _destination = position + new Vector2(_stoppingDistance * delta, 0);
                    _seeker.StartPath(_meleeEntityBehaviour.transform.position, _destination, OnPathCalculated);
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void OnPathCalculated(Path path)
        {
            if (path.error)
                return;

            _currentPath = path;
            _currentWayPoint = 0;
        }

        private bool TryGetTarget(out Collider2D target)
        {
            target = Physics2D.OverlapBox(_meleeEntityBehaviour.transform.position, _meleeEntityBehaviour.SearchBox, 0,
                _meleeEntityBehaviour.Targets);

            return target != null;
        }

        private void OnFixedUpdateCalled()
        {
            if (_isAttacking || _target == null || _currentPath == null || CheckIfCanAttack() || _currentWayPoint >= _currentPath.vectorPath.Count)
                return;

            Vector2 currentPosition = _meleeEntityBehaviour.transform.position;
            Vector2 waypointPosition = _currentPath.vectorPath[_currentWayPoint];
            var waypointDirection = waypointPosition - currentPosition;

            if (Vector2.Distance(waypointPosition, currentPosition) < 0.2f)
            {
                _currentWayPoint++;
                return;
            }

            if (waypointDirection.y != 0)
            {
                waypointDirection.y = waypointDirection.y > 0 ? 1 : -1;
            }

            if (waypointDirection.x == 0)
                return;

            waypointDirection.x = waypointDirection.x > 0 ? 1 : -1;
            var newHorizontalPosition = currentPosition.x + _moveDelta.y * waypointDirection.x;
            if (waypointDirection.x > 0 && waypointPosition.x < newHorizontalPosition ||
                waypointDirection.x < 0 && waypointPosition.x > newHorizontalPosition)
                newHorizontalPosition = waypointPosition.x;

            if (newHorizontalPosition != _meleeEntityBehaviour.transform.position.x)
                _meleeEntityBehaviour.MoveHorizontally(newHorizontalPosition);
        }

        private bool CheckIfCanAttack()
        {
            var distance = _destination - _meleeEntityBehaviour.transform.position;
            if (Mathf.Abs(distance.x) > 0.2f || Mathf.Abs(distance.y) > 0.2f)
                return false;

            _meleeEntityBehaviour.MoveHorizontally(_destination.x);
            _meleeEntityBehaviour.SetDirection(_meleeEntityBehaviour.transform.position.x > _target.transform.position.x ? Direction.Left : Direction.Right);
            ResetMovement();
            _isAttacking = true;
            _meleeEntityBehaviour.StartAttack();
            if (_searchCoroutine != null)
                ProjectUpdater.Instance.StopCoroutine(_searchCoroutine);

            return true;
        }

        private void ResetMovement()
        {
            _target = null;
            _currentPath = null;
            _previousTargetPosition = Vector2.negativeInfinity;
            var position = _meleeEntityBehaviour.transform.position;
            _meleeEntityBehaviour.MoveHorizontally(position.x);
        }

        private void OnAttackEnded()
        {
            _isAttacking = false;
            _searchCoroutine = ProjectUpdater.Instance.StartCoroutine(SearchCoroutine());
        }

        public override void VisualiseHp(float currentHp)
        {
            if (_meleeEntityBehaviour.HpBar.maxValue < currentHp)
                _meleeEntityBehaviour.HpBar.maxValue = currentHp;

            _meleeEntityBehaviour.HpBar.value = currentHp;
        }
    }
}