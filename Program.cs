﻿using System;
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
            int pointsCount = 4 + 6;//rand.Next(25 - 4);
            int rectsCount = 0;

            InitDistinctDescendingPoints(pointsCount);


            // points.Clear();

            // points.Add(new Point(3, 5));

            // points.Add(new Point(2, 5));
            // points.Add(new Point(4, 5));

            // points.Add(new Point(2, 4));
            // points.Add(new Point(4, 4));
            // points.Add(new Point(3, 3));

            // points.Add(new Point(5, 3));
            // points.Add(new Point(4, 2));
            // points = points.OrderByDescending(p => p.Y).ThenByDescending(p => p.X).ToList();
            // pointsCount = points.Count();

            PlotToConsole();

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

                                    if (rectsCount == 1)
                                    {
                                        InitNextListStringBuilder();
                                    }
                                    if (rectsCount < 18)
                                    {
                                        ListOfSbLists[0] = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[0], false);
                                    }
                                    if (rectsCount == 18)
                                    {
                                        InitNextListStringBuilder();
                                    }
                                    if (rectsCount >= 18 && rectsCount < 36)
                                    {
                                        ListOfSbLists[1] = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[1], false);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            PlotRes(rectsCount, "");


            Console.WriteLine();
            Console.WriteLine("Oblique rectc ");
            ListOfSbLists.Clear();
            rectsCount = 0;

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

                                for (int l = j + 1; l < pointsCount; l++)
                                {
                                    Point br = points[l];// bottom-right
                                    // if (br.X < tr.X)
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

                                            if (rectsCount == 1)
                                            {
                                                InitNextListStringBuilder();
                                            }
                                            if (rectsCount < 18)
                                            {
                                                ListOfSbLists[0] = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[0], true);
                                            }
                                            if (rectsCount == 18)
                                            {
                                                InitNextListStringBuilder();
                                            }
                                            if (rectsCount >= 18 && rectsCount < 36)
                                            {
                                                ListOfSbLists[1] = PlotRectsToSbList(new List<Point> { tl, tr, bl, br }, ListOfSbLists[1], true);
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

            PlotRes(rectsCount);
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
        private static List<StringBuilder> PlotRectsToSbList(List<Point> ps, List<StringBuilder> sbList, bool IsRotd)
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
            for (int y = maxYCoord; y >= 0; y--)
            {
                StringBuilder rowStrB = new StringBuilder("|" + new String(' ', maxXCoord + 1) + "|", maxXCoord + 3);

                sbList[3 + maxYCoord - y].Append(IsRotd ? GetRowStrB(y, ps, rowStrB) : GetRowStrB(ref j, y, ps, rowStrB));
            }
            sbList[4 + maxYCoord].Append(borderStr);

            return sbList;
        }
        private static StringBuilder GetRowStrB(ref int j, int y, List<Point> ps, StringBuilder rowStrB)
        {
            while (j < ps.Count && ps[j].Y == y)
            {
                SetStringBuilder(ps[j], rowStrB);
                j++;
            }
            return rowStrB;
        }
        private static StringBuilder GetRowStrB(int y, List<Point> ps, StringBuilder rowStrB)
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
        private static StringBuilder SetStringBuilder(Point p, StringBuilder sb)
        {
            sb.Remove(p.X + 1, 1);
            sb.Insert(p.X + 1, "+");
            return sb;
        }

        private static void PlotRes(int rectsCount, string kind = "oblique")
        {
            if (ListOfSbLists.Count > 0 && ListOfSbLists[0][0].Length > 1)
                foreach (var SBList in ListOfSbLists)
                {
                    foreach (var sb in SBList)
                        Console.WriteLine(sb);

                    Console.WriteLine();
                }

            Console.WriteLine($"No. of {kind} rects: " + rectsCount);
        }
    }
}
