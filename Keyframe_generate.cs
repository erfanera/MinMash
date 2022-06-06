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
    public class Keyframe_generate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public Keyframe_generate()
          : base("Generate_Keyframe", "Generate_Keyframe",
              "Description",
              "Animate", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Reset", "R", "test", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("capture", "capture", " capture", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Animate", "animate", "animate", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("motion", "motion", "motion", GH_ParamAccess.item, 0.2);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Keyframe", "Keyframe", "etw", GH_ParamAccess.list);
            
        }
        List<keyframeData> allpositions = new List<keyframeData>();
        double motion = 0;

        //List<keyframeData> kg = new List<keyframeData>();
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool animate = false;
            DA.GetData(2, ref animate);

            DA.GetData(3, ref motion);
            bool caputer = false;
            DA.GetData(1, ref caputer);
            
            
            bool reset = false;
            DA.GetData(0, ref reset);
            List<Grasshopper.Kernel.Special.GH_NumberSlider> sliders
              = FindObjectsOfTypeInCurrentGroup<Grasshopper.Kernel.Special.GH_NumberSlider>();

            if (reset)
            {
                allpositions = new List<keyframeData>();
                for (int i = 0; i < sliders.Count; i++)
                {
                    double annn = decimal.ToDouble(sliders[i].CurrentValue);
                    var ee = new keyframeData();
                    ee.values.Add(annn);
                    ee.times.Add(0);
                    

                    allpositions.Add(ee);
                }
            }



            if (caputer)
            {
                for (int i = 0; i < sliders.Count; i++)
                {
                    double currentval = decimal.ToDouble(sliders[i].CurrentValue);
                    if (allpositions[i].values.Sum() != 0.0)
                    {
                        sliders[i].Slider.DrawControlBackground = true;
                        sliders[i].Slider.DrawControlBorder = true;

                        sliders[i].Slider.ControlEdgeColour = Color.Blue;
                        sliders[i].Slider.ControlBackColour = Color.Aquamarine;
                        if (allpositions[i].values[allpositions[i].values.Count - 1] != currentval)
                        {



                            sliders[i].Slider.ControlEdgeColour = Color.Black;
                            sliders[i].Slider.ControlBackColour = Color.Yellow;

                        }

                    }
                    allpositions[i].values.Add((currentval));
                }

            }
            // GH_Document.SolutionEndEventHandler handle = null;
            //handle = delegate (Object sender, GH_SolutionEventArgs e) {

            // };


            //Instances.ActiveCanvas.Document.SolutionEnd += handle;
            if (animate)
                OnPingDocument().ScheduleSolution(10, SolutionCallback);
            //GrasshopperDocument.ScheduleSolution(5, SolutionCallback);
            List<KeyFrameGoo> kg = new List<KeyFrameGoo>();
            for(int i = 0; i < allpositions.Count; i++)
            {

                var kgg = new KeyFrameGoo(allpositions[i]);
                kg.Add(kgg);
            }
            DA.SetDataList(0,  kg);

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

                Interpolator a = new Interpolator(allpositions[i].values);
                sliders[i].Slider.RaiseEvents = false;
                double result = a.InterpolateCatmullRom(motion);
                sliders[i].SetSliderValue((decimal)result);

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
            get { return new Guid("78a35849-dd05-4139-90e8-acc74c565b04"); }
        }
    }
}