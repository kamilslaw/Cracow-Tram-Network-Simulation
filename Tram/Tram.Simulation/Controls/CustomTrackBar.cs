using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tram.Simulation.Controls
{
    public partial class CustomTrackBar : UserControl
    {
        private ToolTip toolTip;

        private int firstValue = 1;
        public int FirstValue
        {
            get { return firstValue; }
            set { if (value < 0 || firstValue >= lastValue) firstValue = 0; else firstValue = value; }
        }

        private int lastValue = 10;
        public int LastValue
        {
            get { return lastValue; }
            set { if (value <= FirstValue) lastValue = FirstValue + 1; else lastValue = value; }
        }

        private int val = 1;
        public int Value
        {
            get { return val; }
            set { if (value < FirstValue) val = FirstValue; else if (value > LastValue) val = LastValue; else val = value; }
        }

        public int Range { get { return LastValue - FirstValue + 1; } }
        
        public int BarHeight { get; set; } = 5;

        public int PointRadius { get; set; } = 15;

        public Color BarColor { get; set; } = ColorTranslator.FromHtml("#1bceb8");

        public Color ShadowColor { get; set; } = ColorTranslator.FromHtml("#dbeff3"); 

        public Color PointerColor { get; set; } = ColorTranslator.FromHtml("#dfede8");

        public Color PointerShadowColor { get; set; } = Color.DodgerBlue;

        public event EventHandler<int> OnBarValueChanged;

        public CustomTrackBar()
        {
            InitializeComponent();
            toolTip = new ToolTip();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Brush barShadowBrush = new SolidBrush(ShadowColor);
            Brush barBrush = new SolidBrush(BarColor);
            Brush pointerBrush = new SolidBrush(PointerColor);
            Brush pointerShadowBrush = new SolidBrush(Color.FromArgb(80, PointerShadowColor));

            e.Graphics.FillRectangle(barShadowBrush,
                new Rectangle(e.ClipRectangle.X + Margin.Left,
                              e.ClipRectangle.Y + e.ClipRectangle.Height / 2 - BarHeight / 2 - 1,
                              e.ClipRectangle.Width - Margin.Left - Margin.Right,
                              BarHeight + 2));

            e.Graphics.FillRectangle(barBrush,
                new Rectangle(e.ClipRectangle.X + Margin.Left + 1,
                              e.ClipRectangle.Y + e.ClipRectangle.Height / 2 - BarHeight / 2,
                              e.ClipRectangle.Width - Margin.Left - Margin.Right - 2,
                              BarHeight));

            int position = Value - FirstValue;
            e.Graphics.FillEllipse(pointerShadowBrush,
                new Rectangle(e.ClipRectangle.X + e.ClipRectangle.Width / Range * position + e.ClipRectangle.Width / Range / 2 - PointRadius / 2 - 1,
                              e.ClipRectangle.Y + e.ClipRectangle.Height / 2 - PointRadius / 2 - 1,
                              PointRadius + 2,
                              PointRadius + 2));
            e.Graphics.FillEllipse(pointerBrush,
                new Rectangle(e.ClipRectangle.X + e.ClipRectangle.Width / Range * position + e.ClipRectangle.Width / Range / 2 - PointRadius / 2,
                              e.ClipRectangle.Y + e.ClipRectangle.Height / 2 - PointRadius / 2,
                              PointRadius,
                              PointRadius));
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Value = e.X * Range / Width + FirstValue;
                Refresh();
                toolTip.SetToolTip(this, Value.ToString());
                OnBarValueChanged(this, Value);
            }
        }
    }
}
