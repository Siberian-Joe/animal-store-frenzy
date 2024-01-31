using Core.Enums;
using Interfaces.Services.DataServices;
using UnityEngine;

namespace Core.ViewModels
{
    public class CameraViewModel : ViewModel<CameraModel>
    {
        public CameraViewModel(IDataService dataService) : base(dataService)
        {
        }

        protected override CameraModel CreateDefaultModel()
        {
            return new CameraModel();
        }

        public void UpdateTargetPosition(Vector3 newPosition)
        {
            Model.TargetPosition.Value = newPosition;
        }

        public void SetInitialOffset(Vector3 offset)
        {
            Model.Offset = offset;
        }
        
        public Vector3 CalculateDesiredPosition()
        {
            return Model.TargetPosition.Value + Model.Offset;
        }
    }
}