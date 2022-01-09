using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    public class Emitter
    {
        //Притяжение по Х
        public float GravitationX = 0;
        //Притяжение по У
        public float GravitationY = 1;

        //Позиция мышки по Х
        public int MousePositionX = 0;
        //Позиция мышки по Х
        public int MousePositionY = 0;

        //Кол-во партиклов
        public int ParticlesCount = 0;

        public int X; 
        public int Y; 
        public int Direction = 0; // вектор направления в градусах
        public int Spreading = 360; // разброс частиц относительно Direction
        public int SpeedMin = 1; // начальная минимальная скорость движения частицы
        public int SpeedMax = 10; // начальная максимальная скорость движения частицы
        public int RadiusMin = 2; // минимальный радиус частицы
        public int RadiusMax = 10; // максимальный радиус частицы
        public int LifeMin = 20; // минимальное время жизни частицы
        public int LifeMax = 100; // максимальное время жизни частицы

        //Кол-во создаваемых партиклов в тик
        public int ParticlesPerTick = 100;

        //Начальный цвет
        public Color ColorFrom = Color.White;
        //Конечный цвет
        public Color ColorTo = Color.FromArgb(0, Color.Black);

        //Список точек изменения
        public List<IImpactPoint> impactPoints = new List<IImpactPoint>();

        //Список частиц
        public List<Particle> particles = new List<Particle>();
        //Обновление состояния частиц
        public void UpdateState()
        {
            //Кол-во точек для создания
            int particlesToCreate = ParticlesPerTick;
            //Пройтись по листу
            foreach (var particle in particles)
            {
                if (particle.Life > 0)
                {
                    particle.Life--;
                }
                if (particle.Life <= 0)
                {
                    if (particlesToCreate > 0)
                    {
                        particlesToCreate -= 1;
                        ResetParticle(particle);
                    }
                }
                else
                {
                    //Сдвинуть по Х
                    particle.X += particle.SpeedX;
                    //Сдвинуть по У
                    particle.Y += particle.SpeedY;
                    foreach (var point in impactPoints)
                    {
                        //Отправть частицу в точку
                        point.ImpactParticle(particle);
                    }
                    //Свинуть скорость по гравитации
                    particle.SpeedX += GravitationX;
                    particle.SpeedY += GravitationY;
                }
            }
            // этот новый цикл также будет срабатывать только в самом начале работы эмиттера
            // собственно пока не накопится критическая масса частиц
            while (particlesToCreate >= 1)
            {
                //Уменьшить кол-во создаваемых частиц
                particlesToCreate -= 1;
                //Создать частицу
                var particle = CreateParticle();
                //Сбросить частицу
                ResetParticle(particle);
                //Добавить частицу
                particles.Add(particle);
            }
        }
        //Нарисовать частицу
        public void Render(Graphics g)
        {
            //Нарисовать частицу
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
            //Нарисовать точку
            foreach (var point in impactPoints)
            {
                point.Render(g);
            }
        }
        //Сбосить частицу
        public virtual void ResetParticle(Particle particle)
        {
            //Случайное значение жизней
            particle.Life = Particle.rand.Next(LifeMin, LifeMax);

            //Установить Х
            particle.X = X;
            //Установить У
            particle.Y = Y;

            //задать направление
            var direction = Direction
                + (double)Particle.rand.Next(Spreading)
                - Spreading / 2;
            //задатть скорость
            var speed = Particle.rand.Next(SpeedMin, SpeedMax);
            //задать скорость по Х
            particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
            //задать скорость по У
            particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);
            //задать радиус
            particle.Radius = Particle.rand.Next(RadiusMin, RadiusMax);
            //привести частицу в цветную
            var p = particle as ParticleColorful;
            //задать ей цвет
            p.FromColor = ColorFrom;
        }
        //создание частицы
        public virtual Particle CreateParticle()
        {
            //создать новую частицу
            var particle = new ParticleColorful();
            //задать начальный цвет
            particle.FromColor = ColorFrom;
            //задать конечный цвет
            particle.ToColor = ColorTo;
            //вернуть частицу
            return particle;
        }
    }
    public class TopEmitter : Emitter
    {
        //Ширина распыления
        public int Width; 
        //Перезагрузка партикла
        public override void ResetParticle(Particle particle)
        {
            //вызвать базовую ресет
            base.ResetParticle(particle);

            // позиция X -- произвольная точка от 0 до Width
            particle.X = Particle.rand.Next(Width);
            // ноль -- это верх экрана 
            particle.Y = 0;
            // падаем вниз по умолчанию
            particle.SpeedY = 1;
            // разброс влево и вправа у частиц 
            particle.SpeedX = Particle.rand.Next(-2, 2); 
        }
    }
}
