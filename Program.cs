internal class Program
{
    [STAThread]
    private static void Main()
    {
        // Вариант 22
        int p = 197, g = 5, x = 6; 
        int y = Exp(g, x, p);
        // 1 + x2 + x3 + x6
        bool[] key = [true, true, true, false, false, true]; 

        while (true)
        {
            Console.WriteLine("0 - Создать подпись");
            Console.WriteLine("1 - Проверить подпись");
            Console.WriteLine("2 - Выход");
            int choice = SafeReadInteger();
            switch (choice)
            {
                case 0:
                    Console.WriteLine("Введите сообщение M: ");
                    byte m = (byte)SafeReadInteger();
                    // Получение значения Хэш-функции
                    byte h = ConvertBoolArrayToByte(CRC(m, key)); 

                    Console.WriteLine("Введите число: ");
                    int k = ChoosingK(SafeReadInteger(), p);

                    int r = Exp(g, k, p);
                    int u = (h - (x * r)) % (p - 1);
                    while (u < 0)
                    {
                        u += p - 1;
                    }

                    int s = Euclid(k, p - 1) * u % (p - 1);

                    //Подписанный документ
                    Console.WriteLine("Ключ r = {0}, Ключ s = {1}", r, s);

                    break;

                case 1:
                    Console.WriteLine("Введите сообщение M: ");
                    byte mCheck = (byte)SafeReadInteger();
                    // Получение значения Хэш-функции
                    byte hCheck = ConvertBoolArrayToByte(CRC(mCheck, key)); 
                    Console.WriteLine("Введите ключ r:");
                    int rCheck = SafeReadInteger();
                    Console.WriteLine("Введите ключ s:");
                    int sCheck = SafeReadInteger();
                    int a = Exp(y, rCheck, p) * Exp(rCheck, sCheck, p) % p;
                    int b = Exp(g, hCheck, p);
                    if (a == b)
                    {
                        Console.WriteLine("Подпись верна");
                    }
                    else
                    {
                        Console.WriteLine("Подпись не верна");
                    }

                    break;

                case 2:
                    return;

                default:
                    Console.WriteLine("Ошибка");
                    break;
            }
        }
    }

    private static bool[] ConvertByteToBoolArray(int b) 
    {
        bool[] result = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            result[i] = (b & (1 << i)) != 0;
        }

        Array.Reverse(result);
        return result;
    }

    private static byte ConvertBoolArrayToByte(bool[] source) 
    {
        byte result = 0;

        int index = 8 - source.Length;

        foreach (bool b in source)
        {

            if (b)
            {
                result |= (byte)(1 << (7 - index));
            }

            index++;
        }

        return result;
    }

    // Возведение в степень и деление с остатком
    private static int Exp(int a, int e, int m) 
    {
        long result = a;
        for (int i = 0; i < e - 1; i++)
        {
            result *= a;

            result %= m;

        }

        return (int)result;
    }

    // Метод для вычисления CRC
    internal static bool[] CRC(byte a, bool[] key)
    {
        bool[] b = ConvertByteToBoolArray(a);
        Array.Resize(ref b, 11); 

        for (int i = 0; i < 8; i++)
        {
            if (b[i])
            {
                for (int j = 0; j < 6; j++)
                {
                    if (i + j < 11) // Проверка на выход за границы массива b
                    {
                        b[i + j] = XOR(b[i + j], key[j]);
                    }
                }
            }
        }

        bool[] c = [b[8], b[9], b[10]];
        return c;
    }

    private static bool XOR(bool a, bool b) 
    {
        bool result = false;
        if (a != b)
        {
            result = true;
        }

        return result;
    }

    private static int NOD(int a, int b) 
    {
        while (b != 0)
        {
            if (a > b)
            {
                a -= b;
            }
            else
            {
                b -= a;
            }
        }

        return a;
    }

    // Выбирается случайное число k (1<k<p-1) взаимно простое с p-1.
    private static int ChoosingK(int a, int p)
    {
        int k = 2;
        for (int i = 0; i < a; i++)
        {
            while (NOD(k, p - 1) != 1)
            {
                k++;
            }

            k++;
        }

        return k - 1;
    }

    private static int SafeReadInteger()
    {
        while (true)
        {
            #pragma warning disable CS8600 
            string sValue = Console.ReadLine();
            if (int.TryParse(sValue, out int iValue))
            {
                return iValue;
            }

            Console.WriteLine("Ошибка");
        }
    }

    // Метод для нахождения обратного элемента по модулю 
    private static int Euclid(int a, int m)
    {
        int r;
        int k = 1;

        while (true)
        {
            k += m;
            if (k % a == 0)
            {
                r = k / a;
                return r;
            }
        }
    }
}
