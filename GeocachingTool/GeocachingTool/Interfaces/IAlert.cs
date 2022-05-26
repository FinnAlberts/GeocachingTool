using System.Threading.Tasks;

namespace GeocachingTool
{
    public interface IAlert
    {
        /// <summary>
        /// Display a non-cancelable alert with three buttons
        /// </summary>
        /// <param name="title">The title of the alert</param>
        /// <param name="message">The message of the alert</param>
        /// <param name="firstButton">First option text</param>
        /// <param name="secondthirdButtoncancel">Third option text</param>
        /// <returns>The clicked button</returns>
        Task<string> Display(string title, string message, string firstButton, string secondButton, string thirdButton);
    }
}
