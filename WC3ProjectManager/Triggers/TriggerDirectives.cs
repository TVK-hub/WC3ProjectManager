namespace WC3ProjectManager
{
    public static class TriggerDirectives
    {
        public static void Parse(Trigger trg)
        {
            if (!string.IsNullOrEmpty(trg.Code))
            {
                List<string> lines = new(trg.Code.Split('\n'));
                List<string> code = new();
                foreach (string line in lines)
                {
                    switch (line.Trim().ToLower())
                    {
                        case "//@init":
                            trg.RunOnInit = true;
                            break;
                        case "//@disabled":
                            trg.Enabled = false;
                            break;
                        case "//@initiallyoff":
                            trg.IsInitiallyOff = true;
                            break;

                        default:
                            code.Add(line);
                            break;
                    }
                }
                trg.Code = string.Join("\n", code);
            }
        }
        public static string Generate(Trigger trg)
        {
            string d = "";
            if (trg.RunOnInit)      { d += "//@init" + "\n"; }
            if (!trg.Enabled)       { d += "//@disabled" + "\n"; }
            if (trg.IsInitiallyOff) { d += "//@initiallyoff" + "\n"; }
            return d + trg.Code;
        }
    }
}