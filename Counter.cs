using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class Counter : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public Counter()
          : base("Counter", "Counter",
              "Description",
              "Animate", "Utilities")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Run", "Run", "Run", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("speed", "speed", "speed", GH_ParamAccess.item, 0.1);
            pManager.AddNumberParameter("Max", "Max", "Max", GH_ParamAccess.item, 5);
            
            pManager.AddBooleanParameter("Reset", "Reset", "Reset", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("value", "Value", "value", GH_ParamAccess.item);
        }
        double counter;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool rn = false;
            DA.GetData(0, ref rn);
            bool reset = false;
            DA.GetData(3, ref reset);
            double speed = 0.1;
            DA.GetData(1, ref speed);
            double max = 5;
            DA.GetData(2, ref max);


            if (reset)
                counter = 0;
           
            if (rn & counter < max )
            {

                this.ExpireSolution(true);
                this.Message = "Count: " + counter.ToString();
                this.NickName = counter.ToString();
                counter += speed;
            }

            else
                this.Message = "Stopped";
            DA.SetData(0, counter);   

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
            get { return new Guid("72be89a6-61e7-4404-a1a3-a50c83029e80"); }
        }
    }
}