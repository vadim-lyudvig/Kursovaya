using System;
using System.Drawing;

namespace Курсовая
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;
        public Color color;

        public abstract void ImpactParticle(Particle particle);
        // базовый класс для отрисовки точки
        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(color),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }
    //Точка подсчета числа частиц
    public class CountPoint : IImpactPoint
    {
        //Радиус
        public int Power = 100;
        //Число частиц
        public int count = 0;
        //Действие точки
        public override void ImpactParticle(Particle particle)
        {
            //Расчитать вектор по Х
            float gX = X - particle.X;
            //Расчитать вектор по У
            float gY = Y - particle.Y;
            //Найти евклидово расстояние
            double r = Math.Sqrt(gX * gX + gY * gY);
            //Если частица внутри точки
            if (r + particle.Radius < Power / 1.8 && particle.Life > 0)
            {
                //Убить частицу
                particle.Life = 0;
                //Увеличить кол-во на 1
                count++;
            }
        }
        //Нарисовать точку
        public override void Render(Graphics g)
        {
            this.color = Color.OrangeRed;
            //Нарисовать точку
            g.FillEllipse(
                   new SolidBrush(color),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
               );
            //Вывести информацию
            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(
                count.ToString(),
                new Font("Verdana", 10),
            new SolidBrush(Color.White),
            X,
            Y,
            stringFormat
                );
        }
    }
    public class ColorPoint : IImpactPoint
    {
        //Установить радиус
        public int Power = 100;
        public override void ImpactParticle(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double r = Math.Sqrt(gX * gX + gY * gY);
            //Если частица внутри точки
            if (r + particle.Radius < Power / 1.7 && particle.Life > 0)
            {
                //Перекрасить частицу
                (particle as ParticleColorful).FromColor = color;
            }
        }

        public override void Render(Graphics g)
        {
            //Нарисовать точку
            g.DrawEllipse(
                   new Pen(color, 3),
                   X - Power / 2,
                   Y - Power / 2,
                   Power,
                   Power
               );
        }
    }
}
