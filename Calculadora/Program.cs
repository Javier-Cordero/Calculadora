namespace Calculadora
{
    internal class Program
    {
        static int[] vector1 = new int[8], vector2 = new int[8], resultado = new int[8];
        static void Main(string[] args)
        {
            string[] opciones = { "Sumar", "Restar", "Salir" };
            int menu;
            do
            {
                menu = MostrarMenu("CALCULADORA BASICA", opciones);
                if (menu == -1) continue;
                switch (menu)
                {
                    case 1: Sumar(); break;
                    case 2: Restar(); break;
                    case 3: Console.WriteLine("Salir"); break;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (menu != 3);
            Console.ReadKey();
        }
        static int SeleccionarBase()
        {
            string[] opciones = { "Binario", "Octal", "Hexadecimal", "Salir" };
            int menu;
            do
            {
                menu = MostrarMenu("SELECCIONAR BASE", opciones);
                if (menu == -1) continue;
                switch (menu)
                {
                    case 1: return 2;
                    case 2: return 8;
                    case 3: return 16;
                    case 4: return 0;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (true);
        }
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
        static void Operar(char signo, string operacion)
        {
            Console.WriteLine(operacion.ToUpper());
            switch (SeleccionarBase())
            {
                case 2: Binario(signo); break;
                case 8: Octal(signo); break;
                case 16: Hexadecimal(signo); break;
                default: Console.WriteLine("Base no permitida"); break;
            }
        }
        static string PedirNumero(string tipo, string caracteresValidos)
        {
            string numero;
            bool valido;
            do
            {
                Console.WriteLine($"Ingrese el número {tipo} (maximo 8bits):");
                numero = Console.ReadLine()?.ToUpper() ?? "";
                if (string.IsNullOrEmpty(numero))
                {
                    Console.WriteLine("Error: Entrada vacía. Intente de nuevo.");
                    valido = false;
                }
                else if (numero.Length > 8)
                {
                    Console.WriteLine($"Error: Máximo 8 bits permitidos.");
                    valido = false;
                }
                else if (numero.All(c => caracteresValidos.Contains(c))) valido = true;
                else
                {
                    Console.WriteLine($"Error: Solo se permiten caracteres: {caracteresValidos}");
                    valido = false;
                }
            } while (!valido);
            return numero.PadLeft(8, '0');
        }
        static void Sumar() => Operar('+', "sumar vectores");
        static void Restar() => Operar('-', "restar vectores");
        static void OperarVectores(char signo, int baseSistema)
        {
            int[] resultado = new int[8];
            int acarreo = 0, prestamo = 0;
            if (signo == '+')
            {
                for (int i = 7; i >= 0; i--)
                {
                    int suma = vector1[i] + vector2[i] + acarreo;
                    resultado[i] = suma % baseSistema;
                    acarreo = suma / baseSistema;
                }
                if (acarreo > 0)
                {
                    int[] vectorExpandido = new int[9];
                    vectorExpandido[0] = acarreo;
                    Array.Copy(resultado, 0, vectorExpandido, 1, 8);
                    resultado = vectorExpandido;
                }
            }
            else // signo == '-'
            {
                for (int i = 7; i >= 0; i--)
                {
                    int resta = vector1[i] - vector2[i] - prestamo;
                    if (resta < 0)
                    {
                        resultado[i] = resta + baseSistema;
                        prestamo = 1;
                    }
                    else
                    {
                        resultado[i] = resta;
                        prestamo = 0;
                    }
                }
            }

            Console.WriteLine($"Resultado: {string.Join("", resultado)}");
        } 
        static void Binario(char signo)
        {
            // Llenar vector 1
            string numero1 = PedirNumero("Binario", "01");
            for (int i = 0; i < numero1.Length; i++) vector1[i] = numero1[i] - '0';
            // Llenar vector 2
            string numero2 = PedirNumero("Binario", "01");
            for (int i = 0; i < numero2.Length; i++) vector2[i] = numero2[i] - '0';
            Console.WriteLine($"Vector 1 cargado: {string.Join("", vector1)} | Vector 2 cargado: {string.Join("", vector2)}");
            OperarVectores(signo, 2);
            Console.ReadKey();
        }
        static void Octal(char signo)
        {
            // Llenar vector 1
            string numero1 = PedirNumero("Octal", "01234567");
            for (int i = 0; i < numero1.Length; i++) vector1[i] = numero1[i] - '0';
            // Llenar vector 2
            string numero2 = PedirNumero("Octal", "01234567");
            for (int i = 0; i < numero2.Length; i++) vector2[i] = numero2[i] - '0';
            Console.WriteLine($"Vector 1 cargado: {string.Join("", vector1)} | Vector 2 cargado: {string.Join("", vector2)}");
            OperarVectores(signo, 8);
            Console.ReadKey();
        }
        static void Hexadecimal(char signo)
        {
            // Llenar vector 1
            string numero1 = PedirNumero("Hexadecimal", "0123456789ABCDEF");
            for (int i = 0; i < numero1.Length; i++)
            {
                switch (numero1[i])
                {
                    case '0': vector1[i] = 0; break;
                    case '1': vector1[i] = 1; break;
                    case '2': vector1[i] = 2; break;
                    case '3': vector1[i] = 3; break;
                    case '4': vector1[i] = 4; break;
                    case '5': vector1[i] = 5; break;
                    case '6': vector1[i] = 6; break;
                    case '7': vector1[i] = 7; break;
                    case '8': vector1[i] = 8; break;
                    case '9': vector1[i] = 9; break;
                    case 'A': vector1[i] = 10; break;
                    case 'B': vector1[i] = 11; break;
                    case 'C': vector1[i] = 12; break;
                    case 'D': vector1[i] = 13; break;
                    case 'E': vector1[i] = 14; break;
                    case 'F': vector1[i] = 15; break;
                }
            }
            // Llenar vector 2
            string numero2 = PedirNumero("Hexadecimal", "0123456789ABCDEF");
            for (int i = 0; i < numero2.Length; i++)
            {
                switch (numero2[i])
                {
                    case '0': vector2[i] = 0; break;
                    case '1': vector2[i] = 1; break;
                    case '2': vector2[i] = 2; break;
                    case '3': vector2[i] = 3; break;
                    case '4': vector2[i] = 4; break;
                    case '5': vector2[i] = 5; break;
                    case '6': vector2[i] = 6; break;
                    case '7': vector2[i] = 7; break;
                    case '8': vector2[i] = 8; break;
                    case '9': vector2[i] = 9; break;
                    case 'A': vector2[i] = 10; break;
                    case 'B': vector2[i] = 11; break;
                    case 'C': vector2[i] = 12; break;
                    case 'D': vector2[i] = 13; break;
                    case 'E': vector2[i] = 14; break;
                    case 'F': vector2[i] = 15; break;
                }
            }
            Console.WriteLine($"Vector 1 cargado: {string.Join("", vector1)} | Vector 2 cargado: {string.Join("", vector2)}");
            OperarVectores(signo, 16);
            Console.ReadKey();
        }
    }
}
