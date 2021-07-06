using CsvHelper;
using CsvHelper.Configuration;
//using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Data
    {
        [JsonProperty(PropertyName = "tanggal")]
        public string tanggal { get; set; }
        [JsonProperty(PropertyName = "created_at")]
        public string created_at { get; set; }
        [JsonProperty(PropertyName = "updated_at")]
        public string updated_at { get; set; }
        [JsonProperty(PropertyName = "kabko")]
        public string kabko { get; set; }
        [JsonProperty(PropertyName = "suspect")]
        public string suspect { get; set; }
        [JsonProperty(PropertyName = "probable")]
        public string probable { get; set; }
        [JsonProperty(PropertyName = "suspect_diisolasi")]
        public string suspect_diisolasi { get; set; }
        [JsonProperty(PropertyName = "suspect_discarded")]
        public string suspect_discarded { get; set; }
        [JsonProperty(PropertyName = "confirm")]
        public string confirm { get; set; }
        [JsonProperty(PropertyName = "confirm_bergejala")]
        public string confirm_bergejala { get; set; }
        [JsonProperty(PropertyName = "confirm_tanpa_gejala")]
        public string confirm_tanpa_gejala { get; set; }
        [JsonProperty(PropertyName = "confirm_perjalanan")]
        public string confirm_perjalanan { get; set; }
        [JsonProperty(PropertyName = "confirm_kontak")]
        public string confirm_kontak { get; set; }
        [JsonProperty(PropertyName = "confirm_tanpa_riwayat")]
        public string confirm_tanpa_riwayat { get; set; }
        [JsonProperty(PropertyName = "confirm_selesai")]
        public string confirm_selesai { get; set; }
        [JsonProperty(PropertyName = "confirm_dilacak")]
        public string confirm_dilacak { get; set; }
        [JsonProperty(PropertyName = "kontak_baru")]
        public string kontak_baru { get; set; }
        [JsonProperty(PropertyName = "kontak_suspect")]
        public string kontak_suspect { get; set; }
        [JsonProperty(PropertyName = "kontak_confirm")]
        public string kontak_confirm { get; set; }
        [JsonProperty(PropertyName = "kontak_mangkir")]
        public string kontak_mangkir { get; set; }
        [JsonProperty(PropertyName = "meninggal_rtpcr")]
        public string meninggal_rtpcr { get; set; }
        [JsonProperty(PropertyName = "covidmeninggal_rtpcr")]
        public string covidmeninggal_rtpcr { get; set; }
        [JsonProperty(PropertyName = "bkncovidmeninggal_rtpcr")]
        public string bkncovidmeninggal_rtpcr { get; set; }
        [JsonProperty(PropertyName = "meninggal_probable")]
        public string meninggal_probable { get; set; }
        [JsonProperty(PropertyName = "spesimen_swab")]
        public string spesimen_swab { get; set; }
        [JsonProperty(PropertyName = "rapid")]
        public string rapid { get; set; }
        [JsonProperty(PropertyName = "rapid_reaktif")]
        public string rapid_reaktif { get; set; }
        [JsonProperty(PropertyName = "reaktif_swab")]
        public string reaktif_swab { get; set; }
        [JsonProperty(PropertyName = "reaktif_swab_plus")]
        public string reaktif_swab_plus { get; set; }
        [JsonProperty(PropertyName = "suspectprob_iso_rujukan")]
        public string suspectprob_iso_rujukan { get; set; }
        [JsonProperty(PropertyName = "suspectprob_iso_darurat")]
        public string suspectprob_iso_darurat { get; set; }
        [JsonProperty(PropertyName = "suspectprob_iso_mandiri")]
        public string suspectprob_iso_mandiri { get; set; }
        [JsonProperty(PropertyName = "konfirmasi_iso_rujukan")]
        public string konfirmasi_iso_rujukan { get; set; }
        [JsonProperty(PropertyName = "konfirmasi_iso_darurat")]
        public string konfirmasi_iso_darurat { get; set; }
        [JsonProperty(PropertyName = "konfirmasi_iso_mandiri")]
        public string konfirmasi_iso_mandiri { get; set; }
        [JsonProperty(PropertyName = "suspectprob_iso_gedung")]
        public string suspectprob_iso_gedung { get; set; }
        [JsonProperty(PropertyName = "konfirmasi_iso_gedung")]
        public string konfirmasi_iso_gedung { get; set; }
        [JsonProperty(PropertyName = "ke_iso_mandiri")]
        public string ke_iso_mandiri { get; set; }
        [JsonProperty(PropertyName = "kontak_discarded")]
        public string kontak_discarded { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string t = "";
            string strDate = "";

            for (DateTime d = new DateTime(2020, 3, 20); d <= DateTime.Today; d = d.AddDays(1))
            {
                strDate = d.ToString("yyyy-MM-dd");
                t += GetWeb($"https://covid19dev.jatimprov.go.id/xweb/drax/data/{strDate}/{strDate}").TrimStart('[').TrimEnd(']') + ",";
            }
            t = t.TrimEnd(',');

            File.WriteAllTextAsync($@"C:\temp\JATIM{strDate}.json", $"[{t}]").Wait();

            if (string.IsNullOrEmpty(strDate))
            {
                strDate = DateTime.Today.ToString("yyyy-MM-dd");
            }

            string jsonContent = File.ReadAllText($@"C:\temp\JATIM{strDate}.json");

            List<Data> dt = JsonConvert.DeserializeObject<List<Data>>(jsonContent);

            using (var writer = new StreamWriter($@"C:\temp\JATIM{strDate}.csv"))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(dt);
                }
            }
        }

        static string GetWeb(string url)
        {
            var request = WebRequest.Create(url);

            string text;
            
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                text = sr.ReadToEnd();
            }

            return text;
        }        
    }
}
