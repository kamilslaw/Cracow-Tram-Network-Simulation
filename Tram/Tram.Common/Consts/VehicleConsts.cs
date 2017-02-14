namespace Tram.Common.Consts
{
    public static class VehicleConsts
    {
        public const int MAX_CAPACITY = 200;
                
        // m
        public const int LENGTH = 36;

        // m
        public const float SAFE_SPACE = LENGTH * 1.4f;

        // km/h
        public const float MAX_SPEED = 50f;

        // m/s
        public const float MAX_SPEED_M_S = MAX_SPEED * 1000 / 3600;

        // km/h
        public const float MAX_CROSS_SPEED = 15f;

        // m/s
        public const float MAX_CROSS_SPEED_M_S = MAX_CROSS_SPEED * 1000 / 3600;

        // s
        public const float TIME_TO_MAX_SPEED = 8f;

        // m/s^2
        public const float ACCELERATION = MAX_SPEED_M_S / TIME_TO_MAX_SPEED;

        // m
        public const float DISTANCE_TO_MAX_SPEED = (TIME_TO_MAX_SPEED / 2) * MAX_SPEED_M_S;

        // m
        public const float DISTANCE_TO_MAX_CROSS_SPEED = (MAX_CROSS_SPEED_M_S * MAX_CROSS_SPEED_M_S) / (2 * ACCELERATION);
    }
}
