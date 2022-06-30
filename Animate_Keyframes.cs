using System;
using System.Collections;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using System.IO;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.InteropServices;

using Rhino.DocObjects;
using Rhino.Collections;
using GH_IO;
using GH_IO.Serialization;

namespace Animate
{
    public class Animate_Keyframes : GH_Component
    {
        
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Animate_Keyframes()
          : base("Animate_Keyframe", "Animate_Keyframe",
              "Description",
              "Animate", "Keyframe")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
       
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Keyframes", "Keyframes", "generateKeyfraem", GH_ParamAccess.tree);
            pManager.AddNumberParameter("motion", "motion", "motion", GH_ParamAccess.item, 0.2);
         
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        
          


        }
        List<PolylineCurve> test_crv = new List<PolylineCurve>();
        double motion = 0;
        Grasshopper.DataTree<Point3d>keyframe = new Grasshopper.DataTree<Point3d>();
        Grasshopper.DataTree<double> parametrs = new Grasshopper.DataTree<double>();
     

        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
          
      
           
            DA.GetData(1, ref motion);


            Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Point> Keyframe = new Grasshopper.Kernel.Data.GH_Structure<Grasshopper.Kernel.Types.GH_Point>();

            if (!DA.GetDataTree<Grasshopper.Kernel.Types.GH_Point>(0, out Keyframe)) return;


            if (true) { 
                for (int i = 0; i < Keyframe.Branches.Count; i++)
                {
                    Grasshopper.Kernel.Data.GH_Path new_path = new Grasshopper.Kernel.Data.GH_Path(i);
                    for(int j = 0; j < Keyframe.Branches[i].Count;j++) {
                        Point3d a =Keyframe.Branches[i][j].Value;
                        keyframe.Add(a, new_path);
                    }
                
                }
            }


            if(true)
            OnPingDocument().ScheduleSolution(10, SolutionCallback);
           
          
;         
            

        }

        //Document.SolutionEnd += handle;



         
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        /// 

        private void SolutionCallback(GH_Document doc)
        {
            List<Grasshopper.Kernel.Special.GH_NumberSlider> sliders
     = FindObjectsOfTypeInCurrentGroup<Grasshopper.Kernel.Special.GH_NumberSlider>();

            for (int i = 0; i < sliders.Count; i++)
            {
                
                PolylineCurve pa = new PolylineCurve(keyframe.Branch(i));
                //Curve aa = Curve.CreateInterpolatedCurve(keyframe.Branch(i), 3 , CurveKnotStyle.ChordPeriodic);
                test_crv.Add(pa);
                Curve aa = pa.ToNurbsCurve();
                Point3d eval = new Point3d(motion , 0,0);
              
                //Rhino.Geometry.Intersect.Intersection.//(new(Point3d(motion , 0 ,0)) , new Vector3d(1,0,0)) 
                //var evants = Rhino.Geometry.Intersect.Intersection.CurveLine(aa, new Line(eval, new Point3d(motion, 10000000, 0)), 0.001, 0.001);
                var evants = Rhino.Geometry.Intersect.Intersection.CurvePlane(aa, new Plane(eval, new Vector3d(1, 0, 0)), 0.001);
                var ccx_event = evants[0];


                Point3d resu = ccx_event.PointA;
                double slider_val = resu.Y;
                //Interpolator a = new Interpolator(parametrs.Branch(i));
                sliders[i].Slider.RaiseEvents = false;
                //double result = a.InterpolateCatmullRom(motion);
                //result = slider_val;
                sliders[i].SetSliderValue((decimal)slider_val);

                sliders[i].Slider.RaiseEvents = true;

                sliders[i].ExpireSolution(false);
                
            }

        }


        private IGH_DocumentObject[] AllCanvasObjects()
        {
            var doc = OnPingDocument();
            if (doc == null)
                return new IGH_DocumentObject[0];
            return doc.Objects.ToArray();
        }
        private List<T> FindObjectsOfTypeInCurrentGroup<T>() where T : Grasshopper.Kernel.IGH_ActiveObject
        {
            //Grasshopper.Kernel.Special.GH_Group test;
            //test. 


            List<Grasshopper.Kernel.Special.GH_Group> groups = AllCanvasObjects().OfType<Grasshopper.Kernel.Special.GH_Group>()
              .Where(gr => gr.ObjectIDs.Contains(InstanceGuid))
              .ToList<Grasshopper.Kernel.Special.GH_Group>();

            List<T> output = groups.Aggregate(new List<T>(), (list, item) =>
            {
                list.AddRange(
                  AllCanvasObjects().OfType<T>()
                  .Where(obj => item.ObjectIDs.Contains(obj.InstanceGuid))
                  );
                return list;
            }).Distinct().ToList();

            return output;

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
            get { return new Guid("18a35849-dd05-4139-90e8-acc74c565b04"); }
        }
    }
}