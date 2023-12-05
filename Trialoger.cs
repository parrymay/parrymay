namespace Trialogica
{
    public static class Trialoger
    {
        // По итогу бенчмарков получилось добиться скорости ~27 ns на процессоре i7-1165G7.
        // Код изобирует комментариями и пояснениями, в рабочем проекте обычно не пишем
        // Упор был на производительность, поэтому по читаемости могут быть вопросы


        // Перед началом выполнения задания, стоило уточнить о требованиях по точности.
        // Делаю механизм её регулирования

        /// <summary>
        /// Get the type of triangle based on its three sides.
        /// </summary>
        public static int Precision { get; set; } = 4;

        /// <summary>
        /// Get the type of triangle based on its three sides.
        /// </summary>
        public static TriangleType GetTriangleType(double sideA, double sideB, double sideC)
        {
            var sides = new double[] { sideA, sideB, sideC };
            
            Array.Sort(sides);
            Validate(sides[2], sides[1], sides[0]);
            
            //Косинус большей стороны позволит однозначно определить тип
            var cos = Math.Round(Cos(sides[2], sides[1], sides[0]), Precision, MidpointRounding.AwayFromZero);

            if (cos <= 0)
            {
                if (cos == 0)
                    return TriangleType.RightTriangle;
                return TriangleType.ObtuseTriangle;
            }
            return TriangleType.AcuteTriangle;
        }

        private static void Validate(double sideA, double sideB, double sideC)
        {
            if (sideA <= 0 || sideB <= 0 || sideC <= 0)
                throw new ArgumentException($"The side of a triangle cannot be negative");

            if (!IsTriangle(sideA, sideB, sideC))
                throw new ArgumentException($"A triangle with sides: {sideA}, {sideB}, {sideC} does not exist.");
        }

        //Благодаря тому, что массив отсортирован, можем использовать только одну проверку вместо трех
        private static bool IsTriangle(double longestSide, double sideB, double sideC)
            => sideB + sideC > longestSide;

        //Не использую Math.Pow() в угоду большей скорости. Бенчмарк показывает 27 ns против 90 ns.
        private static double Cos(double opposite, double adjacentA, double adjacentB)
            => ((adjacentA * adjacentA) + (adjacentB * adjacentB) - (opposite * opposite)) / (2 * adjacentA * adjacentB);
    }
}
