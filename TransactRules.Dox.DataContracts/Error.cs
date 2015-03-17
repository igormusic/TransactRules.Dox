using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactRules.Dox.DataContracts
{

  public class ErrorItem
{
    public string domain { get; set; }
    public string reason { get; set; }
    public string message { get; set; }
    public string locationType { get; set; }
    public string location { get; set; }
}

public class Error
{
    public List<ErrorItem> errors { get; set; }
    public int code { get; set; }
    public string message { get; set; }
}


}
