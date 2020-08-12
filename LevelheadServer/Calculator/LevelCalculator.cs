using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LevelheadServer.Calculator
{
	public static class LevelCalculator
	{
        public static string A;
        private static double BASE = 10_000;
        private static double GROWTH = 2_500;
        private static double HALF_GROWTH = 0.5 * GROWTH;
        private static double REVERSE_PQ_PREFIX = -(BASE - 0.5 * GROWTH) / GROWTH;
        private static double REVERSE_CONST = REVERSE_PQ_PREFIX * REVERSE_PQ_PREFIX;
        private static double GROWTH_DIVIDES_2 = 2 / GROWTH;
        public static double GetLevel(double exp)
        {
            return exp < 0 ? 1 : Math.Floor(1 + REVERSE_PQ_PREFIX + Math.Sqrt(REVERSE_CONST + GROWTH_DIVIDES_2 * exp));
        }
        public static double GetExactLevel(double exp)
        {

            return GetLevel(exp) + GetPercentageToNextLevel(exp);
        }
        public static string GetExactStringLevel(double exp) => GetExactLevel(exp).ToString("0.0");
        public static double GetTotalExpToLevel(double level)
        {
            double lv = Math.Floor(level), x0 = GetTotalExpToFullLevel(lv);
            if (level == lv) return x0;
            return (GetTotalExpToFullLevel(lv + 1) - x0) * (level % 1) + x0;
        }
        public static double GetTotalExpToFullLevel(double level)
        {
            return (HALF_GROWTH * (level - 2) + BASE) * (level - 1);
        }
        public static double GetPercentageToNextLevel(double exp)
        {
            double lv = GetLevel(exp), x0 = GetTotalExpToLevel(lv);
            return (exp - x0) / (GetTotalExpToFullLevel(lv + 1) - x0);
        }
    }

	
}
