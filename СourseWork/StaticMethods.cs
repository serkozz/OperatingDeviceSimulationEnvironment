using System;

namespace СourseWork
{
    static class StaticMethods
    {
        // Преобразует число в строку битов
        // Длина полученной строки больше или равна указанной
        public static string GetString(uint value, int lenght)
        {
            var result = Convert.ToString(value, 2);
            if (result.Length < lenght)
            {
                result = new string('0', lenght - result.Length) + result;
            }
            return result;
        }

        // Извлекает подстроку указанного диапазона битов переданного числа
        public static string GetSubstring(uint value, int start, int end)
        {
            var tempString = GetString(value, ++start );
            var substring = tempString[^start..^end];
            return substring;
        }

        // Возвращает значение бита числа
        public static int GetBit(uint value, int index)
        {
            var tempString = GetString(value, ++index);
            var ch = tempString[^index];
            var result = Convert.ToInt32(ch.ToString());
            return result;
        }

        // Посимвольная инверсия битов в строке
        public static string Invert(string str)
        {
            var array = str.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '1')
                {
                    array[i] = '0';
                }
                else
                {
                    array[i] = '1';
                }
            }
            return new string(array);
        }
    }
}
