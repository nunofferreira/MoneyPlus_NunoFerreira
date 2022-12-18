namespace MoneyPlus.Data.Serialize;

public class DataBase
{
    public List<string> CategoryList { get; set; }
    public List<string> SubCategoryList { get; set; }

    public DataBase()
    {
        CategoryList = new List<string>();
        SubCategoryList = new List<string>();
    }

    public void Serialize(string filename)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var stringResult = serializer.Serialize(this);

        File.WriteAllText(filename, stringResult);
    }
}