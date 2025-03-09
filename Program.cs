/*
EVALUDADOR DE EXPRESIONES
INTEGRANTES: Leymar Buenaventura Asprilla y Luis Carlos Tez
FECHA: 2025-08-01
CURSO: Progrmation II
github: github.com/lebuas/evaluador
*/
using System;
using System.Collections.Generic;
using System.Globalization;
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
        Valida la expresión ingresada por el usuario utilizando expresiones regulares.
        Verifica caracteres permitidos, operadores seguidos y división por cero.
        Si la expresión es válida, procede a evaluarla.
        */
        static void ValidarExpresion()
        {
            try
            {
                // Validar si la expresión es válida o contiene caracteres no permitidos
                if (!Regex.IsMatch(Expresion, @"^-?[0-9+\-*/().\s]+$"))
                {
                    Console.WriteLine("Error: La expresión contiene caracteres no permitidos.\n");
                    return;
                }
                // Validar si hay operadores seguidos
                if (Regex.IsMatch(Expresion, @"[\+\-\*/]{2,}"))
                {
                    Console.WriteLine("Error: La expresión tiene operadores seguidos.\n");
                    return;
                }
                // Validar si hay divisiones por cero
                if (Regex.IsMatch(Expresion, @"\/(0(?!\.\d*[1-9])|0\.0+$)"))
                {
                    Console.WriteLine("Error: División por cero detectada.\n");
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
            var resultado = 0.0;
            Empilar();
            resultado = Desempilar();
            Console.WriteLine($"Expresion: {Expresion}");
            Console.WriteLine($"Resultado: {resultado}\n");

            OperacionesGuardadas.Add(Tuple.Create(Expresion, resultado));
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
                Console.WriteLine(
                    $"Resultado: {OperacionesGuardadas[int_numeroExpresion].Item2}\n"
                );
            }
            catch
            {
                Console.WriteLine("ERROR: POSICIÓN DE MEMORIA NO TIENE EXPRESIÓN.\n");
            }
        }

        /*
            Este método convierte la expresión en una pila de números y procesa operaciones de mayor jerarquía (multiplicación o división).
            - Recorre la expresión carácter por carácter y concatena en la variable `numero1` los caracteres que no sean operadores (/ o *).
            - Si encuentra un operador de suma o resta (+ o -), agrega el número que se iba almacenando en `numero1` a la pila,
              lo convierte a `double` y lo resetea a una cadena vacía y le asigna el operador actual al nuevo numero que se va a llenar.
            - Si encuentra un operador de multiplicación o división (* o /), recorre la expresión desde la posición actual +1
              hasta encontrar otro operador, almacenando el segundo número en `numero2`.
            - En cuanto se encuentra el operador, se procesa la multiplicación o división inmediatamente y el resultado se almacena en `numero1`.
            - Al finalizar el recorrido, el último número (si existe) se agrega a la pila.
        */


        static void Empilar()
        {
            var numero1 = string.Empty;
            var numero2 = string.Empty;
            var operadores = "+-*/";
            double resultado = 0;

            for (int x = 0; x < Expresion.Length; x++)
            {
                var caracter = Expresion[x];
                string new_caracter = caracter.ToString();

                // Maneja números negativos al inicio o después de un operador
                if (caracter == '-' && (x == 0 || operadores.Contains(Expresion[x - 1].ToString())))
                {
                    numero1 += new_caracter;
                    continue;
                }
                // Si encuentra un operador de suma o resta (+ o -), guarda el número en la pila
                else if (caracter == '+' || caracter == '-')
                {
                    stackNumeros.Push(
                        double.Parse(numero1, System.Globalization.CultureInfo.InvariantCulture)
                    );
                    numero1 = new_caracter; // Se reinicia para capturar el nuevo número
                    numero2 = string.Empty;
                }
                // Si encuentra un operador de multiplicación o división (* o /)
                else if (caracter == '*' || caracter == '/')
                {
                    int i = x + 1;
                    numero2 = string.Empty;

                    // Capturar el segundo número hasta encontrar otro operador
                    while (i < Expresion.Length && !operadores.Contains(Expresion[i]))
                    {
                        numero2 += Expresion[i].ToString();
                        i++;
                    }
                    x = i - 1; // Ajustar índice

                    // Convertir los números y realizar la operación inmediatamente
                    double num1 = double.Parse(
                        numero1,
                        System.Globalization.CultureInfo.InvariantCulture
                    );
                    double num2 = double.Parse(
                        numero2,
                        System.Globalization.CultureInfo.InvariantCulture
                    );

                    resultado = (caracter == '*') ? num1 * num2 : num1 / num2;

                    // Guardar el resultado en numero1 para seguir procesando
                    numero1 = resultado.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    numero1 += new_caracter; // Acumular número
                }
            }

            // Guardar el último número en la pila si existe
            if (!string.IsNullOrEmpty(numero1))
            {
                stackNumeros.Push(
                    double.Parse(numero1, System.Globalization.CultureInfo.InvariantCulture)
                );
            }
        }

        /// Desempila los números empilador, y mientras desempila, los va sumando, para obtener el resultado final de la expresión.
        static double Desempilar()
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
        - Muestra un menú interactivo.
        - Permite ingresar expresiones, ver la memoria o salir.
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
