using System.Data;
using Newtonsoft.Json;

namespace CommonFunction
{
    public class Function
    {
        public string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
    }
}
