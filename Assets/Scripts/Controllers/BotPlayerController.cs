using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BotPlayerController : Player
{
    public AIDestinationSetter AIDestinationSetter;
    public AIPath AIPath;
    private bool _canMove = false;
    private bool _canDecide = false;

    private bool _isReachedEndOfPath = true;
    private PathController _pathController;

    public override void Start()
    {
        base.Start();
        AIPath.maxSpeed = MovementSpeed;
        AIPath.canMove = false;

        _pathController = GameController.Instance.PathController;
    }

    private void OnEnable()
    {
        GameplayUI.OnToggleChanged += BotToggleChanged;
        ItemContainer.OnItemsUpdated += ItemsUpdated;
    }

    public override void Update()
    {
        base.Update();
    }

    private void OnDestroy()
    {
        GameplayUI.OnToggleChanged -= BotToggleChanged;
        ItemContainer.OnItemsUpdated -= ItemsUpdated;
    }
    private void BotToggleChanged(bool state)
    {
        if (state)
        {
            _canMove = true;
            _canDecide = true;
            ItemsUpdated();
        }

        else
        {
            _canDecide = false;
            StopBot();
        }
    }

    private void StopBot()
    {
        if (items.Count > 0)
        {
            ItemContainer trashCan = _pathController.TrashCan;
            Transform targetPoint = trashCan.BotPoint;
            SetTarget(targetPoint);
        }
        else
        {
            SetBotDefault();
        }
    }

    private void FixedUpdate()
    {
        if (_canMove && !_isReachedEndOfPath && AIPath.reachedEndOfPath)
        {
            if (AIDestinationSetter.target != null)
            {
                Vector3 targetPosition = new Vector3(AIDestinationSetter.target.position.x, transform.position.y, AIDestinationSetter.target.position.z);
                float targetDistance = Vector3.Distance(targetPosition, transform.position);

                if (targetDistance <= AIPath.endReachedDistance)
                {
                    PathCompleted();
                    _isReachedEndOfPath = true;
                }
            }
        }
    }
    public void SetTarget(Transform target)
    {
        if (_canMove)
        {
            AIPath.canMove = true;
            AIDestinationSetter.target = target;

            SetAnimator(1);
            StartCoroutine(Util.DelayOneFrame(() => _isReachedEndOfPath = false));
        }
    }

    public void ItemsUpdated()
    {
        if (!_canDecide)
        {
            if (items.Count == 0) SetBotDefault();
        }
        else
        {
            if (items.Count > 0)
            {
                if (IsReachedMax() || IsContainerEmpty())
                {
                    string stackedItemsId = items[0].GetItemId();
                    ItemContainer nextItemContainer = _pathController.DecideTarget(stackedItemsId);

                    if (nextItemContainer.IsAvailable())
                    {
                        Transform targetPoint = nextItemContainer.BotPoint;
                        SetTarget(targetPoint);
                    }
                    else
                    {
                        Transform targetPoint = nextItemContainer.GetWaitingPoint(transform.position);
                        SetTarget(targetPoint);
                    }
                }
            }
            else
            {
                ItemContainer itemContainer = _pathController.GetAvailableItemContainer();

                if (itemContainer != null)
                {
                    Transform targetPoint = itemContainer.BotPoint;
                    SetTarget(targetPoint);
                }
            }
        }
    }

    private bool IsContainerEmpty()
    {
        if (_currentItemContainer != null)
        {
            if (_currentItemContainer.ContainerType != ContainerType.TrashCan)
            {
                return _currentItemContainer.Items.Count == 0;
            }
        }

        return false;
    }


    private void PathCompleted()
    {
        AIPath.canMove = false;
        SetAnimator(0);
    }

    private void SetBotDefault()
    {
        Transform targetPoint = _pathController.BotStartPoint;
        SetTarget(targetPoint);
    }

    private void SetAnimator(float value)
    {
        Animator.SetFloat("Movement", value);
        DOTween.To(() => currentSpeed, x => currentSpeed = x, value, 0.3f);
    }
}
