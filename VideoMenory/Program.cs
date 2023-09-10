using System;
using OpenHardwareMonitor.Hardware;

namespace GPUHealthChecker
{
    class Program
    {
        static Computer computer;

        static void Main(string[] args)
        {
            // Создаем экземпляр компьютера
            computer = new Computer();
            // Настройка для получения данных о видеокарте
            computer.GPUEnabled = true;
            computer.Open();

            // Получаем первую видеокарту в системе
            var gpu = computer.Hardware[0];

            // Выводим информацию о видеокарте
            Console.WriteLine("Модель видеокарты: " + gpu.Name);
            Console.WriteLine("Температура: " + GetGPUTemperature(gpu) + "°C");
            Console.WriteLine("Скорость вентилятора: " + GetGPUFanSpeed(gpu) + " RPM");

            // Закрываем соединение с компьютером
            computer.Close();

            Console.WriteLine("Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        static float GetGPUTemperature(IHardware gpu)
        {
            gpu.Update();
            foreach (var sensor in gpu.Sensors)
            {
                if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                {
                    return sensor.Value ?? 0;
                }
            }
            return 0;
        }

        static int GetGPUFanSpeed(IHardware gpu)
        {
            gpu.Update();
            foreach (var sensor in gpu.Sensors)
            {
                if (sensor.SensorType == SensorType.Fan && sensor.Name == "GPU Fan")
                {
                    return (int)(sensor.Value ?? 0);
                }
            }
            return 0;
        }
    }
}