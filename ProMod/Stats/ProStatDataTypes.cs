using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.Stats
{
    public class ProVectorStat
    {
        private double x; private double y; private double z;
        private long count;
        public Vector3 Average()
        {
            if (count == 0) { return Vector3.zero; }
            return new Vector3((float)(x / (double)count), (float)(y / (double)count), (float)(z / (double)count));
        }

        public void Add(Vector3 v)
        {
            x += (double)v.x;
            y += (double)v.y;
            z += (double)v.z;
            count++;
        }
    }
    public class ProFloatStat
    {
        private double sum;
        private long count;
        public float Average()
        {
            if (count == 0) { return 0f; }
            return (float)(sum / (double)count);
        }
        public float Value()
        {
            return (float)sum;
        }
        public void Add(float v)
        {
            sum += (double)v;
            count++;
        }
    }
    public class ProIntegerStat
    {
        private long sum;
        private long count;
        public float Average()
        {
            if (count == 0) { return 0f; }
            return (float)((double)sum / (double)count);
        }

        public int Value()
        {
            return (int)sum;
        }

        public void Add(int v)
        {
            sum += (long)v;
            count++;
        }
    }

    public class ProIntegerHistogram
    {
        private int min;
        private int max;

        public int Min => min;
        public int Max => max;

        private long count;
        private long sum;

        private long[] values;

        public ProIntegerHistogram(int _min, int _max)
        {
            min = Math.Min(_min, _max);
            max = Math.Max(_min, _max);
            count = 0;
            sum = 0;
            values = new long[max - min + 1];
        }

        public void Add(int v)
        {
            if (v < min && v > max) { return; }

            sum += (long)v;
            count++;
            values[v - min]++;
        }
        public float Average()
        {
            if (count <= 0) { return 0.0f; }
            return (float)((double)sum / (double)count);
        }
        public long CountInRange(int _min, int _max)
        {
            int start = Math.Max(Math.Min(_min, _max), min);
            int end = Math.Min(Math.Max(_min, _max), max);
            long countInRange = 0;
            for (int i = start; i <= end; i++)
            {
                countInRange += values[i - min];
            }
            return countInRange;
        }
        public float RatioInRange(int _min, int _max)
        {
            if (count <= 0) { return 0.0f; }
            return (float)((double)CountInRange(_min, _max) / (double)count);
        }
        public long CountAtValue(int v)
        {
            if (v < min || v > max) { return 0; }
            return values[v - min];
        }
        public float RatioAtValue(int v)
        {
            if (count <= 0 || v < min || v > max) { return 0f; }

            return (float)((double)values[v] / (double)count);
        }
        public float StandardDeviation()
        {
            if (count <= 0) { return 0.0f; }
            double average = (double)sum / (double)count;
            double varSum = 0d;
            for (int i = min; i <= max; i++)
            {
                double diff = ((double)i - average);
                varSum += (double)values[i - min] * diff * diff;
            }

            return (float)Math.Sqrt(varSum / (double)count);
        }

        public long Count() { return count; }

    }
}
