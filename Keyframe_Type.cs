using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class KeyframeParam : GH_Param<KeyFrameGoo>
    {
        /// <summary>
        /// Initializes a new instance of the SettingsParam class.
        /// </summary>
        public KeyframeParam() : base(new GH_InstanceDescription("Keyframe", "Keyframe", "Contains a collection of keyframes", "Animate", "shit")) { }

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
            get { return new Guid("0cf2adf9-fc92-4a47-9176-00d364dcd78d"); }
        }
    }
}