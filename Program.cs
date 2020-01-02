using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace RectangleCount
{
    enum Quadrilateral
    {
        Parallelgrm,
        HorizRect,
        DiagRect,
        Square,
        HorizSquare,
        DiagSquare
    }
    class Program
    {
        static Random rand = new Random();
        static List<Point> points;
        static int maxXCoord = 9 + 0;
        static int maxYCoord = 5;
        static List<List<StringBuilder>>[] ListOfSbLists;

        static void Main(string[] args)
        {
            int quadrilateralTypesCount = Enum.GetNames(typeof(Quadrilateral)).Length;
            ListOfSbLists = new List<List<StringBuilder>>[quadrilateralTypesCount];
            for (int type = 0; type < quadrilateralTypesCount; type++)
            {
                ListOfSbLists[type] = new List<List<StringBuilder>>();
            }

            int pointsCount = 4 + 14;//rand.Next(25 - 4);
            int[] quadrilateralsCount = new int[quadrilateralTypesCount];

            InitPoints(pointsCount);
            // InitTestPoints(2); pointsCount = points.Count();

            PlotPointsToConsole();

            // Finding all rects
            for (int i = 0; i < pointsCount - 3; i++)
            {
                Point t1 = points[i];// top 1
                for (int j = i + 1; j < pointsCount - 1; j++)
                {
                    Point t2 = points[j];// top 2

                    // top side coord diff
                    int DxT = t2.X - t1.X;
                    int DyT = t2.Y - t1.Y;

                    for (int k = j + 1; k < pointsCount; k++)
                    {
                        Point b1 = points[k];// bottom 1

                        // left side coord diff 
                        int DxL = b1.X - t2.X;
                        int DyL = b1.Y - t2.Y;

                        // check that whether top and left side on the same line
                        if ((DyL != 0 && DyT != 0 && DxL / (double)DyL == DxT / (double)DyT) ||
                            (DxL != 0 && DxT != 0 && DyL / (double)DxL == DyT / (double)DxT))
                            continue;

                        for (int l = j + 1; l < pointsCount; l++)
                        {
                            Point b2 = points[l];// bottom 2
                            // bottom side coord diff  
                            int DxB = b1.X - b2.X;
                            int DyB = b1.Y - b2.Y;
                            // right side coord diff 
                            int DxR = b2.X - t1.X;
                            int DyR = b2.Y - t1.Y;
                            
                            // check for paralellism T-B, L-R   
                            if (DxB == DxT && DyB == DyT && DxR == DxL && DyR == DyL)
                            {
                                for (int type = 0; type < quadrilateralTypesCount; type++)
                                {
                                    if (IsType(type, DxT, DyT, DxL, DyL))
                                    {
                                        if (quadrilateralsCount[type] % 17 == 0)
                                            InitNextListStringBuilder(ListOfSbLists[type]);

                                        int indx = quadrilateralsCount[type] / 17;
                                        ListOfSbLists[type][indx] = PlotRectsToSbList(new List<Point> { t2, t1, b1, b2 }, ListOfSbLists[type][indx], true);
                                        quadrilateralsCount[type]++;
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }

            for (int type = 0; type < quadrilateralTypesCount; type++)
            {
                PlotResults(quadrilateralsCount[type], ListOfSbLists[type], ((Quadrilateral)type).ToString());
            }
        }

        // Initialize list of distinct descending points 
        private static void InitPoints(int pointsCount)
        {
            points = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                Point newP;
                do
                {
                    newP = new Point(rand.Next(maxXCoord + 1), rand.Next(maxYCoord + 1));
                } while (IsDuplicate(newP));

                points.Add(newP);
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();

            //C# 7.0, local function
            bool IsDuplicate(Point newP)
            {
                foreach (var item in points)
                {
                    if (item == newP)
                        return true;
                }
                return false;
            }
        }
        private static void InitNextListStringBuilder(List<List<StringBuilder>> listOfSbLists)
        {
            List<StringBuilder> sbList = new List<StringBuilder>();
            for (int i = 0; i < 5 + maxYCoord; i++)
            {
                sbList.Add(new StringBuilder(" "));
            }
            listOfSbLists.Add(sbList);
        }
        private static void PlotPointsToConsole()
        {
            Console.WriteLine();

            int prevY = points[0].Y;
            List<Point> pointsWithSameY = new List<Point>();
            foreach (var p in points)
            {
                if (p.Y != prevY)
                {
                    WritePointRow(prevY, pointsWithSameY);
                    pointsWithSameY.Clear();
                    prevY = p.Y;
                }
                pointsWithSameY.Add(p);
            }

            WritePointRow(prevY, pointsWithSameY);

            Console.WriteLine();

            PlotPoints(points);

            Console.WriteLine();
        }
        private static void WritePointRow(int prevY, List<Point> pointsWithSameY)
        {
            pointsWithSameY.Reverse();
            StringBuilder pRow = new StringBuilder(prevY + " : ");
            foreach (var pWithSameY in pointsWithSameY)
            {
                pRow.Append(pWithSameY.X + "; ");
            }
            Console.WriteLine(pRow);
        }
        private static void PlotPoints(List<Point> ps)
        {
            string borderStr = GetRowStr('-');
            Console.WriteLine(borderStr);
            int j = 0;
            for (int y = maxYCoord; y >= 0; y--)
            {
                StringBuilder rowStrB = new StringBuilder(GetRowStr(' '));

                GetRowStringBuilder(ref j, y, ps, rowStrB);
                Console.WriteLine(rowStrB);
            }
            Console.WriteLine(borderStr);
            Console.WriteLine();
        }
        private static List<StringBuilder> PlotRectsToSbList(List<Point> ps, List<StringBuilder> sbList, bool IsRotd)
        {
            Point t2 = ps[0];// top-left
            Point t1 = ps[1];// top-right
            Point b1 = ps[2];// bottom-left
            Point b2 = ps[3];// bottom-right

            sbList[0].Append(" (" + t2.X + "," + t2.Y + ")" + "(" + t1.X + "," + t1.Y + ") ");
            sbList[1].Append(" (" + b1.X + "," + b1.Y + ")" + "(" + b2.X + "," + b2.Y + ") ");

            string borderStr = GetRowStr('-');
            sbList[2].Append(borderStr);

            int j = 0;
            for (int y = maxYCoord; y >= 0; y--)
            {
                StringBuilder rowStrB = new StringBuilder(GetRowStr(' '));

                sbList[3 + maxYCoord - y].Append(IsRotd ? GetRowStringBuilder(y, ps, rowStrB) : GetRowStringBuilder(ref j, y, ps, rowStrB));
            }
            sbList[4 + maxYCoord].Append(borderStr);

            return sbList;
        }
        private static string GetRowStr(char chr = ' ')
        {
            return "|" + new String(chr, maxXCoord + 1) + "|"; ;
        }
        private static StringBuilder GetRowStringBuilder(ref int j, int y, List<Point> ps, StringBuilder rowStrB)
        {
            while (j < ps.Count && ps[j].Y == y)
            {
                SetStringBuilder(ps[j], rowStrB);
                j++;
            }
            return rowStrB;
        }
        private static StringBuilder GetRowStringBuilder(int y, List<Point> ps, StringBuilder rowStrB)
        {
            foreach (var p in ps)
            {
                if (p.Y == y)
                {
                    SetStringBuilder(p, rowStrB);
                }
            }
            return rowStrB;
        }
        private static void SetStringBuilder(Point p, StringBuilder sb)
        {
            sb.Remove(p.X + 1, 1);
            sb.Insert(p.X + 1, "+");
        }
        private static void PlotResults(int objCount, List<List<StringBuilder>> listOfSbLists, string type)
        {
            Console.WriteLine();
            Console.WriteLine($"-------------------  {type} No.: {objCount}  -------------------");
            Console.WriteLine();

            if (listOfSbLists.Count > 0 && listOfSbLists[0][0].Length > 1)
                foreach (var SBList in listOfSbLists)
                {
                    foreach (var sb in SBList)
                        Console.WriteLine(sb);

                    Console.WriteLine();
                }
        }
        private static bool IsType(int type, int DxT, int DyT, int DxL, int DyL)
        {
            switch ((Quadrilateral)type)
            {
                case Quadrilateral.Parallelgrm:
                    return true;
                case Quadrilateral.HorizRect:
                    return IsHorizontal();
                case Quadrilateral.DiagRect:
                    return !IsHorizontal() && IsPerpendicular();
                case Quadrilateral.Square:
                    return IsEqualSided() && IsPerpendicular();
                case Quadrilateral.HorizSquare:
                    return IsEqualSided() && IsPerpendicular() && IsHorizontal();
                case Quadrilateral.DiagSquare:
                    return IsEqualSided() && IsPerpendicular() && !IsHorizontal();
                default:
                    return false;
            }

            //C# 7.0, local functions
            bool IsHorizontal()
            {
                return (DyT == 0 && DxL == 0) || (DxT == 0 && DyL == 0);
            }

            bool IsPerpendicular()
            {
                return ((DyL != 0 && DxT != 0 && Math.Abs(DxL / (double)DyL) == Math.Abs(DyT / (double)DxT)) ||
                        (DxL != 0 && DyT != 0 && Math.Abs(DyL / (double)DxL) == Math.Abs(DxT / (double)DyT)));
            }
            
            bool IsEqualSided()
            {
                return Math.Abs(DxT) == Math.Abs(DyL) && Math.Abs(DyT) == Math.Abs(DxL);
            }

        }
        private static void InitTestPoints(int testCase)
        {
            points.Clear();
            switch (testCase)
            {
                case 1:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 3));
                    break;
                case 2:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(1, 3));
                    points.Add(new Point(2, 2));
                    points.Add(new Point(5, 3));
                    points.Add(new Point(4, 2));
                    break;
                case 3:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(1, 2));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(5, 2));
                    points.Add(new Point(4, 1));
                    break;
                case 4:
                    points.Add(new Point(3, 5));
                    points.Add(new Point(5, 5));
                    points.Add(new Point(2, 5));
                    points.Add(new Point(4, 5));

                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));

                    points.Add(new Point(5, 3));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 3));
                    points.Add(new Point(1, 3));
                    break;
                case 5:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(5, 1));
                    points.Add(new Point(3, 1));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(1, 1));
                    break;
                case 6:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(2, 4));
                    points.Add(new Point(4, 4));

                    points.Add(new Point(5, 3));

                    points.Add(new Point(2, 3));
                    break;
                case 7:
                    points.Add(new Point(5, 5));
                    points.Add(new Point(4, 5));
                    points.Add(new Point(3, 5));
                    points.Add(new Point(2, 5));

                    points.Add(new Point(5, 4));
                    points.Add(new Point(4, 4));
                    points.Add(new Point(3, 4));
                    points.Add(new Point(2, 4));

                    points.Add(new Point(5, 3));
                    points.Add(new Point(4, 3));
                    points.Add(new Point(3, 3));
                    points.Add(new Point(2, 3));

                    points.Add(new Point(5, 2));
                    points.Add(new Point(4, 2));
                    points.Add(new Point(3, 2));
                    points.Add(new Point(2, 2));
                    break;

                default:
                    break;
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
        }
    }
}
