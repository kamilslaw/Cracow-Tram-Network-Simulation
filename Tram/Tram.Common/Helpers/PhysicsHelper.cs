using System;
using Tram.Common.Consts;

namespace Tram.Common.Helpers
{
    public static class PhysicsHelper
    {
        public static float GetNewSpeed(float oldSpeed, float deltaTime, bool increase = true)
        {
            return oldSpeed + 
                   (increase ? 1 : -1) * deltaTime * VehicleConsts.ACCELERATION * 3600 / 1000;
        }

        public static float GetBrakingDistance(float speed)
        {
            float speedMS = speed * 1000 / 3600;
            return (speedMS * speedMS) / (2 * VehicleConsts.ACCELERATION);
        }

        public static float GetTranslation(float oldSpeed, float newSpeed, float deltaTime)
        {
            float newSpeedMS = newSpeed * 1000 / 3600;
            float oldSpeedMS = oldSpeed * 1000 / 3600;
            return deltaTime / 2 * (newSpeedMS + oldSpeedMS);
        }

        // distance [m]; time [s]; result [km/h]
        public static float GetMaxSpeed(float distanceToNextStop, float timeToNextStop)
        {
            return distanceToNextStop / timeToNextStop * 3600 / 1000;
        }
    }
}
