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
    public class Elevate : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public Elevate()
          : base("Elevate", "Elevate",
              "Description",
              "Animate", "Objects")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Geometries", "Geometries", "Objects in same branches will move together" , GH_ParamAccess.tree);
            pManager.AddNumberParameter("xp", "XP ", "explosion center", GH_ParamAccess.item, 3);
            pManager.AddIntegerParameter("XYZ", "XYZ", "to elevate on Z=0 , X=1 , Y=2", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Motion", "Motion", "motion", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geometires", "geometires", GH_ParamAccess.tree);
            pManager.AddPointParameter("pt", "Pt", "pt", GH_ParamAccess.item);
            pManager.AddPointParameter("pt", "Pt", "pt", GH_ParamAccess.list);
            pManager.AddBoxParameter("bbox", "gbox", "bobx", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
           
            double motion = 0;
            DA.GetData(3, ref motion);
            Grasshopper.Kernel.Data.GH_Structure < Grasshopper.Kernel.Types.GH_Brep > ctGrid;
            int directoin = 0;
            DA.GetData(2, ref directoin);
            double explosion_center = 0;
            DA.GetData(1, ref explosion_center);

           
            

            if (!DA.GetDataTree <Grasshopper.Kernel.Types.GH_Brep> (0, out ctGrid)) return;
            var ptGrid = ctGrid.Duplicate();
            List<List<Point3d>> bucket = new List<List<Point3d>>();
            List<Point3d> temp = new List<Point3d>();
            
            //find center of the geometry
            
            List<BoundingBox> bblist = new List<BoundingBox>();
            for (int i = 0; i < ptGrid.Branches.Count; i++)
            {
               

                for (int j = 0; j < ptGrid.Branches[i].Count; j++)
                {
                    //BoundingBox bb2 = ptGrid[i][j].Boundingbox;
                    Point3d yo;
                    Brep an = new Brep();
                    GH_Convert.ToBrep_Primary(ptGrid[i][j], ref an);
                    BoundingBox bb2 = an.GetBoundingBox(Plane.WorldXY);
                    bblist.Add(bb2);
                   
                    temp.Add(bb2.Center);
                }
                
                
                
                //bblist.Add(bb);
                
            }
            BoundingBox boundingbox = new BoundingBox(temp);


            for (int i = 0; i < ptGrid.Branches.Count; i++)
            {

                double differenceYb = temp[i].Y - boundingbox.Center.Y;
                double differenceXb = temp[i].X - boundingbox.Center.X;
                double differenceZb = temp[i].Z - boundingbox.Center.Z;
                double differenceZ = temp[i].Z - explosion_center;
                double differenceX = temp[i].X - explosion_center;
                double differenceY = temp[i].Y - explosion_center;
                int swith = 1;
      
                for (int j = 0; j < ptGrid.Branches[i].Count; j++)
                {
                    Transform xform = new Transform();
                    if (directoin == 0)
                    {
                        xform = Transform.Translation(new Vector3d(0, 0, motion * differenceZb));
                    } 
                    if (directoin == 1)
                    {
                        xform = Transform.Translation(new Vector3d(0, motion * differenceYb, 0));
                    }
                    if(directoin==2)
                    {
                        xform = Transform.Translation(new Vector3d(motion * differenceXb, 0, 0));
                    }

                    ptGrid[i][j].Transform(xform);


                }

            }
            DA.SetData(1, boundingbox.Center);
            DA.SetDataTree(0, ptGrid);
            DA.SetDataList(2, temp);
            DA.SetDataList(3, bblist);
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
              .Where(gr => gr.ObjectIDs.Contains(ComponentGuid))
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
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f4600bde-d82a-421f-bbce-82490722f370"); }
        }
    }
}