using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace fuerza_bruta
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int numeroHilos = 4;
            //lee el texto de contrasenias y lo convierte a lista
            List<string>lineas = File.ReadAllLines("2151220-passwords.txt").ToList();

            var random = new Random();
            var numAleatorio = random.Next(lineas.Count);

            //devuelve la contrasenia encryptada cogiendo la palabra 
            var contraseniaEncryptada = CalcularSHA256(lineas[numAleatorio]);
            
            //numero de lineas que va a recorrer cada hilo
            var lineasxHilo = lineas.Count / numeroHilos;
            
            //creacion de hilos
            for (var i = 0; i < numeroHilos; i++)
            {
                new Thread(() => fuerzaBruta(i*lineasxHilo, lineasxHilo-1, contraseniaEncryptada, lineas)).Start();
            }

        }
        
        private static string CalcularSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convierte la cadena a bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Calcula el hash SHA-256
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convierte el resultado a una cadena hexadecimal
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        private static void fuerzaBruta(int inicio, int final, string contrasenia, List<String>cadenas)
        {
            bool encontrada = false;
            
            for (int i = inicio; inicio <= final; i++)
            {
                if (CalcularSHA256(cadenas[i]) == CalcularSHA256(contrasenia) && !encontrada)
                {
                    Console.WriteLine("La contrasenia encriptada es: " + contrasenia);
                    encontrada = !encontrada;
                    break;
                }
            }            
        }
    }
}