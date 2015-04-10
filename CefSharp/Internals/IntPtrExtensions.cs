using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Internals
{
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Do an unchecked conversion from IntPtr to int
        /// so overflow exceptions don't get thrown.
        /// </summary>
        /// <param name="intPtr"></param>
        /// <returns></returns>
        public static int CastToInt32(this IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
    }
}
