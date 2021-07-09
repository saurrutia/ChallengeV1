using ChallengeV1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ChallengeV1
{
    class Program
    {
        static void Main(string[] args)
        {
            LogResult(GetValueByIdAndYear, "67352", 2007);
            LogResult(GetValueByIdAndYear, "87964", 2011);
        }

        static void LogResult(Func<string, int, Value> function, string id, int year)
        {
            try
            {
                var result = function.Invoke(id, year);
                Console.WriteLine(result.ToString());
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        static Value GetValueByIdAndYear(string id, int year)
        {            
            var json = File.ReadAllText(Path.Combine("Data", "api-response.json"));
            var equipmentSetJObject = JObject.Parse(json);

            var equipmentSet = equipmentSetJObject.Value<JObject>(id);
            if (equipmentSet == null)
            {
                throw new Exception($"Equipment set with id: {id} not found.");
            }

            var cost = equipmentSet.Value<JObject>("saleDetails")?.Value<decimal>("cost");
            if (cost == null)
            {
                throw new Exception($"Cost of equipment set with id: {id} not found.");
            }

            var equipmentSetSchedule = equipmentSet?.Value<JObject>("schedule")
                ?.Value<JObject>("years")
                ?.Value<JObject>(year.ToString());

            if (equipmentSetSchedule == null)
            {
                throw new Exception($"Schedule data for equipment set with id: {id} and year {year} not found.");
            }

            var marketRatio = equipmentSetSchedule.Value<decimal?>("marketRatio");
            if(marketRatio == null)
            {
                throw new Exception($"Market ratio for equipment set with id: {id} and year {year} not found.");
            }
            var auctionRatio = equipmentSetSchedule.Value<decimal?>("auctionRatio");
            if (auctionRatio == null)
            {
                throw new Exception($"Auction ratio for equipment set with id: {id} and year {year} not found.");
            }

            return new Value { AuctionValue = cost.Value * auctionRatio.Value, MarketValue = cost.Value * marketRatio.Value };
        }
    }
}
