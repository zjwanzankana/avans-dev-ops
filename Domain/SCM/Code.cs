namespace Domain.SCM
{
    public class Code
    {
        private string _code;

        public Code(string code)
        {
            this._code = code;
        }

        public string GetCode()
        {
            return this._code;
        }
    }
}