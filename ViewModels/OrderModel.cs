using Newtonsoft.Json;

namespace updateorder.ViewModels;

public class Detail
{
    [JsonProperty("orderid")]
    public string Orderid { get; set; }

    [JsonProperty("itemid")]
    public int Itemid { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("priceperitem")]
    public double Priceperitem { get; set; }

    public Detail(string orderid, int itemid, int quantity, double priceperitem)
    {
        Orderid = orderid;
        Itemid = itemid;
        Quantity = quantity;
        Priceperitem = priceperitem;
    }
}

public class OrderModel
{
    [JsonProperty("orderid")]
    public string Orderid { get; set; }

    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("createdat")]
    public DateTime Createdat { get; set; }

    [JsonProperty("orderstatus")]
    public string Orderstatus { get; set; }

    [JsonProperty("details")]
    public List<Detail> Details { get; set; }
    

    public OrderModel(string orderid, string userId, DateTime createdat, string orderstatus, List<Detail> details)
    {
        Orderid = orderid;
        UserId = userId;
        Createdat = createdat;
        Orderstatus = orderstatus;
        Details = details;
    }
}