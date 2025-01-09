using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace TemplateTool.PS
{
    public static class Basic
    {
        /// <summary>
        /// The last exception that occurred in the PS.Basic class
        /// </summary>
        public static Exception LastException = null;

        /// <summary>
        /// Collection of PowerShell runtime errors
        /// </summary>
        public static PSDataCollection<ErrorRecord> LastErrors =
                                                    new PSDataCollection<ErrorRecord>();

        /// <summary>
        /// Auxiliary Property that helps to check if there was an error and 
        /// resets the error state
        /// </summary>
        public static bool HasError
        {
            get
            {
                return LastException != null;
            }
            set
            {
                if (!value)
                {
                    LastException = null;
                    LastErrors = new PSDataCollection<ErrorRecord>();
                }
            }
        }

        /// <summary>
        /// A helper Property to help you get the error code
        /// </summary>
        public static int ErrorCode
        {
            get
            {
                if (HasError) return int.Parse(LastException.Message.Substring(1, 4));
                return 0;
            }
        }

        /// <summary>
        /// Basic method of calling PowerShell a script where all commands 
        /// and their data must be presented as one line of text
        /// </summary>
        /// <param name="ps">PowerShell environment</param>
        /// <param name="psCommand">A single line of text containing commands 
        /// and their parameters (in text format)</param>
        /// <param name="outs">A collection of objects that contains the feedback</param>
        /// <returns>The method returns true when executed correctly 
        /// and false when some errors have occurred</returns>
        public static bool RunPS(PowerShell ps, string psCommand, out Collection<PSObject> outs)
        {
            //Programmer's Commandment I: Remember to reset your variables
            outs = new Collection<PSObject>();
            HasError = false;

            //Cleanup of PowerShell also due to commandment I
            ps.Commands.Clear();
            ps.Streams.ClearStreams();

            //We put the script into the PowerShell environment 
            //along with all commands and their parameters
            ps.AddScript(psCommand);

            //We are trying to execute our command
            outs = ExecutePS(ps);

            //The method returns true when executed correctly and false 
            //when some errors have occurred
            return !HasError;
        }
        public static string RunScript(string scriptText)
        {
            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            // open it
            runspace.Open();
            // create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);
            pipeline.Commands.Add("Out-String");

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();
            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Method 2 cmdlet call where we can only give one command 
        /// at a time and its parameters are passed as Name/Value pairs,
        /// where values can be of any type
        /// </summary>
        /// <param name="ps">PowerShell environment</param>
        /// <param name="psCommand">Single command with no parameters</param>
        /// <param name="outs">A collection of objects that contains the feedback</param>
        /// <param name="parameters">A collection of parameter pairs 
        /// in the form Name/Value</param>
        /// <returns>The method returns true when executed correctly 
        /// and false when some errors have occurred</returns>
        public static bool RunPS(PowerShell ps, string psCommand,
               out Collection<PSObject> outs, params ParameterPair[] parameters)
        {
            //Programmer's Commandment I: Remember to reset your variables
            outs = new Collection<PSObject>();
            HasError = false;

            if (!psCommand.Contains(' '))
            {
                //Cleanup of PowerShell also due to commandment I
                ps.Commands.Clear();
                ps.Streams.ClearStreams();

                //We put a single command into the PowerShell environment
                ps.AddCommand(psCommand);

                //Now we enter the command parameters in the form of Name/Value pairs
                foreach (ParameterPair PP in parameters)
                {
                    if (PP.Name == null || PP.Name == String.Empty)
                    {
                        LastException = new Exception("E1008:Parameter cannot be unnamed");
                        return false;
                    }

                    if (PP.Value == null) ps.AddParameter(PP.Name);
                    else ps.AddParameter(PP.Name, PP.Value);
                }

                //We are trying to execute our command
                outs = ExecutePS(ps);
            }
            //And here we have a special exception if we tried 
            //to apply the method not to a single command
            else LastException = new Exception("E1007:Only one command with no parameters is allowed");

            //The method returns true when executed correctly and false 
            //when some errors have occurred
            return !HasError;
        }

        /// <summary>
        /// Internal method in which we try to execute a script or command with parameters
        /// This method does not need to return a fixed value that indicates 
        /// whether or not the execution succeeded,
        /// since the parent methods use the principal properties of the class set in it.
        /// </summary>
        /// <param name="ps">PowerShell environment</param>
        /// <returns>A collection of objects that contains the feedback</returns>
        private static Collection<PSObject> ExecutePS(PowerShell ps)
        {
            Collection<PSObject> retVal = new Collection<PSObject>();

            //We are trying to execute our script
            try
            {
                retVal = ps.Invoke();

                // ps.HadErrors !!! NO!
                // The PowerShell environment has a special property 
                // that indicates in the assumption whether errors have occurred
                // unfortunately, most often, I have found that despite errors 
                // its value is false or vice versa,
                // in the absence of errors, it pointed to the truth.

                // Therefore, we check the fact that errors have occurred, 
                // using the error counter in PowerShell.Streams
                if (ps.Streams.Error.Count > 0) //czy są błędy wykonania
                {
                    //We create another general exception, but we do not raise it.
                    LastException = new Exception("E0002:Errors were detected during execution");

                    //And we write runtime errors to the LastErrors collection
                    LastErrors = new PSDataCollection<ErrorRecord>(ps.Streams.Error);
                }
            }
            //We catch script execution errors and exceptions
            catch (Exception ex)
            {
                //And if they do, we create a new general exception but don't raise it
                LastException = new Exception("E0001:" + ex.Message);
            }

            //Returns a collection of results
            return retVal;
        }
    }

    /// <summary>
    /// Class defining the PowerShell parameter in the form Name/Value.
    /// it can be replaced with any convenient dictionary class
    /// </summary>
    public class ParameterPair
    {
        public string Name { get; set; } = string.Empty;

        public object Value { get; set; } = null;
    }
}