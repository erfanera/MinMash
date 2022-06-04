using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GH_IO.Serialization;

namespace Animate
{
    public class keyframeData
    {
        #region Members
            private List<double> mvalues = new List<double>();
            private List<double> mtimes = new List<double>();

        #endregion Members
        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public keyframeData() { }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="ds">settings to copy from</param>
        public keyframeData(keyframeData ds)
        {
            this.mvalues = ds.values;
            this.mtimes = ds.times;

        }
        #endregion Constructors


        #region Properties

        
        public List<double> values
        {
            get { return this.mvalues; }
            set { this.mvalues = value; }
        }

        public List<double> times
        {
            get { return this.mtimes; }
            set { this.mtimes = value; }
        }
        #endregion Properties


 
    }
}
