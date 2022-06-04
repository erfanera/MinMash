using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class Explosion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent3 class.
        /// </summary>
        public Explosion()
          : base("Explosion", "Explosion",
              "Create explosion effect",
              "Animate", "Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Input geometry", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Relative", "R", "relatvie", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("Motion", "M", "motion", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

            pManager.AddGeometryParameter("Geometry", "Geometry", "output Geometry", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //input datas 
            List<GeometryBase> geos = new List<GeometryBase>();
            DA.GetDataList(0, geos);
            double motion = 0;
            DA.GetData(2, ref motion);
            List<Point3d> centers = new List<Point3d>();

            for (int i = 0; i < geos.Count; i++)
            {
                BoundingBox bbox = geos[i].GetBoundingBox(true);
                centers.Add(bbox.Center);
                

            }
            bool relative = false;
            DA.GetData(1, ref relative);
 

                BoundingBox bb2 = new BoundingBox();
                for (int i = 0; i < geos.Count; i++)
                {
                    var temp_bbo = geos[i].GetBoundingBox(true);
                    bb2.Union(temp_bbo);
                Vector3d sum_vect = new Vector3d(0, 0, 0);
                if (relative)
                {
                    
                    for (int j = 0; j < geos.Count; j++)
                    {
                        if (i != j)
                        {
                            Vector3d difference = centers[i] - centers[j];
                            sum_vect[0] += difference[0];
                            sum_vect[1] += difference[1];
                            sum_vect[2] += difference[2];
                        }
                    }
                    sum_vect.Unitize();
                   

                }
                else
                {
                    sum_vect = centers[i] - bb2.Center;
                    sum_vect.Unitize();
                }
                geos[i].Translate(sum_vect * motion);
            }
   

            DA.SetDataList(0, geos);
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
            get { return new Guid("147acc65-d859-4f1a-b585-d1dbbe125dbf"); }
        }
    }
}