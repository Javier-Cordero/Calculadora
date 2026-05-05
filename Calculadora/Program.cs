namespace Calculadora
{
    internal class Program
    {
        //vectores declarados a nivel de clase de tamaño 8
        static int[] vector1 = new int[8];
        static int[] vector2 = new int[8];
        // Puede crecer a 9 elementos si hay acarreo en la suma
        static int[] resultado=new int[8];
        static void Main(string[] args)
        {
            string[] opciones = { "Sumar", "Restar", "Salir" };
            int menu;
            do
            {
                menu = MostrarMenu("CALCULADORA BASICA", opciones);
                switch (menu)
                {
                    case 1: OperarVectores('+'); break;
                    case 2: OperarVectores('-'); break;
                    case 3: Console.WriteLine("¡Gracias por usar la calculadora!"); break;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (menu != 3);
            Console.ReadKey();
        }
        // Mostrar menu dinamico
        static int MostrarMenu(string titulo, string[] opciones)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║                   {titulo,-46} ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            for (int i = 0; i < opciones.Length; i++) Console.WriteLine($" {i + 1}. {opciones[i]}");
            Console.Write("ELIJA UNA OPCION: ");
            string input = Console.ReadLine() ?? "";
            return int.TryParse(input, out int opcion) ? opcion : -1;
        }
        // Presenta un submenú para que el usuario seleccione la base numérica.
        static int SeleccionarBase()
        {
            string[] opciones = { "Binario", "Octal", "Hexadecimal", "Salir" };
            int menu;
            do
            {
                menu = MostrarMenu("SELECCIONAR BASE", opciones);
                if (menu == 1) return 2;
                if (menu == 2) return 8;
                if (menu == 3) return 16;
                if (menu == 4) return 0;
            } while (true);
        }
        // Solicita al usuario un número en la base especificada.
        static string PedirNumero(string n, int baseSistema)
        {
            bool valido;
            string numero, tipo = "", caracteresValidos = "";
            if (baseSistema == 2) { caracteresValidos = "01"; tipo = "Binario"; }
            if (baseSistema == 8) { caracteresValidos = "01234567"; tipo = "Octal"; }
            if (baseSistema == 16) { caracteresValidos = "0123456789ABCDEF"; tipo = "Hexadecimal"; }
            do
            {
                Console.Write($"Ingrese el {n} número {tipo} (maximo 8bits): ");
                numero = Console.ReadLine()?.ToUpper() ?? "";
                valido = true;
                // valida que los dígitos sean correctos.
                if (string.IsNullOrEmpty(numero))
                {
                    Console.WriteLine("Error: Entrada vacía.");
                    valido = false;
                }
                else if (numero.Length > 8)
                {
                    Console.WriteLine($"Error: Máximo 8 bits permitidos.");
                    valido = false;
                }
                else
                {
                    for (int i = 0; i < numero.Length; i++)
                    {
                        if (!caracteresValidos.Contains(numero[i]))
                        {
                            Console.WriteLine($"Error: Solo se permiten: {caracteresValidos}");
                            valido = false;
                            break;
                        }
                    }
                }
            } while (!valido);
            // Completar con ceros a la izquierda hasta 8 dígitos
            return numero.PadLeft(8, '0');
        }
        // Convierte un string numérico en un arreglo de enteros.
        static void LlenarVector(string numero, int[] vector, int baseSistema)
        {
            int pos = 8 - numero.Length;
            // conversion a su formato de su base
            for (int i = 0; i < numero.Length; i++) vector[pos + i] = baseSistema == 16 ? Convert.ToInt32(numero[i].ToString(), 16) : numero[i] - '0';
        }
        // Realiza la suma de dos vectores dígito por dígito,
        // manejando el acarreo según la base numérica.
        // El resultado se almacena en el vector global 'resultado'.
        static void Sumar(int baseSistema)
        {
            int acarreo = 0;
            int[] sumaTemp = new int[9];  // Temporal de 9 posiciones
            // Sumar de derecha a izquierda
            for (int i = 7; i >= 0; i--)
            {
                int suma = vector1[i] + vector2[i] + acarreo;
                sumaTemp[i + 1] = suma % baseSistema;
                acarreo = suma / baseSistema;
            }
            sumaTemp[0] = acarreo;
            // Si el primer dígito es 0, reducir tamaño
            if (sumaTemp[0] == 0 && sumaTemp.Length > 1)
            {
                resultado = new int[sumaTemp.Length - 1];
                Array.Copy(sumaTemp, 1, resultado, 0, resultado.Length);
            }
            else resultado = sumaTemp;
        }
        // resta de vectores segun la base seleccionada
        // Compara dos vectores para determinar cuál es mayor numéricamente.
        static bool EsMayor(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > b[i]) return true;
                if (a[i] < b[i]) return false;
            }
            return false; // Son iguales
        }
        // Realiza la resta de dos vectores dígito por dígito,
        // manejando el préstamo según la base numérica.
        // El resultado se almacena en el vector global 'resultado'.
        static void Restar(int baseSistema)
        {
            // Reiniciar resultado (por si venía de una suma con tamaño 9)
            resultado = new int[8];
            // Determinar cuál vector es mayor para evitar resultados negativos
            bool vector1Mayor = EsMayor(vector1, vector2);
            // Mayor y menor usando operador ternario
            int[] mayor = vector1Mayor ? vector1 : vector2;
            int[] menor = vector1Mayor ? vector2 : vector1;
            int prestamo = 0;
            for (int i = 7; i >= 0; i--)
            {
                int resta = mayor[i] - menor[i] - prestamo;
                if (resta < 0)
                {
                    resultado[i] = resta + baseSistema; // Pedir prestado
                    prestamo = 1;
                }
                else
                {
                    resultado[i] = resta;
                    prestamo = 0;
                }
            }
        }
        // Método principal de la operación:
        // selecciona base, solicita números, llena vectores,
        // ejecuta suma o resta y muestra resultados.
        static void OperarVectores(char signo)
        {
            // guarda el valor de la base numérica en una variable
            int baseSistema = SeleccionarBase();
            // Limpiar vectores (por si tenían datos de operaciones anteriores)
            Array.Clear(vector1, 0, vector1.Length);
            Array.Clear(vector2, 0, vector2.Length);
            Array.Clear(resultado, 0, resultado.Length);
            // Solicitar los dos números al usuario
            string numero1 = PedirNumero("primer", baseSistema);
            string numero2 = PedirNumero("segundo", baseSistema);
            // Convertir strings a vectores de enteros
            LlenarVector(numero1, vector1, baseSistema);
            LlenarVector(numero2, vector2, baseSistema);
            // Ejecutar la operación seleccionada
            if (signo == '+') Sumar(baseSistema);
            else Restar(baseSistema);
            // Mostrar resultados
            Console.WriteLine($"{new string('═', 65)}");
            Console.WriteLine($"Vector 1 cargado: {FormatearVector(vector1)}");
            Console.WriteLine($"Vector 2 cargado: {FormatearVector(vector2)}");
            Console.WriteLine($"Resultado: {FormatearVector(resultado)}");
            Console.WriteLine($"{new string('═', 65)}");
            Console.ReadKey();
        }
        // Formatea un vector para mostrarlo en consola,
        // eliminando ceros a la izquierda y manejando formato hexadecimal.
        static string FormatearVector(int[] vector)
        {
            string resultadoTexto = string.Join("", vector.Select(d => d.ToString("X")));
            string resultadoTrimmed = resultadoTexto.TrimStart('0');
            return string.IsNullOrEmpty(resultadoTrimmed) ? "0" : resultadoTrimmed;
        }
    }
}
