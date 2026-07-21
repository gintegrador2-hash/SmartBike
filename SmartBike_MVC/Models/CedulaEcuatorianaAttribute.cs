using System.ComponentModel.DataAnnotations;

namespace SmartBike_MVC.Models
{
    /// <summary>
    /// Valida que el valor sea una cédula ecuatoriana real,
    /// usando el algoritmo oficial del dígito verificador (módulo 10).
    /// </summary>
    public class CedulaEcuatorianaAttribute : ValidationAttribute
    {
        public CedulaEcuatorianaAttribute()
        {
            ErrorMessage = "La cédula ingresada no es una cédula ecuatoriana válida.";
        }

        public override bool IsValid(object? value)
        {
            var cedula = value as string;

            // 1. Debe tener exactamente 10 dígitos numéricos
            if (string.IsNullOrWhiteSpace(cedula) || cedula.Length != 10 || !cedula.All(char.IsDigit))
                return false;

            // 2. Código de provincia válido (01 a 24)
            int provincia = int.Parse(cedula.Substring(0, 2));
            if (provincia < 1 || provincia > 24)
                return false;

            // 3. Tercer dígito menor a 6 (personas naturales)
            int tercerDigito = cedula[2] - '0';
            if (tercerDigito >= 6)
                return false;

            // 4. Algoritmo módulo 10 con coeficientes 2,1,2,1,2,1,2,1,2
            int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int suma = 0;

            for (int i = 0; i < 9; i++)
            {
                int producto = (cedula[i] - '0') * coeficientes[i];
                if (producto > 9) producto -= 9;
                suma += producto;
            }

            int digitoVerificador = cedula[9] - '0';
            int decenaSuperior = ((suma + 9) / 10) * 10;
            int verificadorCalculado = decenaSuperior - suma;
            if (verificadorCalculado == 10) verificadorCalculado = 0;

            // 5. El décimo dígito debe coincidir con el calculado
            return digitoVerificador == verificadorCalculado;
        }
    }
}