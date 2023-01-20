using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class krovakConverter
{
    // http://www.ibot.cas.cz/personal/wild/data/WGS_JTSK.pdf
    //constants for BLH_xy
    const double a = 6377397.15508, e = 0.081696831215303, n = 0.97992470462083, cont_u_ro = 12310230.12797036,
        sinUQ = 0.863499969506341, cosUQ = 0.504348889819882, sinVQ = 0.420215144586493, cosVQ = 0.907424504992097,
        alfa = 1.000597498371542, k_2 = 1.00685001861538;
    //constants for BLH_xyz
    const double a1 = 6378137.0, f_1 = 298.257223563;
    //constants for transformCoord
    const double dx = -570.69, dy = -85.69, dz = -462.84;
    const double wz = 5.2611 / 3600 * Math.PI / 180;
    const double wy = 1.58676 / 3600 * Math.PI / 180;
    const double wx = 4.99821 / 3600 * Math.PI / 180;
    const double m = -3.543e-6;
    //constants for xyz_BLH
    const double aO = 6377397.15508, f_1O = 299.152812853;

    const double d2r = Math.PI / 180;


    private double sqr(double x)
    {
        return x * x;
    }
    /// <summary>
    /// Transforms S-JTSK geocoordinates to S-JTSK plane coordinates
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    private Vector3D BLH_XY(double latitude, double longitude)
    {
        double ro, epsilon, D, V, t, sinS, cosS, sinU, cosU, cosDV, sinDV, sinV, cosV, sinB, sinD;
        double B, L, H;
        B = latitude;
        L = longitude;
        sinB = Math.Sin(B);
        t = (1 - e * sinB) / (1 + e * sinB);
        t = sqr(1 + sinB) / (1 - sqr(sinB)) * Math.Exp(e * Math.Log(t));
        t = k_2 * Math.Exp(alfa * Math.Log(t));
        sinU = (t - 1) / (t + 1);
        cosU = Math.Sqrt(1 - sqr(sinU));
        V = alfa * L;
        sinV = Math.Sin(V);
        cosV = Math.Cos(V);
        cosDV = cosVQ * cosV + sinVQ * sinV;
        sinDV = sinVQ * cosV - cosVQ * sinV;
        sinS = sinUQ * sinU + cosUQ * cosU * cosDV;
        cosS = Math.Sqrt(1 - sqr(sinS));
        sinD = sinDV * cosU / cosS;
        D = Math.Atan(sinD / Math.Sqrt(1 - sqr(sinD)));
        epsilon = n * D;
        ro = cont_u_ro * Math.Exp(-n * Math.Log((1 + sinS) / cosS));
        double X = ro * Math.Cos(epsilon);
        double Y = ro * Math.Sin(epsilon);


        return new Vector3D(X, Y, 0);
    }


    /// <summary>
    /// Transforms WGS-84 geodetic coordinates to S-JTSK planar coordinates
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="altitude"></param>
    /// <returns></returns>
    public Vector3D transform(double latitude, double longitude, double altitude)
    {
        double B, L, H;
        B = latitude * d2r;
        L = longitude * d2r;
        H = altitude;
        Vector3D WGSxyz = BLH_xyz(B, L, H);
        Vector3D JTSKxyz = transformCoord(WGSxyz.x, WGSxyz.y, WGSxyz.z);
        Vector3D JTSBLH = xyz_BLH(JTSKxyz.x, JTSKxyz.y, JTSKxyz.z);
        Vector3D JTSxy = BLH_XY(JTSBLH.x, JTSBLH.y);

        return new Vector3D(-JTSxy.x, -JTSxy.y, JTSBLH.z);
    }

    /// <summary>
    /// Transforms geodetic coordinates to right-angled coordinates in WGS-84
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="altitude"></param>
    /// <returns></returns>
    public Vector3D BLH_xyz(double latitude, double longitude, double altitude)
    {
        double ro, e2, x1, y1, z1;
        e2 = 1 - Math.Pow(1 - 1 / f_1, 2);
        ro = a1 / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(latitude), 2));
        x1 = (ro + altitude) * Math.Cos(latitude) * Math.Cos(longitude);
        y1 = (ro + altitude) * Math.Cos(latitude) * Math.Sin(longitude);
        double q = Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(latitude), 2));
        z1 = ((1 - e2) * ro + altitude) * Math.Sin(latitude);
        return new Vector3D(x1, y1, z1);
    }
    /// <summary>
    /// Transforms right-angled coordinates from WGS-84 to S-JTSK
    /// </summary>
    /// <param name="x"> x-coordinate in WGS-84 </param>
    /// <param name="y"> y-coordinate in WGS-84 </param>
    /// <param name="z"> z-coordinate in WGS-84 </param>
    /// <returns></returns>
    private Vector3D transformCoord(double x, double y, double z)
    {
        double xn, yn, zn;
        xn = dx + (1 + m) * (x + wz * y - wy * z);
        yn = dy + (1 + m) * (-wz * x + y + wx * z);
        zn = dz + (1 + m) * (wy * x - wx * y + z);
        return new Vector3D(xn, yn, zn);
    }
    /// <summary>
    /// Transforms S-JTSK right-angled coordinates to S-JTSK geocoordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    private Vector3D xyz_BLH(double x, double y, double z)
    {
        double a_b, e2, theta, st, ct, p, t;
        a_b = f_1O / (f_1O - 1);
        p = Math.Sqrt(sqr(x) + sqr(y));
        e2 = 1 - sqr(1 - 1 / f_1O);
        theta = Math.Atan(z * a_b / p);
        st = Math.Sin(theta);
        ct = Math.Cos(theta);
        t = (z + e2 * a_b * a * sqr(st) * st) / (p - e2 * a * sqr(ct) * ct);
        double B, L, H;
        B = Math.Atan(t);
        L = 2 * Math.Atan(y / (p + x));
        H = Math.Sqrt(1 + sqr(t)) * (p - a / Math.Sqrt(1 + (1 - e2) * sqr(t)));

        return new Vector3D(B, L, H);
    }

}
