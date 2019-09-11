using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using UIAutomationClientsideProviders;

namespace UiaImplementer
{
    public static class Program
    {
        public static StringBuilder feedbackText = new StringBuilder();
        ///--------------------------------------------------------------------
        /// <summary>
        /// Inserts a string into each text control of interest.
        /// </summary>
        /// <param name="element">A text control.</param>
        /// <param name="value">The string to be inserted.</param>
        ///--------------------------------------------------------------------
        static void InsertTextUsingUIAutomation(AutomationElement element,
                                            string value)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter must not be null");

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + " is not enabled.\n\n");
                }

                // Check #2: Are there styles that prohibit us 
                //           from sending text to this control?
                if (!element.Current.IsKeyboardFocusable)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + "is read-only.\n\n");
                }


                // Once you have an instance of an AutomationElement,  
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern 
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern 
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of 
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //       
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    feedbackText.Append("The control with an AutomationID of ")
                        .Append(element.Current.AutomationId.ToString())
                        .Append(" does not support ValuePattern.")
                        .AppendLine(" Using keyboard input.\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");     // Delete selection
                    SendKeys.SendWait(value);
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                    feedbackText.Append("The control with an AutomationID of ")
                        .Append(element.Current.AutomationId.ToString())
                        .Append((" supports ValuePattern."))
                        .AppendLine(" Using ValuePattern.SetValue().\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    ((ValuePattern)valuePattern).SetValue(value);
                }
            }
            catch (ArgumentNullException exc)
            {
                feedbackText.Append(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                feedbackText.Append(exc.Message);
            }
            finally
            {
                Console.WriteLine(feedbackText.ToString());
            }
        }
        static AutomationElement FindElementFromAutomationID(AutomationElement targetApp,
            string automationID)
        {
            return targetApp.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationID));
        }
        public static void Main(string[] args)
        {

            var name = FindElementFromAutomationID(AutomationElement.RootElement, "ID_NAME");
            //name.SetFocus();
            //Thread.Sleep(100);
            //SendKeys.SendWait("A");

            InsertTextUsingUIAutomation(name, "Adam Stinziani");

            var age = FindElementFromAutomationID(AutomationElement.RootElement, "ID_AGE");
            //age.SetFocus();
            //SendKeys.SendWait("20");

            InsertTextUsingUIAutomation(age, "20");

            var email = FindElementFromAutomationID(AutomationElement.RootElement, "ID_EMAIL");
            //email.SetFocus();
            //SendKeys.SendWait("adamstinziani@gmail.com");
            //Console.WriteLine(name.Current);
            //Thread.Sleep(10000);
            InsertTextUsingUIAutomation(email, "adamstinziani@gmail.com");

        }

        //[DllImport("user32.dll", EntryPoint = "FindWindow")]
        //private static extern int FindWindowEx(string sClass, string sWindow);
        //static void Main(string[] args)
        //{
        //    int nWinHandle = FindWindowEx(null, "MainWindow");
        //    if (nWinHandle == 0)
        //    {
        //        Console.WriteLine("Error");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Success");
        //    }
        //    Thread.Sleep(1000);
        //IntPtr hwnd = Win32.FindWindow("ATH_Note", null);
        //}
    }
}
