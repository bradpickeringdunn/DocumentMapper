using System;
using System.Runtime.InteropServices;

namespace DocumentMapper.Word.AddIn
{
    public static class ApplicationVariables
    {
        public static string GetVariable(string variableName)
        {
            var variable = string.Empty;

            try
            {
                variable = Globals.ThisAddIn.Application.ActiveDocument.Variables[variableName].Value;
            }
            catch(Exception ex){
            
                if(ex.GetType() != typeof(COMException))
                {                    
                    throw ex;
                }

            }

            return variable;
        }

        public static void SetVariable(string variablename, ref object value)
        {
            Globals.ThisAddIn.Application.ActiveDocument.Variables.Add(variablename, value);     
        }

        public static string DocumentMapFilePath
        {
            get { return "DocumentMapFilePath";  }
        }
    }
}
