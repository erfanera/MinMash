using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
namespace Animate
{
    public class Looping_SliderComponent : GH_Param<GH_Number>
    {
        public Looping_SliderComponent() :
          base(new GH_InstanceDescription("Looping Slider", "Nickname", "Description", "Animate", "Input"))
        { }

        public override void CreateAttributes()
        {
            m_attributes = new Looping_SliderComponentAttributes(this);
        }

        protected override Bitmap Icon
        {
            get
            {
                //TODO: return a proper icon here.
                return null;
            }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary | GH_Exposure.obscure;
            }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("11904C26-A453-4331-AE26-E543CCF3968E"); }
        }

        private double m_value = 6;
        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        protected override void CollectVolatileData_Custom()
        {
            VolatileData.Clear();
            AddVolatileData(new Grasshopper.Kernel.Data.GH_Path(0), 0, new GH_Number(Value));
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetDouble("Value", m_value);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            m_value = 0;
            reader.TryGetDouble("Value", ref m_value);
            return base.Read(reader);
        }
    }

    public class Looping_SliderComponentAttributes : GH_Attributes<Looping_SliderComponent>
    {
        public Looping_SliderComponentAttributes(Looping_SliderComponent owner)
          : base(owner)
        {
        }

        public override bool HasInputGrip { get { return false; } }
        public override bool HasOutputGrip { get { return true; } }

        private double mouseX;
        private bool clickingX;
        private bool mouseOverX;
        private double changeX;
        private double startX;
        private Region xRegion;

        //Our object is always the same size, but it needs to be anchored to the pivot.
        protected override void Layout()
        {
            //Lock this object to the pixel grid. 
            //I.e., do not allow it to be position in between pixels.
            Pivot = GH_Convert.ToPoint(Pivot);
            Bounds = new RectangleF(Pivot, new SizeF(200, 30));
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (xRegion.IsVisible(e.CanvasLocation))
                {
                    mouseX = e.CanvasX;
                    clickingX = true;
                    Owner.ExpireSolution(true);
                    return GH_ObjectResponse.Capture;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseMove(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (clickingX)
            {
                changeX = e.CanvasX - mouseX;
                Owner.ExpireSolution(true);
                return GH_ObjectResponse.Ignore;
            }
            if (xRegion.IsVisible(e.CanvasLocation))
            {
                mouseOverX = true;
                return GH_ObjectResponse.Capture;
            }
            if (mouseOverX)
            {
                mouseOverX = false;
                return GH_ObjectResponse.Release;
            }
            return base.RespondToMouseMove(sender, e);
        }

        public override GH_ObjectResponse RespondToMouseUp(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (clickingX)
                {
                    startX += changeX;
                    changeX = 0;
                    clickingX = false;
                    Owner.ExpireSolution(true);
                    return GH_ObjectResponse.Release;
                }
            }
            return base.RespondToMouseUp(sender, e);
        }

        public override void SetupTooltip(PointF point, GH_TooltipDisplayEventArgs e)
        {
            base.SetupTooltip(point, e);
            e.Description = "Double click to set a new integer";
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            if (channel == GH_CanvasChannel.Objects)
            {
                double posX = startX + changeX;
                double myValue = Math.Round(posX / 50, 2);
                Owner.Value = myValue;

                //Drawing tools.
                Brush defaultGray = new SolidBrush(Color.FromArgb(255, 50, 50, 50));

                //Render output grip.
                GH_CapsuleRenderEngine.RenderOutputGrip(graphics, canvas.Viewport.Zoom, OutputGrip, true);

                //Render capsules.
                RectangleF scrollCanvas = new RectangleF(Pivot.X, Pivot.Y, 200, 30);
                GH_Capsule scrollCanvasCapsule = GH_Capsule.CreateCapsule(scrollCanvas, GH_Palette.White, 30, 0);
                scrollCanvasCapsule.Render(graphics, Selected, Owner.Locked, false);
                scrollCanvasCapsule.Dispose();

                RectangleF textCanvas = new RectangleF(Pivot.X + 20, Pivot.Y + 33, 60, 20);
                GH_Capsule textCanvasCapsule = GH_Capsule.CreateCapsule(textCanvas, GH_Palette.White, 10, 0);
                textCanvasCapsule.Render(graphics, Selected, Owner.Locked, false);
                textCanvasCapsule.Dispose();

                //Render graphics.
                RectangleF bounds = new RectangleF(Pivot.X + 20, Pivot.Y + 5, 160, 20);
                GraphicsPath boundsPath = new GraphicsPath();
                boundsPath.AddRectangle(bounds);
                xRegion = new Region(boundsPath);

                for (int xLoop = 0; xLoop < 6; xLoop++)
                {
                    double count = 1 + Math.Floor(Math.Abs(posX) / 100);
                    double pos = (Pivot.X) + (((count * 100) + posX + xLoop * 30) % 180);
                    RectangleF balls = new RectangleF((float)pos, Pivot.Y + 5, 20, 20);
                    graphics.FillEllipse(Brushes.Azure, balls);
                }

                graphics.DrawString(Math.Round(myValue, 2).ToString(), GH_FontServer.Standard, defaultGray, new PointF(Pivot.X + 28, Pivot.Y + 34));

            }
        }
    }
}