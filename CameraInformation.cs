using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class CameraInformation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public CameraInformation()
          : base("Camera Information", "Camera Information",
              "Description",
              "Animate", "Camera")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
           
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("location", "location", "pt", GH_ParamAccess.item);
            pManager.AddPointParameter("target", "target", "target", GH_ParamAccess.item);
            pManager.AddNumberParameter("lens", "lens", "lens", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane test_pl;
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.GetCameraFrame(out test_pl);
            Point3d location = test_pl.Origin;
            Point3d target = test_pl.Origin + test_pl.Normal;
            Rhino.DocObjects.ViewInfo eee = new Rhino.DocObjects.ViewInfo(Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport);
            Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.PushViewInfo(eee, false);
            double cm=Rhino.RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.Camera35mmLensLength;
            DA.SetData(2,  cm);

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
            get { return new Guid("3f8817ec-27d6-48de-a934-893a0d47403a"); }
        }
    }
}