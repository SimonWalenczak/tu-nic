using UnityEngine;

namespace Tunic
{
    public class CameraSwapper : Etienne.Singleton<CameraSwapper>
    {
        [SerializeField] Cinemachine.CinemachineVirtualCamera vCamBase, vCamPlanet;

        public void ToBase()
        {
            vCamBase.Priority = 10;
            vCamPlanet.Priority = 0;
        }

        public void ToPlanet()
        {
            vCamBase.Priority = 0;
            vCamPlanet.Priority = 10;
        }
    }
}
