namespace MoneyPlus.Data.Serialize;

public class DataBase
{
    public List<Category> CategoryList { get; set; }
    public List<CategoryType> SubCategoryList { get; }

    public DataBase()
    {
        CategoryList = new List<Category>();
        SubCategoryList = new List<CategoryType>();
    }

    //private static string GetPath()
    //{
    //    string path = Environment.CurrentDirectory.Replace(Path.GetRelativePath("../../..", Environment.CurrentDirectory), "");
    //    path = Path.Combine(path, "Categories/", "Categories.yaml");

    //    return path;
    //}

    public void Serialize()
    {
        var DataBase = new DataBase();

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var stringResult = serializer.Serialize(DataBase);
    }
}