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
        static string PedirNumero(string n, string tipo, string caracteresValidos)
        {
            string numero;
            bool valido;
            do
            {
                Console.Write($"Ingrese el {n} número {tipo} (maximo 8bits): ");
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
        static void LlenarVector(string numero, int[] vector, int baseSistema)
        {
            int pos = 8 - numero.Length;
            for (int i = 0; i < numero.Length; i++) vector[pos + i] = baseSistema == 16 ? Convert.ToInt32(numero[i].ToString(), 16) : numero[i] - '0';
        }
        static void Sumar(int baseSistema)
        {
            int acarreo = 0;
            for (int i = 7; i >= 0; i--)
            {
                int suma = vector1[i] + vector2[i] + acarreo;
                resultado[i] = suma % baseSistema;
                acarreo = suma / baseSistema;
            }
            if (acarreo > 0)
            {
                Console.WriteLine($"acarreos: {acarreo}");
                int[] temporal = new int[9];
                temporal[0] = acarreo;
                Array.Copy(resultado, 0, temporal, 1, 8);
                resultado = temporal;
            }
        }
        static void Restar(int baseSistema)
        {
            int prestamo = 0;
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
        static void OperarVectores(char signo, int baseSistema)
        {
            Array.Clear(vector1, 0, vector1.Length);
            Array.Clear(vector2, 0, vector2.Length);
            Array.Clear(resultado, 0, resultado.Length);
            string numero1, numero2;
            switch(baseSistema)
            {
                case 2:
                    numero1 = PedirNumero("primer", "Binario", "01");
                    numero2 = PedirNumero("segundo", "Binario", "01");
                    break;
                case 8:
                    numero1 = PedirNumero("primer", "Octal", "01234567");
                    numero2 = PedirNumero("segundo", "Octal", "01234567");
                    break;
                case 16:
                    numero1 = PedirNumero("primer", "Hexadecimal", "0123456789ABCDEF");
                    numero2 = PedirNumero("segundo", "Hexadecimal", "0123456789ABCDEF");
                    break;
                default:return;
            }
            LlenarVector(numero1, vector1, baseSistema);
            LlenarVector(numero2, vector2, baseSistema);
            if (signo == '+') Sumar(baseSistema);
            else Restar(baseSistema);
            Console.WriteLine($"Vector 1 cargado: {(string.Join("", vector1).TrimStart('0') == "" ? "0" : string.Join("", vector1).TrimStart('0'))}\nVector 2 cargado: {(string.Join("", vector2).TrimStart('0') == "" ? "0" : string.Join("", vector2).TrimStart('0'))}\nResultado: {(string.Join("", resultado.Select(d => d.ToString("X"))).TrimStart('0') == "" ? "0" : string.Join("", resultado.Select(d => d.ToString("X"))).TrimStart('0'))}");
            Console.ReadKey();
        }
    }
}
