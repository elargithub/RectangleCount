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
        static int maxCoord = 10;
        static void Main(string[] args)
        {
            int pointsCount = 4 + rand.Next(50 - 4);
            int rectsCount = 0;

            InitDistinctDescendingPoints(pointsCount);

            PlotToConsole();

            for (int i = 0; i < pointsCount - 3; i++)
            {
                Point ru = points[i];// right upper
                for (int j = i + 1; j < pointsCount - 2; j++)
                {
                    Point lu = points[j];//left upper
                    if (ru.Y != lu.Y)
                        break;
                    for (int k = j + 1; k < pointsCount - 1; k++)
                    {
                        Point rl = points[k];// right lower
                        // if (ru.X > rl.X)
                        //     break;
                        // if (ru.X < rl.X)
                        //     continue;

                        if (ru.X == rl.X)
                        {
                            for (int l = k + 1; l < pointsCount; l++)
                            {
                                Point ll = points[l];// left lower
                                if (ll.Y == rl.Y && ll.X == lu.X)
                                {
                                    rectsCount++;
                                    Console.WriteLine(lu + "  " + ru);
                                    Console.WriteLine(ll + "  " + rl);
                                    Console.WriteLine();

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("no of rects: " + rectsCount);
        }

        private static List<Point> InitPoints(int pointsCount)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                points.Add(new Point(rand.Next(10), rand.Next(10)));
            }

            return points;
        }
        private static void InitDistinctDescendingPoints(int pointsCount)
        {
            points = new List<Point>();
            for (int i = 0; i < pointsCount; i++)
            {
                Point newP;
                do
                {
                    newP = new Point(rand.Next(maxCoord + 1), rand.Next(maxCoord + 1));
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
            foreach (var item in points)
            {
                Console.WriteLine(item);
            }

            int prevY = points[0].Y;

            // stringbuffer rowStr = "OOOOOOOOOO";
            // string rowStr = "----------";

            int j = 0;
            for (int i = maxCoord; i >= 0; i--)
            {
                StringBuilder rowStrB = new StringBuilder("            ", maxCoord + 1);//"OOOOOOOOOOO", maxCoord + 1);

                while (j < points.Count && points[j].Y == i)
                {
                    rowStrB.Remove(points[j].X, 1);
                    rowStrB.Insert(points[j].X, "+");
                    j++;
                }
                Console.WriteLine(rowStrB);
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            j = 0;
            for (int i = maxCoord; i >= 0; i--)
            {
                StringBuilder rowStrB = new StringBuilder("                                    ", maxCoord + 1);

                while (j < points.Count && points[j].Y == i)
                {
                    rowStrB.Remove(3 * points[j].X, 3);
                    rowStrB.Insert(3 * points[j].X, " + ");
                    j++;
                }
                Console.WriteLine(rowStrB);
            }

            Console.WriteLine();

        }
    }
}
