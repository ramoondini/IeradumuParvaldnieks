using System.Text.Json;
using System.Text.Json.Serialization;
using IeradumuParvaldnieks.Domain.Entities;
using IeradumuParvaldnieks.Domain.TestData;

namespace IeradumuParvaldnieks.Services
{
    public class IeradumuService : IIeradumuService //serviss ieradumu ieladei un saglabasanai
    {
        private readonly string failaCels = Path.Combine(FileSystem.AppDataDirectory, "ieradumi.json"); //cels uz json failu

        private readonly JsonSerializerOptions jsonIestatijumi = //json faila iestatijumi
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter() //Enum vertibas json faila saglabajas ka teksts
                }
            };

        public async Task <List<Ieradums>> IeladetIeradumusAsync() //ielade ieradumus no json faila
        {
            if (!File.Exists(failaCels)) //ja nepastav sis json fails, tad izmanto testa datus
            {
                List<Ieradums> testaIeradumi = TestaDati.IzveidotIeradumus();
                await SaglabatIeradumusAsync(testaIeradumi); //saglaba testa datus json faila

                return testaIeradumi;
            }

            string json = await File.ReadAllTextAsync(failaCels); //nolasa json faila saturu
            List<Ieradums>? ieradumi = JsonSerializer.Deserialize<List<Ieradums>>(json, jsonIestatijumi); //parveido json datus uz ieradumu sarakstu

            return ieradumi ?? new List<Ieradums>(); //ja datu nav, tad atgriez tuksu sarakstu
        }
        public async Task SaglabatIeradumusAsync(List<Ieradums> ieradumi)//saglaba ieradumus json faila
        {
            string json = JsonSerializer.Serialize(//parveido ieradumu sarakstu uz json formatu
                ieradumi, jsonIestatijumi);
            await File.WriteAllTextAsync(failaCels, json);//ieraksta json datus faila
        }
    }
}