using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Core.Utility
{
    public class MathLib
    {
        /// <summary>
        /// 두 직선으로 교차점을 구함.
        /// a직선 위의 임의의 2점과
        /// b직선 위의 임의의 2점으로 교점을 찾는다.
        /// </summary>
        /// <param name="ax1">a직선 x1점</param>
        /// <param name="ay1">a직선 y1점</param>
        /// <param name="ax2">a직선 x2점</param>
        /// <param name="ay2">a직선 y2점</param>
        /// <param name="bx1">b직선 x1점</param>
        /// <param name="by1">b직선 y1점</param>
        /// <param name="bx2">b직선 x2점</param>
        /// <param name="by2">b직선 y2점</param>
        /// <param name="x">교차점 x</param>
        /// <param name="y">교차점 y</param>
        public static void GetIntersectPointFrom2Line(
            double ax1, double ay1, double ax2, double ay2,
            double bx1, double by1, double bx2, double by2,
            ref double x, ref double y)
        {
            double fIncrease1 = 0.0, fConstant1 = 0.0, fSameValue1 = 0.0;
            double fIncrease2 = 0.0, fConstant2 = 0.0, fSameValue2 = 0.0;

            if (ax1 == ax2)
                fSameValue1 = ax1;
            else
            {
                fIncrease1 = (double)(ay2 - ay1) / (ax2 - ax1);
                fConstant1 = ay1 - fIncrease1 * ax1;
            }

            if (bx1 == bx2)
                fSameValue2 = bx1;
            else
            {
                fIncrease2 = (double)(by2 - by1) / (bx2 - bx1);
                fConstant2 = by1 - fIncrease2 * bx1;
            }

            if (ax1 == ax2 && bx1 == bx2)
            {
                x = -1;
                y = -1;
            }
            if (ax1 == ax2)
            {
                x = fSameValue1;
                y = fIncrease2 * fSameValue1 + fConstant2;
            }
            else if (bx1 == bx2)
            {
                x = fSameValue2;
                y = fIncrease1 * fSameValue2 + fConstant1;
            }
            else
            {
                x = -(fConstant1 - fConstant2) / (fIncrease1 - fIncrease2);
                y = fIncrease1 * x + fConstant1;
            }
        }

        /// <summary>
        /// 직선의 방정식에서 기울기 a, Y절편 b를 구함. 
        /// </summary>
        /// <param name="x1">첫 번째 X 좌표</param>
        /// <param name="y1">첫 번째 Y 좌표</param>
        /// <param name="x2">두 번째 X 좌표</param>
        /// <param name="y2">두 번째 Y 좌표</param>
        /// <param name="a">기울기</param>
        /// <param name="b">Y 절편</param>
        public static void GetLineCoef(double x1, double y1, double x2, double y2, ref double a, ref double b)
        {
            a = (y2 - y1) / (x2 - x1); // 기울기
            b = y1 - a * x1; // Y 절편
        }

        /// <summary>
        /// y = ax + b 공식에서 y 값을 찾는다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double GetLineYFromX(double a, double b, double x)
        {
            return (a * x) + b;
        }

        /// <summary>
        /// y = ax + b 공식에서 x 값을 찾는다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetLineXFromY(double a, double b, double y)
        {
            return (y - b) / a;
        }

        /// <summary>
        /// 두 직선의 교차점을 찾는다.
        /// </summary>
        /// <param name="AP1"></param>
        /// <param name="AP2"></param>
        /// <param name="BP1"></param>
        /// <param name="BP2"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool GetIntersectPoint(DoublePoint AP1, DoublePoint AP2, DoublePoint BP1, DoublePoint BP2, ref DoublePoint IP)
        {
            double t;
            double s;
            double under = (BP2.y - BP1.y) * (AP2.x - AP1.x) - (BP2.x - BP1.x) * (AP2.y - AP1.y);
            if (under == 0) return false;

            double _t = (BP2.x - BP1.x) * (AP1.y - BP1.y) - (BP2.y - BP1.y) * (AP1.x - BP1.x);
            double _s = (AP2.x - AP1.x) * (AP1.y - BP1.y) - (AP2.y - AP1.y) * (AP1.x - BP1.x);

            t = _t / under;
            s = _s / under;

            if (t < 0.0 || t > 1.0 || s < 0.0 || s > 1.0) return false;
            if (_t == 0 && _s == 0) return false;

            IP.x = AP1.x + t * (double)(AP2.x - AP1.x);
            IP.y = AP1.y + t * (double)(AP2.y - AP1.y);

            return true;
        }

        ////public static void GetCrossPoint(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double cx, out double cy)
        ////{
        ////    cx = ((x1 * y2 - y1 * x2) * (x3 - x4) -
        ////          (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

        ////    cy = ((x1 * y2 - y1 * x2) * (y3 - y4) -
        ////          (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
        ////}

        public static bool AboutEqual(double x, double y)
        {
            double epsilon = Math.Max(Math.Abs(x), Math.Abs(y)) * 1E-15;
            return Math.Abs(x - y) <= epsilon;
        }

        public static double DegToRad(double degree)
        {
            return degree * (Math.PI / 180.0);
        }

        public static double RadToDeg(double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        /// <summary>
        /// 기준 X, Y에서 Edge X, Y 위치의 각도를 구함
        /// </summary>
        /// <param name="refX">기준 좌표 X</param>
        /// <param name="refY">기준 좌표 Y</param>
        /// <param name="edgeX">끝 좌표 X</param>
        /// <param name="edgeY">끝 좌표 Y</param>
        /// <returns>Degree 각도값</returns>
        public static double GetAngle(double refX, double refY, double edgeX, double edgeY)
        {
            var dx = edgeX - refX;
            var dy = edgeY - refY;
            var realAngle = Math.Round((Math.Atan2(dy, dx) * 180.0 / (Math.PI)), 8); // 2사분면 서쪽 방향을 기준으로 시계방향 +로 계산
            return realAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotationRefX">회전 기준 X 좌표</param>
        /// <param name="rotationRefY">회전 기준 Y 좌표</param>
        /// <param name="inputX">입력 X 좌표</param>
        /// <param name="inputY">입력 Y 좌표</param>
        /// <param name="angle">회전 할 각도(Degree)</param>
        /// <param name="ox">회전 후 결과 X 좌표</param>
        /// <param name="oy">회전 후 결과 Y 좌표</param>
        public static void GetXYPositionAfterRotation(double rotationRefX, double rotationRefY, double inputX, double inputY, double angle, out double ox, out double oy)
        {
            ox = (inputX - rotationRefX) * Math.Cos(DegToRad(angle)) - (inputY - rotationRefY) * Math.Sin(DegToRad(angle)) + rotationRefX;
            oy = (inputX - rotationRefX) * Math.Sin(DegToRad(angle)) + (inputY - rotationRefY) * Math.Cos(DegToRad(angle)) + rotationRefY;
        }

        /// <summary>
        /// 3점 좌표를 입력 받아 ccw 방향인지 체크
        /// </summary>
        /// <param name="x1">첫 번째 x 좌표값</param>
        /// <param name="y1">첫 번째 y 좌표값</param>
        /// <param name="x2">두 번째 x 좌표값</param>
        /// <param name="y2">두 번째 y 좌표값</param>
        /// <param name="x3">세 번째 x 좌표값</param>
        /// <param name="y3">세 번째 y 좌표값</param>
        /// <returns>+: 반시계 방향, 0: 평행, -: 시계 방향</returns>
        public static double CheckCcw(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double a = x1 * y2 + x2 * y3 + x3 * y1;
            double b = y1 * x2 + y2 * x3 + y3 * x1;
            return a - b;
        }

        public static bool CheckBitOn(int value, int bitIndex)
        {
            if ((value & (1 << bitIndex)) == 0x01)
            {
                return true;
            }

            return false;
        }

        public static int AddBitValue(int originalValue, int bitIndex)
        {
            originalValue |= (1 << bitIndex);
            return originalValue;
        }

        public static int DelBitValue(int originalValue, int bitIndex)
        {
            originalValue &= ~(1 << bitIndex);
            return originalValue;
        }

        public static int ToggleBitValue(int originalValue, int bitIndex)
        {
            originalValue ^= (1 << bitIndex);
            return originalValue;
        }
    }

    public struct DoublePoint
    {
        public double x;
        public double y;
    }
}
