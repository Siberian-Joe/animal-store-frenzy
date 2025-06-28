using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public class PlayerViewModel : EntityViewModel<Player>
    {
        public ReadOnlyReactiveProperty<Vector2> Position => Proxy.Position;
        public ReadOnlyReactiveProperty<Vector2> TargetPosition => Proxy.TargetPosition;
        
        public PlayerViewModel(Player proxy) : base(proxy) 
        {
        }
        
        public override void Dispose()
        {
            base.Dispose();
            Position.Dispose();
            TargetPosition.Dispose();
        }
    }
}