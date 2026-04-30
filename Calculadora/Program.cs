namespace Calculadora
{
    internal class Program
    {
        static int[] vector1 = new int[8], vector2 = new int[8], resultado = new int[8];
        static void Main(string[] args)
        {
            byte menu;
            do
            {
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                        CALCULADORA BASICA                        ║");
                Console.WriteLine("║                             GRUPO #4                             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
                Console.WriteLine(" 1. Sumar ");
                Console.WriteLine(" 2. Restar ");
                Console.WriteLine(" 3. Salir ");
                Console.Write("ELIJA UNA OPCION: ");
                menu = Convert.ToByte(Console.ReadLine());
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
            byte menu;
            int BaseSeleccionada = 0;
            bool valido = false;
            do
            {
                Console.Clear();
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                        CALCULADORA BASICA                        ║");
                Console.WriteLine("║                         SELECCIONAR BASE                         ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
                Console.WriteLine(" 1. Binario (Base 2) ");
                Console.WriteLine(" 2. Octal (Base 8) ");
                Console.WriteLine(" 3. Hexadecimal (Base 16) ");
                Console.WriteLine(" 4. Salir ");
                Console.Write("ELIJA UNA OPCION: ");
                menu = Convert.ToByte(Console.ReadLine());
                switch (menu)
                {
                    case 1:
                        BaseSeleccionada = 2;
                        valido = true;
                        break;
                    case 2:
                        BaseSeleccionada = 8;
                        valido = true;
                        break;
                    case 3:
                        BaseSeleccionada = 16;
                        valido = true;
                        break;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (!valido);
            return BaseSeleccionada;
        }
        static void Sumar()
        {
            Console.WriteLine("SUMAR VECTORES");
            char signo = '+';
            if (SeleccionarBase() == 2) Binario(signo);
            else if (SeleccionarBase() == 8) Console.WriteLine("Octal");
            else if (SeleccionarBase() == 16) Console.WriteLine("Hexadecimal");
        }
        static void Restar()
        {
            Console.WriteLine("RESTAR");
            char signo = '-';
            Binario(signo);
        }
        static void Binario(char signo)
        {
            // Llenar vector 1
            Console.WriteLine("Ingrese el primer número binario (8 bits):");
            string numero1 = Console.ReadLine() ?? "";
            // Completar con ceros a la izquierda hasta 8 bits
            numero1 = numero1.PadLeft(8, '0');
            if (numero1.Length > 8) Console.WriteLine("Error: Debe ingresar exactamente 8 bits.");
            for (int i = 0; i < numero1.Length; i++)
            {
                if (numero1[i] == '0') vector1[i] = 0;
                else if (numero1[i] == '1') vector1[i] = 1;
                else Console.WriteLine($"Error: El carácter '{numero1[i]}' no es un dígito binario.");
            }
            // Llenar vector 2
            Console.WriteLine("Ingrese el segundo número binario (8 bits):");
            string numero2 = Console.ReadLine() ?? "";
            // Completar con ceros a la izquierda hasta 8 bits
            numero2 = numero2.PadLeft(8, '0');
            if (numero2.Length > 8) Console.WriteLine("Error: Debe ingresar exactamente 8 bits.");
            for (int i = 0; i < numero2.Length; i++)
            {
                if (numero2[i] == '0') vector2[i] = 0;
                else if (numero2[i] == '1') vector2[i] = 1;
                else Console.WriteLine($"Error: El carácter '{numero2[i]}' no es un dígito binario.");
            }
            Console.WriteLine($"Vector 1 cargado: {string.Join("", vector1)} | Vector 2 cargado: {string.Join("", vector2)}");
            //Sumar de vectores
            if (signo == '+')
            {
                int acarreo = 0;
                for (int i = 7; i >= 0; i--)
                {
                    int suma = vector1[i] + vector2[i] + acarreo;
                    resultado[i] = suma % 2;
                    acarreo = suma / 2;
                }
                if (acarreo > 0)
                {
                    int[] vectorExpandido = new int[9];
                    vectorExpandido[0] = acarreo;
                    for (int i = 0; i < 8; i++) vectorExpandido[i + 1] = resultado[i];
                    resultado = vectorExpandido;
                }
            }
            else //Resta de vectores
            {
                int prestamo = 0;
                for (int i = 7; i >= 0; i--)
                {
                    int resta = vector1[i] - vector2[i] - prestamo;
                    if (resta < 0)
                    {
                        resultado[i] = resta + 2;
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
    }
}
