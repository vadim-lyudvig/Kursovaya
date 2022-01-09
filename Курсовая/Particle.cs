using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    public class Particle
    {
        //Радиус частицы
        public int Radius;
        //расположение по Х
        public float X;
        //расположение по У
        public float Y;

        //Скорость по Х
        public float SpeedX;
        //Скорость по У
        public float SpeedY;

        //Жизни частицы
        public float Life;
        //Можно ли телепортировать частицу
        public bool isTP;

        //Переменная для создания случайных чисел
        public static Random rand = new Random();
        //Конструктор по умолчанию(создание новой частицы)
        public Particle()
        {
            //Направление частницы в градусах
            var direction = (double)rand.Next(360);
            //Начальная скорость
            var speed = 1 + rand.Next(10);

            //Задать скорость по Х
            SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            //Задать скорость по У
            SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

            //Задать случайный радиус
            Radius = 2 + rand.Next(10);
            //Задать случайную жизнь
            Life = 20 + rand.Next(100);
            //Указать возиожность телепортации
            isTP = true;
        }
        //Метод для отрисовки
        public virtual void Draw(Graphics g)
        {
            //Прозрачность в зависимости от жизней
            float k = Math.Min(1f, Life / 100);
            //Прозрачность
            int alpha = (int)(k * 255);
            //Задачть цвет
            var color = Color.FromArgb(alpha, Color.White);
            //Кисть для рисования
            var b = new SolidBrush(color);
            //Нарисовать частицу
            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            //Очистить кисть
            b.Dispose();
        }
    }
     public class ParticleColorful : Particle
     {
        //Начальный цвет
        public Color FromColor;
        //Конечный цвет
        public Color ToColor;

        //Смешиваем цвета
        public static Color MixColor(Color color1, Color color2, float k)
        {
            return Color.FromArgb(
                (int)(color2.A * k + color1.A * (1 - k)),
                (int)(color2.R * k + color1.R * (1 - k)),
                (int)(color2.G * k + color1.G * (1 - k)),
                (int)(color2.B * k + color1.B * (1 - k))
            );
        }
        //Отрисовать
        public override void Draw(Graphics g)
        {
            float k = Math.Min(1f, Life / 100);

            var color = MixColor(ToColor, FromColor, k);
            var b = new SolidBrush(color);

            g.FillEllipse(b, X - Radius, Y - Radius, Radius * 2, Radius * 2);

            b.Dispose();
        }
     }
}
