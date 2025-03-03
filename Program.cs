/*
Evalutor of expretions, for basic operations
Creator: Leymar Buenaventura Asprilla
Date: 2025-03-01
Curse: Progrmation II
*/

using System.Text.RegularExpressions;
using Operador;

namespace Programa
{
    class Program : Evaludor
    {
        static Stack<double> stackNumeros = new();
        static Stack<char> stackOperadores = new();
        static List<Tuple<string, double>> OperacionesGuardadas = new();
        private double Numero1 { get; set; }
        private double Numero2 { get; set; }
        private char Operacion { get; set; }
        private static string Expresion { get; set; } = string.Empty;

        // Se crear el constuctor de la clase heredada del namespace Operador
        public Program(double n1, char opr, double n2)
            : base(n1, opr, n2)
        {
            this.Numero1 = n1;
            this.Numero2 = n2;
            this.Operacion = opr;
        }

        static void ObterInforacion() // Obtiene la expresion que el usuirio quiere evaluar
        {
            Console.Write("Ingrese la expresion: ");
            var expresion = Console.ReadLine();
            Expresion = expresion?.Trim() ?? "";
        }

        static void ValidarExpresion()
        {
            // 1️⃣ Validar caracteres permitidos (solo números, operadores y espacios)
            if (!Regex.IsMatch(Expresion, @"^[0-9+\-*/().\s]+$"))
            {
                Console.WriteLine("❌ Error: La expresión contiene caracteres no permitidos.");
                return;
            }

            // 2️⃣ Validar si hay dos operadores seguidos (Ejemplo: 5++3, 4-+2)
            if (Regex.IsMatch(Expresion, @"[\+\-\*/]{2,}"))
            {
                Console.WriteLine("❌ Error: La expresión tiene operadores seguidos.");
                return;
            }

            // 3️⃣ Validar división por cero
            if (Regex.IsMatch(Expresion, @"/\s*0\b"))
            {
                Console.WriteLine("❌ Error: División por cero detectada.");
                return;
            }

            // ✅ Si pasa todas las validaciones, se evaluar la expreison la expresión
            try
            {
                return;
            }
            catch
            {
                Console.WriteLine("❌ Error: La expresión es inválida.");
            }
        }

        static void EvaluarExpresion()
        {
            var resutaldo = 0.0;
            bool existencia = Expresion.Contains("/") && Expresion.Contains("*");
            if (existencia)
            {
                empilar();
            }
            else
            {
                resutaldo = desempilar();
            }
            OperacionesGuardadas.Add(Tuple.Create(Expresion, resutaldo));
        }

        static void MostrarExpresiones()
        {
            var contador = 1;
            foreach (var expresion in OperacionesGuardadas)
            {
                Console.WriteLine($"[{contador}] ", expresion.Item1);
                contador++;
            }
        }

        static void BuscarExpresion()
        {
            Console.WriteLine("Ingrese el numero dela expreson a consultar: ");
            var numeroExpresion = Console.ReadLine() ?? "0";

            try
            {
                Console.WriteLine(
                    "Operacion: ",
                    OperacionesGuardadas[int.Parse(numeroExpresion)].Item1
                );
                Console.WriteLine(
                    "Resultado: ",
                    OperacionesGuardadas[int.Parse(numeroExpresion)].Item2
                );
            }
            catch
            {
                Console.WriteLine("Error: El numero ingresado no es valido.");
            }
        }

        static void empilar()
        {
            double acumulador_numeros = 0.0;
            foreach (var caracter in Expresion)
            {
                if (caracter == '+' || caracter == '-')
                {
                    stackOperadores.Push(caracter);
                    stackNumeros.Push(acumulador_numeros);
                    acumulador_numeros = 0.0;
                }
                if (caracter == '*' || caracter == '/') { }
                else
                {
                    Convert.ToInt32(caracter);
                    double double_caracter = (double)caracter;
                    acumulador_numeros += double_caracter;
                }
            }
        }

        static double desempilar()
        {
            return 0.0;
        }

        static void Main()
        {
            Evaludor evaluar = new Program("", "", "");
            var salida = false;

            while (salida)
            {
                Console.WriteLine("Opciones: ");
                Console.WriteLine("1. Evaluar Una Expresion.");
                Console.WriteLine("2. Mostrar Lista De Expresines.");
                Console.WriteLine("3. Consultar Expresion.");
                Console.WriteLine("0. Salir.");
                var Opciones = Console.ReadLine();

                switch (Opciones)
                {
                    case "1":
                        ObterInforacion();
                        ValidarExpresion();
                        EvaluarExpresion();
                        break;
                    case "2":
                        MostrarExpresiones();
                        break;
                    case "3":
                        BuscarExpresion();
                        break;
                    case "0":
                        salida = true;
                        break;
                    default:
                        Console.WriteLine("Opcion Incorrecta");
                        break;
                }
            }
        }
    }
}
