using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Commands;
using Rhino.FileIO;
using Rhino.PlugIns;
using Rhino.Collections;

namespace Animate
{
    public class Animate_geometries : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public Animate_geometries()
          : base("animate", "animate",
              "Description",
              "Animate", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometries", "geometries ", "gh", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Capture", "Capture", "capture the geometry", GH_ParamAccess.item , false);
            pManager.AddNumberParameter("m", "Motion", "Motions", GH_ParamAccess.item , 0.5);
            pManager.AddBooleanParameter("Reset", "R", "reset captures", GH_ParamAccess.item , false);

            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {

            pManager.AddGeometryParameter("Geometry", "Geometry", "Geometry", GH_ParamAccess.list);
            




        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        /// 
        List<List<GeometryBase>> geos = new List<List<GeometryBase>>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GeometryBase> geo = new List<GeometryBase>();
            DA.GetDataList(0, geo);

            bool capture = false;
            DA.GetData(1, ref capture);
            
            //if (geos.Count == 0)
            //{
            //    for(int i = 0; i < geo.Count; i++)
            //    {
            //        List<GeometryBase> temp_list = new List<GeometryBase>() { geo[i] };
            //        geos.Add(temp_list);
            //    }

            //}
            if (capture)
            {
                for (int i = 0; i < geo.Count; i++)
                {
                    geos[i].Add(geo[i]);

                }
            }
            bool reset = false;
            DA.GetData(3, ref reset);

            if (reset)
            {
                geos = new List<List<GeometryBase>>();
                for (int i = 0; i < geo.Count; i++)
                {
                    List<GeometryBase> temp_list = new List<GeometryBase>() ;
                    geos.Add(temp_list);
                }
            }


            double t = 0;
            DA.GetData(2, ref t);
            List<Curve> curvs = new List<Curve>();
            List<GeometryBase> out_geo = new List<GeometryBase>();
            List<Plane> plss = new List<Plane>();
            for (int n = 0;n < geo.Count; n++) {
                List<GeometryBase> gm = geos[n];


            List<Plane> planes = new List<Plane>();
                if (gm.Count == 0)
                {
                    continue;
                }
            for (int i = 0; i < gm.Count; i++)
            {
                Plane pp = extractPlane(gm[i]);
                planes.Add(pp);

            }
            
            
           // DA.GetDataList(0, planes);

            //List<Point3d> pts = new List<Point3d>();
            //List<Vector3d> Us = new List<Vector3d>();
            //List<Vector3d> Vs = new List<Vector3d>();
            //List<Vector3d> Zs = new List<Vector3d>();
            //for(int i=0; i < planes.Count; i++)
            //{
            //    Point3d  pt = planes[i].Origin;
            //    Vector3d U = planes[i].XAxis;
            //    Vector3d V = planes[i].YAxis;
            //    Vector3d Z = planes[i].Normal;

            //    pts.Add(pt);
            //    Us.Add(U);
            //    Vs.Add(V);
            //    Zs.Add(Z);
            //}


                //generate base Curve 

            //    Curve cur = Curve.CreateInterpolatedCurve(pts,2, CurveKnotStyle.Chord);
            //    curvs.Add(cur);

            //    //find point

            //    Point3d interpt = cur.PointAtNormalizedLength(t);
            //List<double> numbers = new List<double>();
            //List<int> highest = new List<int>();
            //List<int> lowest= new List<int>();

                //find the current points boundary (i mean lowest nearest and highest ef

               
            //for (int i = 0; i < pts.Count; i++)
            //{
            //    Point3d test_pt = pts[i];

            //    double test_t = -0.001;
            //    cur.ClosestPoint(test_pt, out test_t);

            //    if (test_t >= t)
            //    {
            //        highest.Add(i);
            //    }
            //    if(test_t<=t)
            //    {
            //        lowest.Add(i);
            //    }
            //    numbers.Add(test_t);

            // }
            //int LowestIndx = lowest[lowest.Count - 1];
            //int HighestIndx = highest[0];


            ////generate tween crvs 

            ////find parameters 
            //double highest_P = numbers[HighestIndx];
            //double lowest_P = numbers[LowestIndx];

            //double proportion = Remap(t, lowest_P, highest_P, 0, 1);

            //Vector3d tweenU = Us[HighestIndx] * proportion + Us[LowestIndx] * (1 - proportion);
            //Vector3d tweenV = Vs[HighestIndx] * proportion + Vs[LowestIndx] * (1 - proportion);
            //Vector3d tweenZ = Zs[HighestIndx] * proportion + Zs[LowestIndx] * (1 - proportion);

            //Plane newpl = new Plane(interpt, tweenU, tweenV);

                Plane newpl = pp(planes , t);

                plss.Add(newpl);
                //Rhino.Geometry.Transform orient = Rhino.Geometry.Transform.ChangeBasis(planes[0], newpl);
                var orient = Transform.PlaneToPlane(planes[0], newpl);
            var new_geo = gm[0].Duplicate();
            new_geo.Transform(orient);

                out_geo.Add(new_geo);
            }

            DA.SetDataList(0, out_geo);
            
 

        }

        public static double Remap(double val, double from1, double to1, double from2, double to2)
        {


            double remapped = (val - from1) / (to1 - from1) * (to2 - from2) + from2;
            if (val > to1) { remapped = to2; }
            if (val < from1) { remapped = from2; }
            return remapped;
        }
        public Plane extractPlane(GeometryBase c)
        {
            Plane pl = new Plane();
            var ewe = c.HasBrepForm;
            if (ewe)
            {
                
               // Brep a = Brep.TryConvertBrep(c);
                var n = ((Brep)c).Surfaces;
                var x = n[0];
                Point3d p;
                Vector3d[] vv;
                
                var ss = x.Domain(0);
                var s2 = x.Domain(1);

               
                Vector3d vect_norm  =x.NormalAt(0.5, 0.5);
                x.Evaluate(ss.Mid, s2.Mid, 1, out p, out vv);
                pl = new Plane(p, vv[0], vv[1]);
                //pl = new Plane(p, vect_norm);
                

            }
            else
            {
                
                Vector3d ee = ((Mesh)c).FaceNormals[0];
                Point3d ew = ((Mesh)c).Vertices[0];
                pl = new Plane(ew, ee);

            }
            return pl;
        }

        public Plane pp(List<Plane> pls , double t)
        {
            List<Point3d> pts = new List<Point3d>();
            List<Point3d> Us = new List<Point3d>();
            List<Point3d> Vs = new List<Point3d>();
            List<Point3d> Zs = new List<Point3d>();
            for (int i = 0; i < pls.Count; i++)
            {
                Point3d pt = pls[i].Origin;
                Vector3d U = pls[i].XAxis;
                Vector3d V = pls[i].YAxis;
                Vector3d Z = pls[i].Normal;

                Point3d Upt = new Point3d(U);
                Point3d Vpt = new Point3d(V);
                Point3d Zpt = new Point3d(Z);

                pts.Add(pt);
                Us.Add(Upt);
                Vs.Add(Vpt);
                Zs.Add(Zpt);
            }


            Curve aa = Curve.CreateInterpolatedCurve(pts, 3);
            Curve bb = Curve.CreateInterpolatedCurve(Us, 3);
            Curve cc = Curve.CreateInterpolatedCurve(Vs, 3);
            Vector3d bbp = new Vector3d(0, 0, 0);
            Vector3d ccp = new Vector3d(0, 0, 0);
            var lenb = bb.GetLength();
            if (lenb < 0.01)
            {

                bbp = pls[0].XAxis;
            }
            else
            {
                bbp = new Vector3d(bb.PointAtNormalizedLength(t));
            }
            var lenc = cc.GetLength();
            if (lenc < 0.01)
            {

                ccp = pls[0].YAxis;
            }
            else
            {
                ccp = new Vector3d(cc.PointAtNormalizedLength(t));
            }


            var b = bb.PointAtNormalizedLength(t);
            var c = cc.PointAtNormalizedLength(t);
            var a = aa.PointAtNormalizedLength(t);
            Plane np = new Plane(aa.PointAtNormalizedLength(t), bbp, ccp);
            return np;
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

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            for(int i = 0; i < geos.Count; i++)
            {
                for(int j = 0; j < geos[i].Count; j++)
                {
                    Brep a = (Brep.TryConvertBrep(geos[i][j]));
                    var sampe = a.DuplicateVertices();
                    Point3d aa = sampe[0];
                    
                    args.Display.DrawBrepWires(a, System.Drawing.Color.Red, 2);
                    Rhino.Display.Text3d efew = new Rhino.Display.Text3d(i.ToString());
                    
                    

                }


            }
            

        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c4b4cd3b-7072-4bcc-91ec-af18e9fb87e9"); }
        }
    }
}