using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using GH_IO.Serialization;

namespace Animate
{
    /// <summary>
    /// goo implementation for keyframeData data types
    /// </summary>
    /// <typeparam name="keymask">settings object</typeparam>
    public class KeyFrameGoo : GH_Goo<keyframeData>
    {
        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public KeyFrameGoo()
        {
            this.Value = new keyframeData();

        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="ds">settings to copy from</param>
        public KeyFrameGoo(keyframeData ds)
        {
            if (ds == null)
            {
                ds = new keyframeData();
            }
            this.Value = new keyframeData(ds);
        }

        /// <summary>
        /// make a complete duplicate of this instance
        /// </summary>
        /// <returns>duplicated KeyFrameGoo</returns>
        public KeyFrameGoo DuplicateGoo()
        {
            if (this.Value == null)
            {
                return new KeyFrameGoo();
            }
            else
            {
                keyframeData ds = new keyframeData(this.Value);
                return new KeyFrameGoo(ds);
            }
        }

        /// <summary>
        /// make a complete duplicate of this instance
        /// </summary>
        /// <returns>duplicated KeyFrameGoo</returns>
        public override IGH_Goo Duplicate()
        {
            return DuplicateGoo();
        }
        #endregion

        #region Properties

        /// <summary>
        /// gets a value indicating whether or not the current value is valid
        /// </summary>
        /// <returns>property value</returns>
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// gets a string describing the state of "invalidness". if the instance is valid, then this property should return Nothing or String.Empty
        /// </summary>
        /// <returns>property value</returns>
        public override string IsValidWhyNot
        {
            get
            {
                return base.IsValidWhyNot;
            }
        }

        /// <summary>
        /// creates a string description of the current instance value
        /// </summary>
        /// <returns>property value</returns>
        public override string ToString()
        {
            if (m_value == null)
                return "Null Keyframe object";
            else return "Keyframe object";
        }

        /// <summary>
        /// gets a description of the type of the implementation
        /// </summary>
        /// <returns>property value</returns>
        public override string TypeDescription
        {
            get { return ("Keyframe object"); }
        }

        /// <summary>
        /// gets the name of the type of the implementation
        /// </summary>
        /// <returns>property value</returns>
        public override string TypeName
        {
            get { return "Keyframe object"; }
        }
        #endregion

        #region Casting
        /// <summary>
        /// this function will be called when the local IGH_Goo instance disappears into a user script
        /// </summary>
        /// <returns>returns the DendroMask instance</returns>
        public override object ScriptVariable()
        {
            return Value;
        }

        /// <summary>
        /// attempt a cast to type Q
        /// </summary>
        /// <param name="target">pointer to target of cast</param>
        /// <typeparam name="Q">type to cast to</typeparam>
        /// <returns>true on success, false on failure</returns>
        public override bool CastTo<Q>(ref Q target)
        {
            if (typeof(Q).IsAssignableFrom(typeof(keyframeData)))
            {
                if (Value == null)
                    target = default(Q);
                else
                    target = (Q)(object)Value;
                return true;
            }
            target = default(Q);
            return false;
        }

        /// <summary>
        /// attempt a cast from generic object
        /// </summary>
        /// <param name="source">reference to source of cast</param>
        /// <returns>true on success, false on failure</returns>
        public override bool CastFrom(object source)
        {
            return false;
        }
        #endregion

    }
}