namespace ChallengeV1.Models
{
    public class Value
    {
        public decimal MarketValue { get; set; }
        public decimal AuctionValue { get; set; }

        public override string ToString()
        {
            return $"Market Value: {MarketValue:C4}, Auction Value: {AuctionValue:C4}.";
        }
    }
}
