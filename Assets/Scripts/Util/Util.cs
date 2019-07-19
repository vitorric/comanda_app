using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Util  {

	public static string formatarDataParaAPI(string data){

        if (!string.IsNullOrEmpty(data))
        {
            string[] campos = data.Split('/');

            return campos[2] + "-" + campos[1] + "-" + campos[0];
        }

        return null;
	}

	public static string FormatarValores(double valor)
    {
        string valorFormatado = string.Empty;

        if (valor == 0)
            valorFormatado = "0";
        else if (valor < 1000)
            valorFormatado = string.Format("{0}", Util.Floor10(valor));
        else if (valor >= 1000 && valor < 999999)
            valorFormatado = string.Format("{0}K", Util.Floor10((valor / 1000)));
        else if (valor > 1000000)
            valorFormatado = string.Format("{0}M", Util.Floor10((valor / 1000000)));


        return valorFormatado;
    }

	public static double Floor10(double x)
    {
        return Math.Floor(x * 10) / 10;
    }
    
    public static string GerarHashMd5(string input)
    {
        MD5 md5Hash = MD5.Create();
        // Converter a String para array de bytes, que é como a biblioteca trabalha.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Cria-se um StringBuilder para recompôr a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop para formatar cada byte como uma String em hexadecimal
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }
    public static DateTime ConverterDataFB(string data)
    {
        if (string.IsNullOrEmpty(data))
            return new DateTime();

        data = data.Replace("T", " ").Substring(0, data.IndexOf("."));
        return DateTime.ParseExact(data, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public static string GetExceptionDetails(Exception exception)
    {
        PropertyInfo[] properties = exception.GetType().GetProperties();
        List<string> fields = new List<string>();
        foreach (PropertyInfo property in properties)
        {
            object value = property.GetValue(exception, null);
            fields.Add(String.Format(
                             "{0} = {1}",
                             property.Name,
                             value != null ? value.ToString() : String.Empty
            ));
        }
        return String.Join("\n", fields.ToArray());
    }

}
