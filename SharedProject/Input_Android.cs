#if ANDROID
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework.Input.Touch;
#endif
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class Input_Android : Input
    {
#if ANDROID
        public static Input Input;
        private static float coefficient;
        private static Accelerometer sensor;
        private static float alpha;
        public override void Init()
        {
            base.Init();
            sensor = new Accelerometer();
            sensor.CurrentValueChanged += sensor_CurrentValueChanged;
            sensor.Start();
            alpha = 0.9f;
            vec = Vector2.Zero;
            coefficient = Game1.ScreenSize.X / 2;
        }
        void sensor_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            vec.X = vec.X * alpha - (float)e.SensorReading.Acceleration.X * coefficient * (1 - alpha);
            vec.Y = vec.Y * alpha + (float)e.SensorReading.Acceleration.Y * coefficient * (1 - alpha);
        }
        public override void Update()
        {
            base.Update();
            isTouched = TouchPanel.GetState().Count > 0;
        }
#endif
    }
}
