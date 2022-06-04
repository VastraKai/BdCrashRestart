using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;


public class Logging
{
    // could be used by other things too
    public string ESC = "\x001B"; // Escape character
    /// <summary>
    /// escColor: Gets the escape code to set a foreground color, or reset the foreground color.
    /// </summary>
    /// <param name="r">The value to be used for Red</param>
    /// <param name="g">The value to be used for Green</param>
    /// <param name="b">The value to be used for Blue</param>
    /// <returns>Foreground color escape code as string</returns>
    public string escColor(string? r = null, string? g = null, string? b = null) // returns a foreground color escape code 
    {                                                                            // (changes the foreground color based on red, green, and blue values)
        if (r != null && g != null && b != null)
        {
            return $"{ESC}[38;2;{r};{g};{b}m";
        }
        else
        {
            return $"{ESC}[0m";
        }
    }
    /// <summary>
    /// logWrite: Writes to the screen with formatting
    /// </summary>
    /// <param name="str">The string to output to the screen</param>
    /// <param name="logtype">The log type to use for the message. (E.x. "INFO", "WARN", etc.)</param>
    public void logWrite(string str, string logtype = "INFO")   // self explanatory  
    {
        str = str.Replace("\n", "\n" + escColor("0", "255", "0") + DateTime.Now +
            escColor("0", "150", "255") + ": " +
            escColor("255", "145", "0") + logtype.ToUpper() +
            escColor("0", "255", "100") + " | " + escColor()); // so that new lines in the same don't lack the formatting
        Console.WriteLine(
            escColor("0", "255", "0") + DateTime.Now +
            escColor("0", "150", "255") + ": " +
            escColor("255", "145", "0") + logtype.ToUpper() +
            escColor("0", "255", "100") + " | " +
            escColor() + str);
    }
}

