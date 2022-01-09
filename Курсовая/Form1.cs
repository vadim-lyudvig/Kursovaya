using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Курсовая
{
    public partial class Form1 : Form
    {
        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);
            emitter = new TopEmitter
            {
                Width = picDisplay.Width,
                GravitationY = 0.25f
            };
            //Обавить его в список
            emitters.Add(this.emitter);
            //Установить значения переключателей по умолчанию
            comboBox1.SelectedIndex = 0;
            tbTick.Value = emitter.ParticlesPerTick;
            picDisplay.MouseWheel += picDisplay_MouseWheel;
            tickInfo.Text = emitter.ParticlesPerTick.ToString();
        }

        private void picDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            //Изменение размеров с помощью колесика
            //Если выбрана первая вкладка
            if (tabControl1.SelectedIndex == 0)
            {
                //Пройтись по всем точками
                foreach (var p in emitter.impactPoints)
                {
                    //Если точка -- цветная
                    if (p is ColorPoint)
                    {
                        //Найти ее координату х
                        var x = (p as ColorPoint).X - e.X;
                        //Найти ее координату у
                        var y = (p as ColorPoint).Y - e.Y;
                        //найти расстояние
                        double r = Math.Sqrt(x * x + y * y);
                        //Если точка в радиусе
                        if (r <= (p as ColorPoint).Power / 2)
                        {
                            //если колесико сдвинули вниз и радиус все еще больше 30
                            if(e.Delta<0 && (p as ColorPoint).Power > 30)
                            {
                                //уменьшить радиус на 10
                                (p as ColorPoint).Power -= 10;
                            }
                            //иначе увеличить на 10
                            if (e.Delta > 0 && (p as ColorPoint).Power < 300)
                            {
                                (p as ColorPoint).Power += 10;
                            }
                        }
                    }
                }
            }
            //Тоже самое для точки подсчета
            if (tabControl1.SelectedIndex == 1)
            {
                foreach (var p in emitter.impactPoints)
                {
                    if (p is CountPoint)
                    {
                        var x = (p as CountPoint).X - e.X;
                        var y = (p as CountPoint).Y - e.Y;
                        double r = Math.Sqrt(x * x + y * y);
                        if (r <= (p as CountPoint).Power / 2)
                        {
                            if (e.Delta < 0 && (p as CountPoint).Power > 30)
                            {
                                (p as CountPoint).Power -= 10;
                            }
                            if (e.Delta > 0 && (p as CountPoint).Power < 300)
                            {
                                (p as CountPoint).Power += 10;
                            }
                        }
                    }
                }
            }
        }

        //Тик таймера
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Обносить логику эмитерра
            emitter.UpdateState();
            //Нарисовать все элементы
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g);
            }
            //Обновить пикчер бокс
            picDisplay.Invalidate();
            //Убить частицу, если она вне радиуса
            foreach(var p in emitter.particles)
            {
                if(p.X > picDisplay.Width || p.Y > picDisplay.Height)
                {
                    p.Life = 0;
                }
            }
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
        //Клик мыши
        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            //Если левый клик
            if(e.Button == MouseButtons.Left)
            {
                //Если выбраны цветные точки
                if(tabControl1.SelectedIndex == 0)
                {
                    //Найти координаты клика
                    int x = e.X;
                    int y = e.Y;
                    //Кстановить базовый цвет
                    Color color = Color.Red;
                    //пройтись по цветами взависимости от индекса комбобокса
                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            color = Color.Red;
                            break;
                        case 1:
                            color = Color.Navy;
                            break;
                        case 2:
                            color = Color.Yellow;
                            break;
                        case 3:
                            color = Color.Purple;
                            break;
                        case 4:
                            color = Color.Green;
                            break;
                    }
                    //добавить цветную точку по координатами и цвету
                    emitter.impactPoints.Add(new ColorPoint
                    {
                        X = x,
                        Y = y,
                        color = color
                    }) ;
                }
                //Добавить счетчик
                if (tabControl1.SelectedIndex == 1)
                {
                    int x = e.X;
                    int y = e.Y;
                    emitter.impactPoints.Add(new CountPoint
                    {
                        X = x,
                        Y = y
                    });
                }
            }
            //если правый клик
            if(e.Button == MouseButtons.Right)
            {
                //если выбраны цветные точки
                if(tabControl1.SelectedIndex == 0)
                {
                    //пройтись по всем точкам
                    foreach (var p in emitter.impactPoints)
                    {
                        //если точка цветная
                        if (p is ColorPoint)
                        {
                            //найти координты
                            var x = (p as ColorPoint).X - e.X;
                            var y = (p as ColorPoint).Y - e.Y;
                            //вычислить евклидово расстояние
                            double r = Math.Sqrt(x * x + y * y);
                            //если точка в радиусе
                            if (r <= (p as ColorPoint).Power / 2)
                            {
                                //удалить ее
                                emitter.impactPoints.Remove((p as ColorPoint));
                                break;
                            }
                        }
                    }
                }
                //тоже самое для точки подсчета
                if (tabControl1.SelectedIndex == 1)
                {
                    foreach (var p in emitter.impactPoints)
                    {
                        if (p is CountPoint)
                        {
                            var x = (p as CountPoint).X - e.X;
                            var y = (p as CountPoint).Y - e.Y;
                            double r = Math.Sqrt(x * x + y * y);
                            if (r <= (p as CountPoint).Power / 2)
                            {
                                emitter.impactPoints.Remove((p as CountPoint));
                                break;
                            }
                        }
                    }
                }
            }
        }
        //вывести кол-во частиц
        private void tbTick_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = tbTick.Value;
            tickInfo.Text = emitter.ParticlesPerTick.ToString();
        }
    }
}
