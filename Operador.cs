namespace Operador
{
    class Evaludor
    {
        // Atributos de la clase
        private double Numero1 { get; set; }
        private double Numero2 { get; set; }
        private char Operacion { get; set; }

        //constructor
        public Evaludor(double n1, char op, double n2)
        {
            Numero1 = n1;
            Numero2 = n2;
            Operacion = op;
        }

        public double Calcular(List<string> Operaciones)
        {
            switch (Operacion)
            {
                case '+':
                    return Numero1 + Numero2;
                case '-':
                    return Numero1 - Numero2;
                case '*':
                    return Numero1 * Numero2;
                case '/':
                    return Numero1 / Numero2;
                default:
                    return 0;
            }
        }
    }
}
