using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public static class Db
{
    public static IDbConnection Conn()
    {
        return new SqlConnection(
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
        );
    }
}
