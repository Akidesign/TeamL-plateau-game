using System;

public class CoordinateUtil
{
    // GRS80 Ellipsoid
    private const double a = 6378137d;
    private const double F = 298.257222101d;

    // 平面直角座標系のX軸上における縮尺係数
    private const double m0 = 0.9999d;

    private const double n = 1d / (2d * F - 1d);

    // Geographic -> Plane Rectangular
    private const double a1 = 1d * n / 2d - 2d * n * n / 3d + 5d * n * n * n / 16d + 41d * n * n * n * n / 180d - 127d * n * n * n * n * n / 288d;
    private const double a2 = 13d * n * n / 48d - 3d * n * n * n / 5d + 557d * n * n * n * n / 1440d + 281d * n * n * n * n * n / 630d;
    private const double a3 = 61d * n * n * n / 240d - 103d * n * n * n * n / 140d + 15061d * n * n * n * n * n / 26880d;
    private const double a4 = 49561d * n * n * n * n / 161280d - 179d * n * n * n * n * n / 168d;
    private const double a5 = 34729d * n * n * n * n * n / 80640d;

    private const double A0 = 1d + n * n / 4d + n * n * n * n / 64d;
    private const double A1 = -3d / 2d * (n - n * n * n / 8d - n * n * n * n * n / 64d);
    private const double A2 = 15d / 16d * (n * n - n * n * n * n / 4d);
    private const double A3 = -35d / 48d * (n * n * n - 5d * n * n * n * n * n / 16d);
    private const double A4 = 315d * n * n * n * n / 512d;
    private const double A5 = -693d * n * n * n * n * n / 1280d;

    public static (double x, double y) JGD2011ToPlaneRectCoord(double lat, double lon, double o_lat, double o_lon)
    {
        double latr = lat * Math.PI / 180d; // TO Radian
        double lonr = lon * Math.PI / 180d;
        double o_latr = o_lat * Math.PI / 180d;
        double o_lonr = o_lon * Math.PI / 180d;

        double t = Math.Sinh(Math.Atanh(Math.Sin(latr))
            - 2d * Math.Sqrt(n) / (1d + n) * Math.Atanh(2d * Math.Sqrt(n) / (1d + n) * Math.Sin(latr)));
        double _t = Math.Sqrt(1d + t * t);

        double Lc = Math.Cos(lonr - o_lonr);
        double Ls = Math.Sin(lonr - o_lonr);

        double Xi_ = Math.Atan(t / Lc);
        double Eta_ = Math.Atanh(Ls / _t);

        double _S = m0 * a / (1d + n) * (A0 * o_latr +
            A1 * Math.Sin(2d * o_latr) +
            A2 * Math.Sin(2d * 2d * o_latr) +
            A3 * Math.Sin(2d * 3d * o_latr) +
            A4 * Math.Sin(2d * 4d * o_latr) +
            A5 * Math.Sin(2d * 5d * o_latr));

        double _A = m0 * a / (1d + n) * A0;

        double x = _A * (Xi_ +
            a1 * Math.Sin(2d * 1d * Xi_) * Math.Cosh(2d * 1d * Eta_) +
            a2 * Math.Sin(2d * 2d * Xi_) * Math.Cosh(2d * 2d * Eta_) +
            a3 * Math.Sin(2d * 3d * Xi_) * Math.Cosh(2d * 3d * Eta_) +
            a4 * Math.Sin(2d * 4d * Xi_) * Math.Cosh(2d * 4d * Eta_) +
            a5 * Math.Sin(2d * 5d * Xi_) * Math.Cosh(2d * 5d * Eta_)) - _S;
        double y = _A * (Eta_ +
            a1 * Math.Cos(2d * 1d * Xi_) * Math.Sinh(2d * 1d * Eta_) +
            a2 * Math.Cos(2d * 2d * Xi_) * Math.Sinh(2d * 2d * Eta_) +
            a3 * Math.Cos(2d * 3d * Xi_) * Math.Sinh(2d * 3d * Eta_) +
            a4 * Math.Cos(2d * 4d * Xi_) * Math.Sinh(2d * 4d * Eta_) +
            a5 * Math.Cos(2d * 5d * Xi_) * Math.Sinh(2d * 5d * Eta_));

        return (x, y);
    }
}