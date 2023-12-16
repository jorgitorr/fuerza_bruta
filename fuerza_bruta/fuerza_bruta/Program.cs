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
            //num de hilos que queremos
            int numeroHilos = 4;
            //lee el texto de contrasenias y lo convierte a lista
            List<string>lineas = File.ReadAllLines("2151220-passwords.txt").ToList();
            
            
            var random = new Random();
            //num aleatorio del cual se va a coger la contrasenia
            var numAleatorio = random.Next(lineas.Count);
            //contrasenia que se ha cogido de entre todas las lineas
            var contrasenia = lineas[numAleatorio];
            
            //devuelve la contrasenia encryptada cogiendo la palabra 
            var contraseniaEncryptada = CalcularSha256(contrasenia);
            
            //numero de lineas que va a recorrer cada hilo
            var lineasxHilo = lineas.Count / numeroHilos;
            
            
            for (var i = 0; i < numeroHilos; i++)
            {
                //lista que va a recorrer cada hilo
                var listaReducida = lineas.GetRange(i * lineasxHilo, lineasxHilo - 1);
                //creación de hilos y fuerza bruta sobre cada parte de la lista para cada hilo
                new Thread(() => FuerzaBruta(contraseniaEncryptada,listaReducida)).Start();
            }

        }
        
        
        /// <summary>
        /// devuelve una contrasenia encryptada en hashBytes
        /// </summary>
        /// <param name="input"></param>
        /// <returns>devuelve un String que es la contrasenia encryptada</returns>
        private static string CalcularSha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                //Convierte la cadena en Bytes
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                //Calcula el hash SHA-256
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

        /// <summary>
        /// es un método que recorre la lista introducida por parámetros buscando una contrasenia en la lista
        /// que sea igual a la pasada por parámetros esto se hace con hilos para hacerlo más rápido
        /// ya que cada hilo se ocupa de una parte de las contrasenias
        /// </summary>
        /// <param name="contraseniaEncryptada">es la contrasenia encryptada</param>
        /// <param name="cadenas">son todas las contrasenias posibles</param>
        private static void FuerzaBruta(string contraseniaEncryptada, List<String>cadenas)
        {
            foreach (string password in cadenas)
            {
                if (CalcularSha256(password) == contraseniaEncryptada)
                {
                    Console.WriteLine("La contrasenia encriptada es: " + contraseniaEncryptada);
                    Console.WriteLine("Sin encrytar: " + password);
                    break;
                }
            }
        }
    }
}