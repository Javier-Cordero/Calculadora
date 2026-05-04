namespace Calculadora
{
    internal class Program
    {
        static int[] vector1 = new int[8], vector2 = new int[8], resultado=new int[8];
        static void Main(string[] args)
        {
            string[] opciones = { "Sumar", "Restar", "Salir" };
            int menu, baseSistema;
            do
            {
                menu = MostrarMenu("CALCULADORA BASICA", opciones);
                if (menu == -1) continue;
                switch (menu)
                {
                    case 1:
                        baseSistema = SeleccionarBase();
                        if (baseSistema != 0) OperarVectores('+', baseSistema);
                        break;
                    case 2:
                        baseSistema = SeleccionarBase();
                        if (baseSistema != 0) OperarVectores('-', baseSistema);
                        break;
                    case 3: Console.WriteLine("Salir"); break;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (menu != 3);
            Console.ReadKey();
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
        static void OperarVectores(char signo, int baseSistema)
        {
            int acarreo = 0, prestamo = 0;
            string numero1, numero2;
            switch(baseSistema)
            {
                case 2:
                    numero1 = PedirNumero("Binario", "01");
                    numero2 = PedirNumero("Binario", "01");
                    for (int i = 0; i < numero1.Length; i++)
                    {
                        int pos = 8 - numero1.Length + i;
                        vector1[i] = numero1[i] - '0';
                    }
                    for (int i = 0; i < numero2.Length; i++)
                    {
                        int pos = 8 - numero2.Length + i;
                        vector2[i] = numero2[i] - '0';
                    }
                    break;
                case 8:
                    numero1 = PedirNumero("Octal", "01234567");
                    numero2 = PedirNumero("Octal", "01234567");
                    for (int i = 0; i < numero1.Length; i++)
                    {
                        int pos = 8 - numero1.Length + i;
                        vector1[i] = numero1[i] - '0';
                    }
                    for (int i = 0; i < numero2.Length; i++)
                    {
                        int pos = 8 - numero2.Length + i;
                        vector2[i] = numero2[i] - '0';
                    }
                    break;
                case 16:
                    numero1 = PedirNumero("Hexadecimal", "0123456789ABCDEF");
                    numero2 = PedirNumero("Hexadecimal", "0123456789ABCDEF");
                    for (int i = 0; i < numero1.Length; i++)
                    {
                        int pos = 8 - numero1.Length + i;
                        switch (numero1[i])
                        {
                            case '0': vector1[pos] = 0; break;
                            case '1': vector1[pos] = 1; break;
                            case '2': vector1[pos] = 2; break;
                            case '3': vector1[pos] = 3; break;
                            case '4': vector1[pos] = 4; break;
                            case '5': vector1[pos] = 5; break;
                            case '6': vector1[pos] = 6; break;
                            case '7': vector1[pos] = 7; break;
                            case '8': vector1[pos] = 8; break;
                            case '9': vector1[pos] = 9; break;
                            case 'A': vector1[pos] = 10; break;
                            case 'B': vector1[pos] = 11; break;
                            case 'C': vector1[pos] = 12; break;
                            case 'D': vector1[pos] = 13; break;
                            case 'E': vector1[pos] = 14; break;
                            case 'F': vector1[pos] = 15; break;
                        }
                    }
                    for (int i = 0; i < numero2.Length; i++)
                    {
                        int pos = 8 - numero2.Length + i;
                        switch (numero2[i])
                        {
                            case '0': vector2[pos] = 0; break;
                            case '1': vector2[pos] = 1; break;
                            case '2': vector2[pos] = 2; break;
                            case '3': vector2[pos] = 3; break;
                            case '4': vector2[pos] = 4; break;
                            case '5': vector2[pos] = 5; break;
                            case '6': vector2[pos] = 6; break;
                            case '7': vector2[pos] = 7; break;
                            case '8': vector2[pos] = 8; break;
                            case '9': vector2[pos] = 9; break;
                            case 'A': vector2[pos] = 10; break;
                            case 'B': vector2[pos] = 11; break;
                            case 'C': vector2[pos] = 12; break;
                            case 'D': vector2[pos] = 13; break;
                            case 'E': vector2[pos] = 14; break;
                            case 'F': vector2[pos] = 15; break;
                        }
                    }
                    break;
            }
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
            Console.WriteLine($"Vector 1 cargado: {string.Join("", vector1).TrimStart('0')} | Vector 2 cargado: {string.Join("", vector2).TrimStart('0')}");
            Console.WriteLine($"Resultado: {string.Join("", resultado.Select(d => d.ToString("X"))).TrimStart('0')}");
            Console.ReadKey();
        }
    }
}
