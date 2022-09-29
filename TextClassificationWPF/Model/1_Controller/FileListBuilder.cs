using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextClassificationWPF.Domain;
using TextClassificationWPF.FileIO;

namespace TextClassificationWPF.Controller
{
    public class FileListBuilder:AbstractFileListsBuilder
    {
        const string AFOLDERNAME = "ClassA";
        const string BFOLDERNAME = "ClassB";

        private FileLists _fileLists;
        private FileAdapter _fileAdapter;

        public FileListBuilder()
        {
            _fileLists = new FileLists();

            _fileAdapter = new TextFile("txt");
        }

        public override FileLists GetFileLists()
        {
            return _fileLists;
        }

        public override void GenerateFileNamesInA()
        { 
            List<string> fileNames = _fileAdapter.GetAllFileNames(AFOLDERNAME);
            _fileLists.SetA(fileNames);
        }

        public override void GenerateFileNamesInB()
        {
            List<string> fileNames = _fileAdapter.GetAllFileNames(BFOLDERNAME);
            _fileLists.SetB(fileNames);
        }
    }
}
