namespace GeocachingTool
{
    public interface IChangeTheme
    {
        /// <summary>
        /// Enable dark/light theme
        /// </summary>
        /// <param name="_">True for dark theme, false for light theme</param>
        void EnableDarkTheme(bool isDarkTheme);
    }
}
