using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Rhino.DocObjects;
using System.Linq;
using Rhino.Commands;

namespace Animate
{
    public class camera : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public camera()
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
            pManager.AddNumberParameter("Lens Lenght", "Lens", "lens length", GH_ParamAccess.item , 30);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {


        }




        public static event EventHandler<System.EventArgs> Create;


        //Rhino.Display.RhinoViewport e = vv.ActiveViewport;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double focal_length = 0;
            DA.GetData(2, ref focal_length);
            Point3d position = new Point3d(0, 0, 0);
            Point3d target = new Point3d(0, 0, 0);
            DA.GetData(0, ref position);
            DA.GetData(1, ref target);
            //Rhino.Display.RhinoView view;


            var active_view_name = Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.Name;
            //var non_active_views =
            //  Rhino.RhinoDoc.ActiveDoc.Views
            //  .Where(v => v.ActiveViewport.Name != active_view_name)
            //  .ToDictionary(v => v.ActiveViewport.Name, v => v);


   

            //Rhino.RhinoDoc.ActiveDoc.Views.ActiveView = non_active_views[name];
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.SetCameraLocation(position, true);
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.Camera35mmLensLength = focal_length;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.SetCameraDirection(new Vector3d(target - position), true);
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
            get { return new Guid("147bcc65-d859-4f1a-b585-d1dbbe125dbf"); }
        }
    }
}