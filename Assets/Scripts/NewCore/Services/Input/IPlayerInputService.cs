using NewCore.Modules.Interaction;
using R3;
using UnityEngine;

namespace NewCore.Services.Input
{
    public interface IPlayerInputService
    {
        Observable<ClickContext> Clicked { get; }
    }
}