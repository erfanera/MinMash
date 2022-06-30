using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Accord.Extensions.Imaging;
using System.Drawing;
using Accord.Video.FFMPEG;
using System.IO;
namespace Animate

{
    public class VideoMaker : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public VideoMaker()
          : base("Videomaker", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("address", "address", "address", GH_ParamAccess.item);
            pManager.AddTextParameter("t", "t", "t", GH_ParamAccess.item);
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
            string address = "yey";
            DA.GetData(0, ref address);

            string target = "yoy";
            DA.GetData(1, ref target);

           makeAvi(address, target);

            string[] fileArray = Directory.GetFiles(address, "*.jpg");
            //videoMaker("firstTest", fileArray);
        }
        private void makeAvi(string imageInputfolderName, string outVideoFileName, float fps = 12.0f, string imgSearchPattern = "*.png")
        {   // reads all images in folder 
            VideoWriter w = new VideoWriter(outVideoFileName,
                new Accord.Extensions.Size(1280, 720), fps, true);
            Accord.Extensions.Imaging.ImageDirectoryReader ir =
                new ImageDirectoryReader(imageInputfolderName, imgSearchPattern);
            while (ir.Position < ir.Length)
            {
                IImage i = ir.Read();
                w.Write(i);
            }
            w.Close();
        }

        private void videoMaker(string outputFileName, string[] inputImageSequence)
        {
            int width = 1920;
            int height = 1080;
            var framRate = 25;

            using (var vFWriter = new VideoFileWriter())
            {
                // create new video file
                var vw = Accord.Video.FFMPEG.VideoCodec.H264;
                vFWriter.Open(outputFileName, width, height, framRate , vw);

                foreach (var imageLocation in inputImageSequence)
                {
                    Bitmap imageFrame = System.Drawing.Image.FromFile(imageLocation) as Bitmap;
                    vFWriter.WriteVideoFrame(imageFrame);
                }
                vFWriter.Close();
            }
        }

        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\File");
            foreach (var file in files)
            {
                Console.WriteLine(file);
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
            get { return new Guid("011a2a2d-0997-4b0b-8219-3b054bbdaae8"); }
        }
    }
}