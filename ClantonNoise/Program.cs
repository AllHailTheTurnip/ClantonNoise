using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ClantonNoise
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program(20000, 1000, 30, 1, 10, 0.9f);
        }

        // Noise Formula
        // Plot a set of points called 'pillars' that behave as the initial limits.
        // It then iterates over the pillars, plotting divisions.
        // After a set of divisions have been plotted, the formula draws between each division.

        public struct Point
        {
            public int x, y;
        }


        public Program(int width, int height, int pillars, int layers, int subdisions, float variance)
        {
            List<Point> points = new List<Point>();
            Pen pen = new Pen(Color.Black, 3);

            void DrawPoint(int x, int y, Graphics g, int index = -1)
            {
                Point point = new Point();
                point.x = x;
                point.y = y;
                // Adds the object to the end.
                if (index == -1)
                    points.Add(point);
                else
                    points.Insert(index, point);
                g.DrawRectangle(pen, new Rectangle(x, y, 1, 1));
            }

            // Create the Bitmap object.
            using (Bitmap bm = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    // Create background.
                    g.Clear(Color.White);

                    // Create random instance.
                    Random random = new Random();

                    // Create the primary points. 
                    for (int i = 0; i < pillars+1; i++)
                    {
                        int yWithVariance = (int) (height * (0 + random.NextDouble()) * variance);
                        DrawPoint(i * (width / pillars), yWithVariance, g);
                    }


                    // Create sub-points.
                    for (int i = 0; i < subdisions; i++)
                    {
                        // Create points between first two points in list. 
                        for (int j = 0; j < points.Count-1; j++)
                        {
                            Point pointA = points[j];
                            Point pointB = points[j + 1];

                            // Since pointA is supposed to be smaller than pointB. 
                            // If it's not, it'll switch the two around. 
                            if (pointA.y > pointB.y)
                            {
                                int reserveA = pointA.y;
                                pointA.y = pointB.y;
                                pointB.y = reserveA;
                            }


                            int x = random.Next(pointA.x, pointB.x);
                            int y = random.Next(pointA.y, pointB.y);
                            j++;
                            DrawPoint(x, y, g, j);
                            
                        }
                    }
                    
                    
                }

                bm.Save(@"Noise.png", ImageFormat.Png);
            }
        }
    }
}