using Rhino.Geometry;
using System;
using System.Runtime.InteropServices;
using CsBindgen; 

namespace RsPoint
{
    public class RsPointtoRhino
    {
        // メッシュデータをRhinoのメッシュに変換するメソッド
        public static Point3d ToRhinoPoint(IntPtr pptr)
        {
            
            double x = NativeMethods.point3_get_x(pptr);
            double y = NativeMethods.point3_get_y(pptr);
            double z = NativeMethods.point3_get_z(pptr);

            Point3d rhinoPoint = new Point3d(x, y, z);

            NativeMethods.point3_free(pptr);

            return rhinoPoint;
        }
    }
}