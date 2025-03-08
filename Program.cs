/*
EVALUDADOR DE EXPRESIONES
INTEGRANTES: Leymar Buenaventura Asprilla y Luis Carlos Tez
FECHA: 2025-08-01
CURSO: Progrmation II
github: github.com/lebuas/evaluador
*/
using System.Text.RegularExpressions;

namespace Programa
{
    class Program
    {
        static Stack<double> stackNumeros = new();
        static List<Tuple<string, double>> OperacionesGuardadas = new();
        private static string Expresion { get; set; } = string.Empty;
        private static int longExpresion;

        /*
        Valida la expresión ingresada por el usuario.
        Verifica caracteres permitidos, operadores seguidos y división por cero.
        Si la expresión es válida, procede a evaluarla.
        */
        static void ValidarExpresion()
        {
            try
            {
                if (!Regex.IsMatch(Expresion, @"^-?[0-9+\-*/().\s]+$"))
                {
                    Console.WriteLine("Error: La expresión contiene caracteres no permitidos.");
                }
                if (Regex.IsMatch(Expresion, @"[\+\-\*/]{2,}"))
                {
                    Console.WriteLine("Error: La expresión tiene operadores seguidos.");
                    return;
                }
                if (Regex.IsMatch(Expresion, @"/\s*0\b"))
                {
                    Console.WriteLine("Error: División por cero detectada.");
                    return;
                }

                EvaluarExpresion();
                return;
            }
            catch
            {
                return;
            }
        }

        /*
        Evalúa la expresión matemática.
        Usa los métodos Empilar() y Desempilar() para calcular el resultado.
        Muestra el resultado y guarda la expresión con su resultado en la lista.
        */
        static void EvaluarExpresion()
        {
            var resutaldo = 0.0;
            empilar();
            resutaldo = desempilar();
            Console.WriteLine($"Expresion: {Expresion}");
            Console.WriteLine($"Resultado: {resutaldo}\n");

            OperacionesGuardadas.Add(Tuple.Create(Expresion, resutaldo));
        }

        /// Muestra todas las expresiones guardadas en la lista OperacionesGuardadas.
        /// Cada expresión se muestra con un número de índice.

        static void MostrarExpresiones()
        {
            var contador = 1;
            foreach (var expresion in OperacionesGuardadas)
            {
                Console.WriteLine($"[{contador}] {expresion.Item1}");
                contador++;
            }
            Console.WriteLine();
        }

        /// Busca una expresión en la lista OperacionesGuardadas usando un índice.
        /// El índice debe estar entre corchetes (ejemplo: [1]).
        /// Muestra la expresión y su resultado si existe; de lo contrario, muestra un error.

        static void BuscarExpresion()
        {
            try
            {
                var nuemeroExpresion = Expresion.Substring(1, (Expresion.Length - 2));
                int int_numeroExpresion = int.Parse(nuemeroExpresion) - 1;
                Console.WriteLine($"Operación: {OperacionesGuardadas[int_numeroExpresion].Item1}");
                Console.WriteLine($"Resultado: {OperacionesGuardadas[int_numeroExpresion].Item2}");
            }
            catch
            {
                Console.WriteLine("ERROR: POSICIÓN DE MEMORIA NO TIENE EXPRESIÓN.\n");
            }
        }

        /*
           Este método convierte la expresion en pila de numeros y procesa las operaiociones de mayor gerarquía, dejando en la pila solo numeros que se suman o se restan
           Funcionamiento:
           -Se recore la exprsion y deacuerdo al aracter se toma una decicion:
           1. Si encuentra un operador de suma o resta (+ o -), lo agrega a la variable numero1.
           2. Si encuentra un operador de multiplicacion o división (/ o *), se agrega a la variable numero2.
           3. Si encuentra un caracter que no sea un operador de suma o resta, se agrega a la variable numero1.
           4. Si encuentra un operador de suma o resta, se suma numero1 y numero2 y se agrega el resultado a la variable numero1.
           5. Si encuentra un operador de multiplicacion o división, se multiplica numero1 y numero2 y se agrega el resultado a la variable numero1.
           6. Si encuentra un caracter que no sea un operador de suma o resta, se agrega a la variable numero2.
           7. Si encuentra un operador de suma o resta, se suma numero1 y numero2 y se agrega el resultado a la variable numero1.
           8. Si encuentra un operador de multiplicacion o división, se multiplica numero1 y numero2 y se agrega el resultado a la variable numero1.
           9. Si encuentra un caracter que no sea un operador de suma o resta, se agrega a la variable numero1.

        */
        static void empilar()
        {
            var numero1 = string.Empty;
            var numero2 = string.Empty;
            var operadores = "+-*/";
            double resultado = 0;

            for (int x = 0; x < Expresion.Length; x++)
            {
                var caracter = Expresion[x];
                string new_caracter = caracter.ToString();

                if (caracter == '-' && (x == 0 || operadores.Contains(Expresion[x - 1].ToString())))
                {
                    numero1 += new_caracter;
                    continue;
                }

                if (caracter == '+' || caracter == '-')
                {
                    if (!string.IsNullOrEmpty(numero1))
                    {
                        var new_numero1 = double.Parse(numero1);
                        stackNumeros.Push(new_numero1);
                    }
                    numero1 = new_caracter;
                    numero2 = string.Empty;
                }
                else if (caracter == '*' || caracter == '/')
                {
                    var i = x + 1;

                    while (i < Expresion.Length && !operadores.Contains(Expresion[i]))
                    {
                        string new2_caracter = Expresion[i].ToString();
                        numero2 += new2_caracter;
                        i++;
                    }
                    x = i - 1;

                    switch (caracter)
                    {
                        case '/':
                            resultado = double.Parse(numero1) / double.Parse(numero2);
                            break;
                        case '*':
                            resultado = double.Parse(numero1) * double.Parse(numero2);
                            break;
                    }

                    numero1 = resultado.ToString();
                }
                else
                {
                    numero1 += new_caracter;
                }
            }

            if (!string.IsNullOrEmpty(numero1))
            {
                double numeroFinal = double.Parse(numero1);
                stackNumeros.Push(numeroFinal);
            }
        }

        /// Desempila los números de la pila stackNumeros y los suma.
        /// Retorna el resultado final de la expresión.

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

        /*
         Método principal del programa.
        Muestra un menú interactivo para ingresar expresiones, ver la memoria o salir.
        */
        static void Main()
        {
            var salida = true;
            Console.WriteLine("Bienvenido al Evaluador de Expresiones.");
            Console.WriteLine("Creado por Leymar Buenaventura Asprilla y Luis Carlos Tez.\n");

            while (salida)
            {
                Console.Write("Ingrese Expresion:-> ");
                var datos = Console.ReadLine();
                Expresion = datos?.Replace(" ", "").ToLower() ?? "";
                longExpresion = Expresion.Length;

                {
                    if (Expresion == "memoria" || Expresion == "m")
                    {
                        MostrarExpresiones();
                    }
                    else if (Expresion == "salir" || Expresion == "s")
                    {
                        salida = false;
                        Console.WriteLine("Saliendo del programa........");
                        Console.WriteLine();
                    }
                    else if (Expresion.Any(c => c == '+' || c == '-' || c == '*' || c == '/'))
                    {
                        ValidarExpresion();
                    }
                    else if (
                        (Expresion.StartsWith("[") && Expresion.EndsWith("]"))
                        && longExpresion > 2
                    )
                    {
                        BuscarExpresion();
                    }
                    else
                    {
                        Console.WriteLine("Expresion invalida. Reintente nuevamente.\n");
                    }
                }
            }
        }
    }
}
