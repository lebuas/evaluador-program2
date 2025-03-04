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
            this.Numero1 = n1;
            this.Numero2 = n2;
            this.Operacion = op;
        }

        public double Calcular()
        {
            switch (this.Operacion)
            {
                case '+':
                    return this.Numero1 + this.Numero2;
                case '-':
                    return this.Numero1 - this.Numero2;
                case '*':
                    return this.Numero1 * this.Numero2;
                case '/':
                    return this.Numero1 / this.Numero2;
                default:
                    return 0;
            }
        }
    }
}
