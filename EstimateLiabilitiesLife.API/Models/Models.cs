
namespace EstimateLiabilitiesLife.API.Models;

internal class Contract
{
    public int contractNo { get; set; }
    public DateTime valueDate { get; set; }
    public DateTime birthDate { get; set; }
    public string sex { get; set; } // M or F
    public int z { get; set; }
    public double guarantee { get; set; }
    public int payPeriod { get; set; }
    public string table { get; set; } // AP or APG

    // Convert to F# EstimateLiabilitiesLife.Contract
    public Insurance.Contract InsuranceContract()
    {
        var contract = new Insurance.Contract(contractNo, birthDate, ConvertSex(sex), z, guarantee, payPeriod,
            ConvertTable(table));
        return contract;
    }

    private Mortality.Sex ConvertSex(string sex)
    {
        return sex == "M" ? Mortality.Sex.M : Mortality.Sex.F;
    }

    private Insurance.Table ConvertTable(string table)
    {
        return table == "AP" ? Insurance.Table.AP : Insurance.Table.APG;
    }
}

