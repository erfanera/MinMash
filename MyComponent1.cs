using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class MyComponent1 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public MyComponent1()
          : base("Camera", "Camera",
              "Description",
              "Animate", "Camera")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Location", "Loc", "camera location", GH_ParamAccess.item);
            pManager.AddPointParameter("Target", "tar", "target object", GH_ParamAccess.item);
            pManager.AddBooleanParameter("reset", "rest", "egow", GH_ParamAccess.item, false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            
        }

        

        //Rhino.Display.RhinoViewport e = vv.ActiveViewport;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool reset = false;
            DA.GetData(2, ref reset);
            Point3d position = new Point3d(0, 0, 0);
            Point3d target = new Point3d(0, 0, 0);
            DA.GetData(0, ref position);
            DA.GetData(1, ref target);

            Rhino.Display.RhinoViewport a = new Rhino.Display.RhinoViewport ();
            Rhino.Display.RhinoView cd = Rhino.RhinoDoc.ActiveDoc.Views.Add("Hello", Rhino.Display.DefinedViewportProjection.Perspective, System.Drawing.Rectangle.FromLTRB(-500, 500, 500, -500), true);
            if (reset)
            {
                //create a viewport

                // var cd = Rhino.RhinoDoc.ActiveDoc.Views.AddPageView("yo", 100, 200);
                bool df = cd.Maximized;
                a.PopViewProjection();
                a = cd.ActiveViewport;
            }
            //var vp = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport;
            
            //bool dff = cd.TitleVisible;
            
            


            a.SetCameraLocation(position, true);
            a.SetCameraDirection(new Vector3d(target - position), true);
          
            //set new camera
            


            // <custom additional code>
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
       
       
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d03bc7d2-ad06-460a-8a6c-844868d268fd"); }
        }
    }
}