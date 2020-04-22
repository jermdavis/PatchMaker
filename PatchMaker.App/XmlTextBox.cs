using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PatchMaker.App
{

    public class XmlTextBox : RequiredFieldTextBox
    {
        public XmlTextBox()
        {
            this.Multiline = true;
            this.AcceptsReturn = true;
            PerformValidation = s => {
                var xml = XDocument.Parse(s);
            };
        }
    }

}   