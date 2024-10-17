using Services;
using UnityEngine;
using Zenject;

public abstract class InteractiveObject : MonoBehaviour, IInteractiveObject
{
    [Inject] private IObjectRegistrationService _objectRegistrationService;

    public Vector3 Position => transform.position;

    protected virtual void OnEnable()
    {
        _objectRegistrationService.Register(this);
    }

    protected virtual void OnDisable()
    {
        _objectRegistrationService.Unregister(this);
    }
}