using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RectangleCount
{
    class Program
    {
        static Random rand = new Random();
        static List<Point> points;
        static int maxXCoord = 9 + 0;
        static int maxYCoord = 5;

        static List<List<StringBuilder>> ListOfSbLists = new List<List<StringBuilder>>();
        static void Main(string[] args)
        {
            int pointsCount = 4 + 25;//rand.Next(25 - 4);
            int rectsCount = 0;

            InitDistinctDescendingPoints(pointsCount);



            points.Clear();

            points.Add(new Point(3, 5));
            points.Add(new Point(2, 4));
            points.Add(new Point(4, 4));
            points.Add(new Point(3, 3));
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
            pointsCount = points.Count();

            PlotToConsole();

            InitNextListStringBuilder();

            for (int i = 0; i < pointsCount - 3; i++)
            {
                Point tr = points[i];// top-right
                for (int j = i + 1; j < pointsCount - 2; j++)
                {
                    Point tl = points[j];// top-left
                    if (tr.Y != tl.Y)
                        break;
                    for (int k = j + 1; k < pointsCount - 1; k++)
                    {
                        Point br = points[k];// bottom-right
                        if (tr.X == br.X)
                        {
                            for (int l = k + 1; l < pointsCount; l++)
                            {
                                Point bl = points[l];// bottom-left
                                if (bl.Y == br.Y && bl.X == tl.X)
                                {
                                    rectsCount++;
                                    // Console.WriteLine(tl + " " + tr);
                                    // Console.WriteLine(bl + " " + br);
                                    // PlotRects(new List<Point> { tl, tr, bl, br });
                                    if (rectsCount < 18)
                                    {
                                        ListOfSbLists[0]/*sbList*/  = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[0]/*sbList*/);
                                    }
                                    if (rectsCount == 18)
                                    {
                                        InitNextListStringBuilder();
                                    }
                                    if (rectsCount >= 18 && rectsCount < 36)
                                    {
                                        ListOfSbLists[1]/*sbList*/  = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[1]/*sbList*/);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (ListOfSbLists[0]/*sbList*/[0].Length > 1)
                foreach (var SBList in ListOfSbLists)
                {
                    foreach (var sb in SBList/*sbList*/)
                        Console.WriteLine(sb);

                    Console.WriteLine();
                }


            Console.WriteLine("no of rects: " + rectsCount);


            List<StringBuilder> sbList = new List<StringBuilder>();
            for (int i = 0; i < 5 + maxYCoord; i++)
            {
                sbList.Add(new StringBuilder(" "));
            }

            Console.WriteLine();
            Console.WriteLine("Oblique rectc ");
            rectsCount = 0;
            sbList.Clear();

            for (int i = 0; i < 5 + maxYCoord; i++)
            {
                sbList.Add(new StringBuilder(" "));
            }

            for (int i = 0; i < pointsCount - 3; i++)
            {
                Point tr = points[i];// top-right
                for (int j = i + 1; j < pointsCount; j++)
                {
                    Point tl = points[j];// top-left
                    if (tr.X > tl.X)
                    {
                        // def top side coord diff
                        int DxT = tl.X - tr.X;
                        int DyT = tl.Y - tr.Y;

                        for (int k = j + 1; k < pointsCount; k++)
                        {
                            Point bl = points[k];// bottom-left
                            if (tl.Y > bl.Y)
                            {
                                // def left side coord diff 
                                int DxL = bl.X - tl.X;
                                int DyL = bl.Y - tl.Y;

                                // check for perpendicularity top and left side

                                for (int l = k + 1; l < pointsCount; l++)
                                {
                                    Point br = points[l];// bottom-right
                                    if (br.X < tr.X)
                                    {
                                        // def bottom side coord diff  
                                        int DxB = bl.X - br.X;
                                        int DyB = bl.Y - br.Y;
                                        // def right side coord diff 
                                        int DxR = br.X - tr.X;
                                        int DyR = br.Y - tr.Y;

                                        // check for paralellism T-B, L-R   

                                        if (DxB == DxT && DyB == DyT && DxR == DxL && DyR == DyL)
                                        {
                                            rectsCount++;
                                            if (rectsCount < 12)
                                            {
                                                sbList = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, sbList);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (sbList[0].Length > 1)
                foreach (var sb in sbList)
                    Console.WriteLine(sb);


            Console.WriteLine("No. of oblique rects: " + rectsCount);

        }

        private static void InitNextListStringBuilder()
        {
            List<StringBuilder> sbList = new List<StringBuilder>();
            for (int i = 0; i < 5 + maxYCoord; i++)
            {
                sbList.Add(new StringBuilder(" "));
            }
            ListOfSbLists.Add(sbList);
        }

        private static void InitDistinctDescendingPoints(int pointsCount)
        {
            points = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                Point newP;
                do
                {
                    newP = new Point(rand.Next(maxXCoord + 1), rand.Next(maxYCoord + 1));
                } while (IsDuplicate(points, newP));

                points.Add(newP);
            }
            points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();

        }
        private static bool IsDuplicate(List<Point> points, Point newP)
        {
            foreach (var item in points)
            {
                if (item == newP)
                    return true;
            }
            return false;
        }
        private static void PlotToConsole()
        {
            Console.WriteLine();

            int prevY = points[0].Y;
            List<Point> pointsWithSameY = new List<Point>();
            foreach (var p in points)
            {
                if (p.Y != prevY)
                {
                    PlotPointRow(prevY, pointsWithSameY);
                    pointsWithSameY.Clear();
                    prevY = p.Y;
                }
                pointsWithSameY.Add(p);
            }
            PlotPointRow(prevY, pointsWithSameY);

            PlotRects(points);

            Console.WriteLine();

        }
        private static void PlotPointRow(int prevY, List<Point> pointsWithSameY)
        {
            pointsWithSameY.Reverse();
            StringBuilder pRow = new StringBuilder(prevY + " : ");
            foreach (var pWithSameY in pointsWithSameY)
            {
                pRow.Append(pWithSameY.X + "; ");
            }
            Console.WriteLine(pRow);
        }
        private static void PlotRects(List<Point> ps)
        {
            string borderStr = "|" + new String('-', maxXCoord + 1) + "|";
            Console.WriteLine(borderStr);
            int j = 0;
            for (int i = maxYCoord; i >= 0; i--)
            {
                StringBuilder rowStrB = new StringBuilder("|" + new String(' ', maxXCoord + 1) + "|", maxXCoord + 3);

                while (j < ps.Count && ps[j].Y == i)
                {
                    rowStrB.Remove(ps[j].X + 1, 1);
                    rowStrB.Insert(ps[j].X + 1, "+");
                    j++;
                }
                Console.WriteLine(rowStrB);
            }
            Console.WriteLine(borderStr);
            Console.WriteLine();
        }
        private static List<StringBuilder> PlotRectsToSbList(List<Point> ps, List<StringBuilder> sbList)
        {
            Point tl = ps[0];// top-left
            Point tr = ps[1];// top-right
            Point bl = ps[2];// bottom-left
            Point br = ps[3];// bottom-right

            sbList[0].Append(" (" + tl.X + "," + tl.Y + ")" + "(" + tr.X + "," + tr.Y + ") ");
            sbList[1].Append(" (" + bl.X + "," + bl.Y + ")" + "(" + br.X + "," + br.Y + ") ");

            string borderStr = "|" + new String('-', maxXCoord + 1) + "|";
            sbList[2].Append(borderStr);

            int j = 0;
            for (int i = maxYCoord; i >= 0; i--)
            {
                StringBuilder rowStrB = new StringBuilder("|" + new String(' ', maxXCoord + 1) + "|", maxXCoord + 3);

                while (j < ps.Count && ps[j].Y == i)
                {
                    rowStrB.Remove(ps[j].X + 1, 1);
                    rowStrB.Insert(ps[j].X + 1, "+");
                    j++;
                }
                sbList[3 + maxYCoord - i].Append(rowStrB);
            }
            sbList[4 + maxYCoord].Append(borderStr);

            return sbList;
        }
    }
}
