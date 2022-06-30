using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class Section : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent3 class.
        /// </summary>
        public Section()
          : base("Section", "Section",
              "Description",
              "Animate", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("gew", "eg", "gwe", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "pl", "Pl", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Plane cplane = new Plane();
            DA.GetData(1, ref cplane);

            List<GeometryBase> gt = new List<GeometryBase>();
            List<Guid> ie = new List<Guid>();
            DA.GetDataList(0, ie);

            
            for (int i = 0; i < ie.Count; i++)
            {
                var cp = new Rhino.DocObjects.ObjRef(ie[i]);
                var source = cp.ClippingPlaneSurface().Plane;
                Rhino.RhinoDoc.ActiveDoc.Objects.Transform(ie[i], Transform.PlaneToPlane(source, cplane), true);
            }
            
            

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
            get { return new Guid("df798249-db5e-4843-a1d8-220388442ab9"); }
        }
    }
}