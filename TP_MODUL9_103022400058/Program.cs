using System;
using System.IO;
using System.Text.Json;
using System;

public class SayaMusicTrack
{

}

public class CovidConfigData
{
    public string satuan_suhu { get; set; }
    public int batas_hari_deman { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }
}

public class CovidConfig
{
    public CovidConfigData config;
    private const string filePath = "covid_config.json";

    public CovidConfig()
    {
        try
        {
            ReadConfigFile();
        }
        catch (Exception)
        {
            SetDefault();
            WriteNewConfigFile();
        }
    }

    private void ReadConfigFile()
    {
        string configJsonData = File.ReadAllText(filePath);
        config = JsonSerializer.Deserialize<CovidConfigData>(configJsonData);
    }

    private void SetDefault()
    {
        config = new CovidConfigData()
        {
            satuan_suhu = "celcius",
            batas_hari_deman = 14,
            pesan_ditolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini",
            pesan_diterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini"
        };
    }

    private void WriteNewConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        string jsonString = JsonSerializer.Serialize(config, options);
        File.WriteAllText(filePath, jsonString);
    }

    public void UbahSatuan()
    {
        if (config.satuan_suhu == "celcius")
        {
            config.satuan_suhu = "fahrenheit";
        }
        else if (config.satuan_suhu == "fahrenheit")
        {
            config.satuan_suhu = "celcius";
        }

        WriteNewConfigFile();
    }
}

class Program
{
    static void Main(string[] args)
    {
        CovidConfig covidConfig = new CovidConfig();

        Console.Write($"Berapa suhu badan anda saat ini? Dalam nilai {covidConfig.config.satuan_suhu}: ");
        double suhuBadan = double.Parse(Console.ReadLine());

        Console.Write("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam? ");
        int hariDemam = int.Parse(Console.ReadLine());

        bool kondisiSuhuAman = false;

        if (covidConfig.config.satuan_suhu == "celcius")
        {
            if (suhuBadan >= 36.5 && suhuBadan <= 37.5)
            {
                kondisiSuhuAman = true;
            }
        }
        else if (covidConfig.config.satuan_suhu == "fahrenheit")
        {
            if (suhuBadan >= 97.7 && suhuBadan <= 99.5)
            {
                kondisiSuhuAman = true;
            }
        }

        bool kondisiHariAman = hariDemam < covidConfig.config.batas_hari_deman;

        Console.WriteLine("\n--- HASIL ---");
        if (kondisiSuhuAman && kondisiHariAman)
        {
            Console.WriteLine(covidConfig.config.pesan_diterima);
        }
        else
        {
            Console.WriteLine(covidConfig.config.pesan_ditolak);
        }

        Console.WriteLine("\n--- PERGANTIAN SATUAN ---");
        Console.WriteLine($"Satuan suhu awal: {covidConfig.config.satuan_suhu}");

        covidConfig.UbahSatuan(); 

        Console.WriteLine($"Satuan suhu setelah diubah: {covidConfig.config.satuan_suhu}");
        Console.WriteLine("Coba jalankan ulang program untuk melihat perubahan satuan pada pertanyaan!");
    }
}