using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Animate
{
    public class PrintedPattern : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PrintedPattern()
          : base("PrintedPattern", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("MeshInput", "m", "input mesh", GH_ParamAccess.item);
    

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("MeshOutput", "m", "output mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {




            Mesh i_mesh = new Mesh();
            DA.GetData(0, ref i_mesh);

            // find middle points 
            var normals = i_mesh.Normals;
            var vertices = i_mesh.Vertices;
        
           

             Mesh n_mesh = new Mesh();
            for (int i = 0; i < i_mesh.Faces.Count; i++)
            {
                //find 4 vertices 
                int A = i_mesh.Faces[i].A;
                int B = i_mesh.Faces[i].B;
                int C = i_mesh.Faces[i].C;
                int D = i_mesh.Faces[i].D;

                Mesh temp_mesh = new Mesh();
                
                //create middle between each to seqeunce 
                Point3d AB_pt = new Point3d((vertices[A] + vertices[B])) * 0.5;
                Point3d BC_pt = new Point3d((vertices[B] + vertices[C])) * 0.5;
                Point3d CD_pt = new Point3d((vertices[C] + vertices[D])) * 0.5;
                Point3d DA_pt = new Point3d((vertices[D] + vertices[A])) * 0.5;

         



          


                Point3d center_pt = new Point3d((vertices[A] + vertices[B] + vertices[C] + vertices[D])) / 4;
            
                Vector3d center_N = new Vector3d((normals[A] + normals[B] + normals[C] + normals[D])) / 4;
            

                Vector3d AB_N = new Vector3d((normals[A] + normals[B])) * 0.5;
                Vector3d BC_N = new Vector3d((normals[B] + normals[C])) * 0.5;
                Vector3d CD_N = new Vector3d((normals[C] + normals[D])) * 0.5;
                Vector3d DA_N = new Vector3d((normals[D] + normals[A])) * 0.5;

                //move vertices in normal direction



                //move the interpolated vertices 


                temp_mesh.Vertices.Add(new Point3d(vertices[A]));                           //A     0
                temp_mesh.Vertices.Add(new Point3d(vertices[B]));                           //B     1
                temp_mesh.Vertices.Add(new Point3d(vertices[C]));                           //C     2
                temp_mesh.Vertices.Add(new Point3d(vertices[D]));                           //D     3
                temp_mesh.Vertices.Add(AB_pt);     //AB    4
                temp_mesh.Vertices.Add(BC_pt);     //BC    5
                temp_mesh.Vertices.Add(CD_pt);     //Cd    6
                temp_mesh.Vertices.Add(DA_pt);     //DA    7
                temp_mesh.Vertices.Add(center_pt); //8


       

                temp_mesh.Faces.AddFace(7, 0, 4);
                temp_mesh.Faces.AddFace(8, 7, 4);
                temp_mesh.Faces.AddFace(8, 4, 5);
                temp_mesh.Faces.AddFace(5, 4, 1);
                temp_mesh.Faces.AddFace(6, 7, 8);
                temp_mesh.Faces.AddFace(6, 3, 7);
                temp_mesh.Faces.AddFace(2, 6, 5);
                temp_mesh.Faces.AddFace(6, 8, 5);
               // temp_mesh.Faces.AddFace(6, 7, 8);

                //replace with previous one 

                n_mesh.Append(temp_mesh);
            }
            DA.SetData( 0,n_mesh);
        }


        public static double Remap(double val, double from1, double to1, double from2, double to2)
        {


            double remapped = (val - from1) / (to1 - from1) * (to2 - from2) + from2;
            if (val > to1) { remapped = to2; }
            if (val < from1) { remapped = from2; }
            return remapped;
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
            get { return new Guid("e3c9a87a-94ab-4e0a-a97f-f83c882f8c8d"); }
        }
    }
}