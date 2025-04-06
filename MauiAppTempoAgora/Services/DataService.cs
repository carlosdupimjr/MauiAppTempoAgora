﻿using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "a36401d440ad5c895a2da8772f6a91da";
            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();

                        var rascunho = JObject.Parse(json);

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            main = (string)rascunho["weather"][0]["main"],
                            description = (string)rascunho["weather"][0]["description"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString(),
                            name = (string)rascunho["name"],
                            cod = (int)rascunho["cod"]

                        };// Fecha objeto do tempo
                    }// Fecha IF se o status do servidor for de sucesso
                }
                catch
                {
                    throw new HttpRequestException("Não foi possível conectar ao servidor. Verifique sua conexão.");
                }
            }// Fecha laço using

            return t;
        }
    }
}
