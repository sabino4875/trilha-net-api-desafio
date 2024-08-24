using System;

namespace TrilhaApiDesafio.Helpers
{
    /// <summary>
    /// Classe contendo rotinas utilizadas no sistema
    /// </summary>
    public class ApplicationRoutines
    {
        /// <summary>
        /// Verifica se um campo String contem algum valor informado.
        /// </summary>
        /// <param name="value">Valor a ser verificado</param>
        /// <returns></returns>
#pragma warning disable CA1822 // Mark members as static
        public Boolean ContainsValue(String value)
        {
            if (String.IsNullOrEmpty(value)) return false;
            if (String.IsNullOrWhiteSpace(value)) return false;
            if (value.Trim().Length < 1) return false;
            return true;
        }
#pragma warning restore CA1822 // Mark members as static
    }
}
