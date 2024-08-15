using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using base64.Model;


namespace base64
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pasar texto del archivo a una variable
            string encodedString = ObtenerCadenaDeArchivo(AppDomain.CurrentDomain.BaseDirectory + "stringBase64_v1.txt");

            // Obtener cadena decodificada
            string decodedString = DecodificarBase64(encodedString);

            // Transformar cadena en objeto Json
            var personas = ConvertirStringEnJson(decodedString);

            // Mujeres casadas mayores de 50 que viven en Miami
            var q1 = personas
                        .Where( p => 
                                p.sexo == "F" && 
                                !p.soltero && 
                                (DateTime.Today.Year - p.fechaNacimiento.Year) > 50 &&
                                p.ciudad == "Miami" )
                        .Count();


            // En el estado de Texas cual es el sueldo promedio de los profesores
            var q2 = personas
                        .Where(p =>
                               p.estado == "Texas" &&
                               p.profesion == "Teacher"
                        )
                        .Average(p => p.salario);


            // Cual es la ciudad con mas personas
            var q3 = (personas
                        .GroupBy(p => p.ciudad)
                        .Select(c => new { ciudad = c.Key, cantidad = c.Count() }))
                        .OrderByDescending(city => city.cantidad)
                        .First();


            // Impresión de resultados
            Console.WriteLine("Mujeres casadas mayores de 50 que viven en Miami: {0}", q1);
            Console.WriteLine("Sueldo promedio de profesores de Texas: {0}", q2);
            Console.WriteLine("Ciudad con más personas: {0}. Cantidad {1}", q3.ciudad, q3.cantidad);
            
            Console.Read();
        }


        private static string ObtenerCadenaDeArchivo(string RutaArchivoTXT)
        {
            string textoArchivo = System.IO.File.ReadAllText(RutaArchivoTXT);
            return textoArchivo;
        }

        private static string DecodificarBase64(string encodedString)
        {
            var bytes = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(bytes);
            return decodedString;
        }

        private static List<Persona> ConvertirStringEnJson(string stringPersonas)
        {
            return JsonConvert.DeserializeObject<List<Persona>>(stringPersonas);
        }

    }
}
