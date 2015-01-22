using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace UnitTestHost
{
    internal class SelectionStorage
    {
        private Assembly FAssembly;
        private TestRunner FRunner;
        private string FFilePath;

        private List<string> FUnSelected = new List<string>();

        public SelectionStorage(Assembly AAssembly, TestRunner ARunner)
        {
            FAssembly = AAssembly;
            FRunner = ARunner;
            string LFileForlder = string.Format(@"{0}\CCH\AddinUnitTests\{1} Tests",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                AAssembly.FullName);
            Directory.CreateDirectory(LFileForlder);
            FFilePath = string.Format(@"{0}\UnselectedTests.txt", LFileForlder);
            if (File.Exists(FFilePath))
            {
                var LLines = File.ReadAllLines(FFilePath);
                foreach(var LLine in LLines)
                    FUnSelected.Add(LLine);
            }
        }

        private void LoadSelectionToList()
        { 
            FUnSelected.Clear();
            for (int i = 0; i < FRunner.lstTests.Items.Count; ++i)
            {
                var LItem = FRunner.lstTests.Items[i];
                if (!LItem.Checked)
                { 
                    string LClassName = LItem.SubItems[1].Text;
                    string LMethodName = LItem.SubItems[2].Text;
                    FUnSelected.Add(LClassName + "." + LMethodName);
                }
            }
        }

        public void StoreTestSelection()
        {
            LoadSelectionToList();
            File.WriteAllLines(FFilePath, FUnSelected.ToArray());
        }

        public bool IsUnChecked(string AClassName, string AMethodName)
        {
            string LFullName = string.Format("{0}.{1}", AClassName, AMethodName);
            return FUnSelected.IndexOf(LFullName) >= 0;
        }
    }
}
