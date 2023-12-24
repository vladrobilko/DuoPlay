namespace DuoPlay.Application.Helpers
{
    public static class TicTakToeBoardHelper
    {
        public static string ConvertToString(this char[] array) => new string(array);

        public static char[] ConvertToCharArray(this string str) => str.ToCharArray();

        public static char[] GetInitialBoard() => new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    }
}
