/*
Evaluator of expretions, for basic operations
Creator: Leymar Buenaventura Asprilla
Date: 2025-03-01
Curse: Progrmation II
*/

using System.Text.RegularExpressions;
using Operador;

namespace Programa
{
    class Program
    {
        static Stack<double> stackNumeros = new();
        static List<Tuple<string, double>> OperacionesGuardadas = new();
        private static string Expresion { get; set; } = string.Empty;

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
            empilar();
            resutaldo = desempilar();
            Console.WriteLine($"Expresion: {Expresion}");
            Console.WriteLine($"Resultado: {resutaldo}");
            Console.WriteLine();
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

        /*
        el metodo empliar, recibe la variables Expresion que ingreso el usuario de tipo string
        en las variables numero1 y numero2 se guardan los numero que se sacan de la cadena
        numero 1 seguardan los valores antes de encotrar un operaodor y numero2 despues de encontrar el operador
        si se encuentra un opprador tipo suma o resta el numero 1 se agreaga al stack y se vuelve  a comenzar, para mantenr la prioridad de las operaciones
         */

        static void empilar()
        {
            var numero1 = string.Empty;
            var numero2 = string.Empty;
            var operadores = "+-*/";

            for (int x = 0; x < Expresion.Count(); x++)
            {
                var caracter = Expresion[x];
                string new_caracter = caracter.ToString();

                if (caracter == '+' || caracter == '-')
                {
                    var new_numero1 = double.Parse(numero1);
                    stackNumeros.Push(new_numero1);
                    numero1 = new_caracter;
                    numero2 = string.Empty;
                }
                else if (caracter == '*' || caracter == '/')
                {
                    var i = x + 1;

                    while (i < Expresion.Count() && !operadores.Contains(Expresion[i]))
                    {
                        string new2_caracter = Expresion[i].ToString();
                        numero2 += new2_caracter;
                        i++;
                    }
                    x = i - 1;
                    var new_numero1 = double.Parse(numero1);
                    var new_numero2 = double.Parse(numero2);
                    Evaludor evaluador = new(new_numero1, caracter, new_numero2);
                    var resultado = evaluador.Calcular();
                    numero1 = resultado.ToString();
                }
                else
                {
                    numero1 += new_caracter;
                }
            }
            double numeroFinal = double.Parse(numero1);
            stackNumeros.Push(numeroFinal);
        }

        static double desempilar()
        {
            double resultado = 0.0;
            while (stackNumeros.Count > 0)
            {
                var dato = stackNumeros.Pop();
                resultado += dato;
            }
            return resultado;
        }

        static void Main()
        {
            // Evaludor evaluar = new Program(0, '+', 0);
            var salida = true;

            while (salida)
            {
                Console.WriteLine("Opciones: ");
                Console.WriteLine("1. Evaluar Una Expresion.");
                Console.WriteLine("2. Mostrar Lista De Expresines.");
                Console.WriteLine("3. Consultar Expresion.");
                Console.WriteLine("0. Salir.");
                Console.Write("Opciones: ");
                var Opciones = Console.ReadLine();
                Console.WriteLine();

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
                        Console.WriteLine("Saliendo del programa....");
                        salida = false;
                        break;
                    default:
                        Console.WriteLine("Opcion Incorrecta");
                        break;
                }
            }
        }
    }
}
