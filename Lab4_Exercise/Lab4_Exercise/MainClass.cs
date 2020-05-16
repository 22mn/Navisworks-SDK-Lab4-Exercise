using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wf = System.Windows.Forms;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.DocumentParts;


namespace Lab4_Exercise
{
    [PluginAttribute("Lab4_Exercise","TwentyTwo",DisplayName ="Lab4_Exec", ToolTip ="Lab4 exercise project")]
    public class MainClass : AddInPlugin
    {
        // implement execute method 
        public override int Execute(params string[] parameters)
        {
            // current document
            Document doc = Application.ActiveDocument;
            // model items collection-1
            ModelItemCollection itemCollection = new ModelItemCollection();
            // get current selected items
            ModelItemCollection selectionItems = doc.CurrentSelection.SelectedItems;
            // get appended models 
            DocumentModels models = doc.Models;
            // display message
            //string message = "";
            
            // each model
            foreach (Model model in models)
            {
                // collect all items from the mode1
                // add to model item collection-1
                itemCollection.AddRange(ItemsFromRoot(model));
            }
            
            // model item collection-2
            ModelItemCollection itemsToColor = new ModelItemCollection();
            
            // each item from model item collection-1
            foreach(ModelItem item1 in itemCollection)
            {
                // get item1 bounding box
                BoundingBox3D box1 = item1.BoundingBox(true);

                // each item from the current selected items
                foreach (ModelItem item2 in selectionItems)
                {
                    // get item2 bounding box
                    BoundingBox3D box2 = item2.BoundingBox(true);
                    // check intersection of box1 vs box2
                    if (box1.Intersects(box2))
                    {
                        //message += item1.DisplayName + "Intersects " + item2.DisplayName + "\n";
                        // item add to model item collection-2
                        itemsToColor.Add(item1);
                    }
                }
            }
            // change the color of model item collection-2 items 
            doc.Models.OverridePermanentColor(itemsToColor, Color.Green);
            //wf.MessageBox.Show(message);
            return 0;
        }

        // get descendant items from model
        public IEnumerable<ModelItem> ItemsFromRoot(Model model)
        {
            // collect all descendants geometric items from a model 
            return model.RootItem.Descendants.Where(x => x.HasGeometry);
        }
    }
}
