namespace Calculadora
{
    // Calculadora básica que opera números en bases binaria, octal y hexadecimal.
    // Los números se manejan como vectores de hasta 8 dígitos.
    internal class Program
    {
        //VARIABLES GLOBALES (a nivel de clase)
        static int[] vector1 = new int[8];
        static int[] vector2 = new int[8];
        static int[] resultado=new int[8];
        // Punto de entrada principal de la aplicación.
        // Muestra el menú principal y gestiona el flujo de la calculadora.
        static void Main(string[] args)
        {
            string[] opciones = { "Sumar", "Restar", "Salir" }; // Opciones del menú principal
            int menu;
            do
            {
                menu = MostrarMenu("CALCULADORA BASICA", opciones); // Mostrar menú y obtener la opción elegida (1, 2 o 3)
                switch (menu)
                {
                    case 1: OperarVectores('+'); break;
                    case 2: OperarVectores('-'); break;
                    case 3: Console.WriteLine("¡Gracias por usar la calculadora!"); break;
                    default: Console.WriteLine("opcion invalida"); break;
                }
            } while (menu != 3); // Repetir hasta que elija "Salir"
            Console.ReadKey();
        }
        // Dibuja un menú con bordes y retorna la opción elegida por el usuario.
        // Número de opción seleccionada (1-indexado) o -1 si la entrada no es válida.
        // Limpia la consola antes de mostrar el menú para una experiencia limpia.
        static int MostrarMenu(string titulo, string[] opciones)
        {
            Console.Clear();
            Console.WriteLine($"╔{new string('═', 65)}╗");
            Console.WriteLine($"║                   {titulo,-45} ║");
            Console.WriteLine($"╚{new string('═', 65)}╝");
            for (int i = 0; i < opciones.Length; i++) Console.WriteLine($" {i + 1}. {opciones[i]}"); // Mostrar cada opción con su número
            Console.Write("ELIJA UNA OPCION: ");
            string input = Console.ReadLine() ?? "";
            return int.TryParse(input, out int opcion) ? opcion : -1; // Intentar convertir a número. Si falla, retorna -1
        }
        // Permite al usuario seleccionar la base numérica para la operación.
        // 2 = Binario, 8 = Octal, 16 = Hexadecimal, 0 = Salir al menú principal.
        // Muestra un submenú específico y se repite hasta que el usuario elija una opción válida.
        static int SeleccionarBase()
        {
            string[] opciones = { "Binario", "Octal", "Hexadecimal", "Salir" };
            int menu;
            do
            {
                menu = MostrarMenu("SELECCIONAR BASE", opciones);
                // Convertir la opción numérica a la base correspondiente
                if (menu == 1) return 2;
                if (menu == 2) return 8;
                if (menu == 3) return 16;
                if (menu == 4) return 0;
            } while (true); // Si es inválido, el ciclo se repite
        }
        // Solicita al usuario un número en la base especificada y lo valida.
        // String de exactamente 8 caracteres, relleno con ceros a la izquierda si es necesario.
        // Validaciones realizadas:
        // - No puede estar vacío
        // - Máximo 8 dígitos (bits)
        // - Solo caracteres válidos según la base (01 para binario, 01234567 para octal, 0-9A-F para hex)
        static string PedirNumero(string n, int baseSistema)
        {
            bool valido;
            string numero, tipo = "", caracteresValidos = "";
            // Configurar caracteres permitidos según la base
            if (baseSistema == 2) { caracteresValidos = "01"; tipo = "Binario"; }
            if (baseSistema == 8) { caracteresValidos = "01234567"; tipo = "Octal"; }
            if (baseSistema == 16) { caracteresValidos = "0123456789ABCDEF"; tipo = "Hexadecimal"; }
            do
            {
                Console.Write($"Ingrese el {n} número {tipo} (maximo 8bits): ");
                numero = Console.ReadLine()?.ToUpper() ?? ""; // Convertir a mayúsculas para hexadecimal
                valido = true;
                // Validar cada carácter individualmente
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
            return numero.PadLeft(8, '0'); // completado con ceros a la izquierda
        }
        // Convierte un string numérico en un arreglo de enteros, respetando la base.
        // Los dígitos se ubican alineados a la derecha (relleno implícito con ceros a la izquierda).
        // En hexadecimal, las letras A-F se convierten a 10-15.
        static void LlenarVector(string numero, int[] vector, int baseSistema)
        {
            int pos = 8 - numero.Length;
            for (int i = 0; i < numero.Length; i++)
            {
                if (baseSistema == 16) vector[pos + i] = Convert.ToInt32(numero[i].ToString(), 16); // Convertir carácter hexadecimal a valor numérico (A=10, B=11, etc.)
                else vector[pos + i] = numero[i] - '0'; // Para binario y octal: restar '0' (código ASCII 48) para obtener el entero
            }
        }
        // Realiza la suma de dos vectores dígito por dígito
        /// Realiza la suma de dos operandos dígito por dígito en la base especificada.
        /// Algoritmo:
        /// 1. Suma de derecha a izquierda (desde el dígito menos significativo)
        /// 2. Maneja acarreo (carry) dividiendo por la base
        /// 3. El resultado temporal tiene 9 posiciones para posible desbordamiento
        /// 4. Si no hay desbordamiento, se elimina el primer dígito (cero)
        /// 
        /// Ejemplo en decimal: 5 + 6 = 11 (acarreo 1, resultado 1)
        /// En binario: 1 + 1 = 10 (acarreo 1, resultado 0)
        /// </remarks>
        static void Sumar(int baseSistema)
        {
            int acarreo = 0;
            int[] sumaTemp = new int[9];  // Temporal de 9 posiciones
            for (int i = 7; i >= 0; i--)
            {
                int suma = vector1[i] + vector2[i] + acarreo;
                sumaTemp[i + 1] = suma % baseSistema; // Dígito actual
                acarreo = suma / baseSistema; // Nuevo acarreo
            }
            sumaTemp[0] = acarreo;
            // Ajustar el tamaño del resultado: eliminar el primer dígito si es cero
            if (sumaTemp[0] == 0 && sumaTemp.Length > 1)
            {
                resultado = new int[sumaTemp.Length - 1];
                Array.Copy(sumaTemp, 1, resultado, 0, resultado.Length);
            }
            else resultado = sumaTemp;
        }
        // Compara dos vectores numéricamente (como números enteros en la base correspondiente).
        // Comparación dígito por dígito de izquierda a derecha (más significativo primero).
        // Si todos los dígitos son iguales, retorna false (no es mayor, son iguales).
        static bool EsMayor(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > b[i]) return true;
                if (a[i] < b[i]) return false;
            }
            return false; // Son iguales
        }
        // Realiza la resta de dos vectores dígito por dígito en la base especificada.
        // resta con préstamo:
        //  Se determina cuál vector es mayor para evitar resultados negativos
        //  Se resta siempre el menor del mayor (valor absoluto)
        //  Si un dígito es menor que el otro, se "pide prestado" sumando la base
        //  El préstamo se propaga al siguiente dígito
        /// 
        // NOTA: Esta implementación NO maneja números negativos. Siempre resta:
        // mayor - menor = resultado positivo o cero.
        static void Restar(int baseSistema)
        {
            resultado = new int[8]; // Reiniciar resultado (por si venía de una suma con tamaño 9)
            bool vector1Mayor = EsMayor(vector1, vector2); // Determinar cuál vector es mayor para evitar resultados negativos
            // Asignar mayor y menor usando el operador ternario
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
        // Método principal: gestiona el flujo completo de una operación.
        // Flujo de ejecución:
        //  Selección de base numérica
        //  Limpieza de vectores globales
        //  Solicitud y validación de los dos números
        //  Conversión a vectores de dígitos
        //  Ejecución de suma o resta
        //  Visualización de resultados en formato legible
        static void OperarVectores(char signo)
        {
            int baseSistema = SeleccionarBase(); // Obtener la base numérica seleccionada por el usuario
            // Limpiar vectores globales (eliminar residuos de operaciones anteriores)
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
        // Convierte un vector de dígitos en una representación string legible.
        // String formateado sin ceros a la izquierda y con dígitos en hexadecimal (A-F) si es necesario.
        static string FormatearVector(int[] vector)
        {
            // Convertir cada dígito a su representación hexadecimal (X = letras mayúsculas)
            string resultadoTexto = string.Join("", vector.Select(d => d.ToString("X")));
            // Eliminar ceros a la izquierda
            string resultadoTrimmed = resultadoTexto.TrimStart('0');
            // Si después de eliminar ceros queda vacío, significa que el valor es cero
            return string.IsNullOrEmpty(resultadoTrimmed) ? "0" : resultadoTrimmed;
        }
    }
}
