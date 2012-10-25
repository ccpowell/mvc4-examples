using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace ModelExaminer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

            ParseModel();

        }

        private class FunctionModel
        {
            public string Name { get; set; }
            public string ImportName { get; set; }

            public string SprocName { get; set; }
            public string Schema { get; set; }
        }

        private FunctionModel ParseFunction(XElement x)
        {
            string spname = x.Attribute("Name").Value;
            if (x.Attribute("StoreFunctionName") != null)
            {
                spname = x.Attribute("StoreFunctionName").Value;
            }
            return new FunctionModel()
            {
                Name = x.Attribute("Name").Value,
                SprocName = spname,
                Schema = x.Attribute("Schema").Value
            };
        }


        private void ParseModel()
        {
            var xdoc = XDocument.Load("..\\..\\..\\Trips4.Data\\Models\\Trips.edmx");
            var xroot = xdoc.Root;
            var xnsEdmx = xroot.Name.Namespace;
            XNamespace xnsMappings = "http://schemas.microsoft.com/ado/2008/09/mapping/cs";
            XNamespace xnsSchema = "http://schemas.microsoft.com/ado/2009/02/edm/ssdl";

            // Functions
            var xschema = xroot.Element(xnsEdmx + "Runtime").Element(xnsEdmx + "StorageModels").Element(xnsSchema + "Schema");
            var xfunctions = xschema.Elements(xnsSchema + "Function");
            var functions = xfunctions.OrderBy(x => x.Attribute("Name").Value).Select(x => ParseFunction(x)).ToArray();
            dgFunctionImports.ItemsSource = functions;

            // Function Imports
            var xmappings = xroot.Element(xnsEdmx + "Runtime").Element(xnsEdmx + "Mappings");
            var xmapping = xmappings.Element(xnsMappings + "Mapping");
            var xContainerMapping = xmapping.Element(xnsMappings + "EntityContainerMapping");
            var xfunctionImportMappings = xContainerMapping.Elements(xnsMappings + "FunctionImportMapping");

            var fim = xfunctionImportMappings.OrderBy(x => x.Attribute("FunctionName").Value).Select(x => new FunctionModel()
            {
                Name = x.Attribute("FunctionName").Value.Replace("TripsModel.Store.", String.Empty),
                ImportName = x.Attribute("FunctionImportName").Value,
                Schema = "?"
            }).ToArray();

            foreach (var f in fim)
            {
                var function = functions.Where(func => func.Name == f.Name).FirstOrDefault();
                if (function != null)
                {
                    f.Schema = function.Schema;
                    f.SprocName = function.SprocName;
                }
            }

            dgFunctionImportMappings.ItemsSource = fim;
        }
    }
}
