﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace management_system
{
    // Static class to handle file editting
    
    public static class FileEditor
    {
        //VARIABLES
        private static GeneralFile file = null; //current text file
        public enum FileType { //currently recognized file types
            noFile = 0, //no file currently open
            general = 1, 
            text = 2, 
            xml = 3, 
            databaseTable = 3,
            rtf = 4
        };
        public static string[] extension = { //currently recognized file extensions - this string array matches the 'FileType' enum
                        null, //no file currently open
                        "", //general file type
                        ".txt", //text file
                        ".xml", //XML file
                        ".tbl", //locally stored database table - custom format & fileExtension: .tbl
                        ".rtf" //rich text format
                        };
        //METHODS

        //determine the file type based on the given file extension
        public static FileType determineFilType(string fileExtension)
        {

            switch(fileExtension)
            {
                case ".txt":
                    return FileType.text;

                case "..xml":
                    return FileType.xml;

                case ".tbl":
                    return FileType.databaseTable;

                case ".rtf":
                        return FileType.rtf;

                default: //general file -> unrecognized file extension
                    return FileType.general;
            }

            return FileType.noFile;
        }
    }
}